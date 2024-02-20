using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleECommerce.Core.Dtos;
using SimpleECommerce.Core.Entities;
using SimpleECommerce.Core.Interfaces;
using SimpleECommerce.DataAccess;

namespace SimpleECommerce.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly SimpleECommerceDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public ProductController(SimpleECommerceDbContext context, IMapper mapper, IPhotoService photoService)
        {
            _context = context;
            _mapper = mapper;
            _photoService = photoService;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.Products
                .Include(p => p.Category) // Eager load category data
                .ToListAsync();
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            return Ok(productDtos);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category) // Eager load category data
                .Include(p => p.Images) // Eager load product images
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var productDto = _mapper.Map<ProductDto>(product);
            return Ok(productDto);
        }

        // POST: api/Products
        // POST: api/Products

        [HttpPost]
        public async Task<IActionResult> PostProduct([FromForm] ProductCreateDto productCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = _mapper.Map<Product>(productCreateDto);

            // Handle main product image upload (if provided)
            if (productCreateDto.MainImage != null)
            {
                var photoResult = await _photoService.AddPhotoAsync(productCreateDto.MainImage);
                product.MainImageUrl = photoResult.Url;
            }

            // Handle additional product image uploads (if provided)
            if (productCreateDto.AdditionalImages != null)
            {
                product.Images = new List<ProductImage>();
                foreach (var image in productCreateDto.AdditionalImages)
                {
                    var photoResult = await _photoService.AddPhotoAsync(image);
                    var productImage = new ProductImage { Url = photoResult.Url, ProductId = product.Id };
                    product.Images.Add(productImage);
                }
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var productDto = _mapper.Map<ProductDto>(product);

            //return CreatedAtRoute(nameof(GetProduct), new { id = product.Id }, productDto);
            return Ok(new
            {
                data = productDto,
                success = true,
                message = "Added successfully"
            });
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductUpdateDto productUpdateDto)
        {
            if (id != productUpdateDto.Id)
            {
                return BadRequest("Id mismatch");
            }

            var product = await _context.Products
                .Include(p => p.Images) // Eager load product images for update
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            // Handle main product image update (if provided)
            if (productUpdateDto.MainImage != null)
            {
                // Delete the old image if it exists
                if (!string.IsNullOrEmpty(product.MainImageUrl))
                {
                    await _photoService.DeletePhotoAsync(Path.GetFileName(product.MainImageUrl));
                }

                // Upload and update the new image
                var photoResult = await _photoService.AddPhotoAsync(productUpdateDto.MainImage);
                product.MainImageUrl = photoResult.Url;
            }

            // Handle additional product image updates (if provided)
            if (productUpdateDto.AdditionalImagesToRemove != null)
            {
                foreach (var imageToRemove in productUpdateDto.AdditionalImagesToRemove)
                {
                    var existingImage = product.Images.FirstOrDefault(i => i.Url == imageToRemove);
                    if (existingImage != null)
                    {
                        // Delete the image from storage
                        await _photoService.DeletePhotoAsync(Path.GetFileName(existingImage.Url));

                        // Remove the image from the product entity
                        product.Images.Remove(existingImage);
                    }
                }
            }

            // Add new additional product images (if provided)
            if (productUpdateDto.AdditionalImages != null)
            {
                foreach (var image in productUpdateDto.AdditionalImages)
                {
                    var photoResult = await _photoService.AddPhotoAsync(image);
                    var productImage = new ProductImage { Url = photoResult.Url, ProductId = product.Id };
                    product.Images.Add(productImage);
                }
            }

            // Update other product properties
            _mapper.Map(productUpdateDto, product);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest("Concurrent update conflict. Please refresh and try again.");
                }
            }

            return NoContent();
        }

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteProduct(int id)
        //{
        //    var product = await _context.Products
        //        .Include(p => p.Images) // Eager load product images for deletion
        //        .FirstOrDefaultAsync(p => p.Id == id);

        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    // Handle associated product images (manual selection for deletion)
        //    var imagesToDelete = new List<ProductImage>();
        //    foreach (var image in product.Images)
        //    {
        //        // Implement your logic to decide whether to delete the image (e.g., check a flag)
        //        if (// your deletion logic here)
        //{
        //            imagesToDelete.Add(image);
        //            await _photoService.DeletePhotoAsync(Path.GetFileName(image.Url)); // Delete image file
        //        }
        //    }

        //    // Handle main image deletion (if present)
        //    if (!string.IsNullOrEmpty(product.MainImageUrl))
        //    {
        //        var mainImageFilename = Path.GetFileName(product.MainImageUrl);
        //        await _photoService.DeletePhotoAsync(mainImageFilename);
        //    }

        //    // Remove selected product images (if any)
        //    if (imagesToDelete.Any())
        //    {
        //        _context.ProductImages.RemoveRange(imagesToDelete);
        //    }

        //    // Remove the product itself
        //    _context.Products.Remove(product);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteProduct(int id)
        //{
        //    var product = await _context.Products
        //        .Include(p => p.Images) // Eager load associated images
        //        .FirstOrDefaultAsync(p => p.Id == id);

        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    // Manually delete associated product images
        //    foreach (var image in product.Images)
        //    {
        //        await _photoService.DeletePhotoAsync(Path.GetFileName(image.Url));
        //    }

        //    // Handle main image deletion (if present)
        //    if (!string.IsNullOrEmpty(product.MainImageUrl))
        //    {
        //        var mainImageFilename = Path.GetFileName(product.MainImageUrl);
        //        await _photoService.DeletePhotoAsync(mainImageFilename);
        //    }

        //    // Remove the product
        //    _context.Products.Remove(product);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Images) // Eager load associated images
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            // Manually delete associated product images
            foreach (var image in product.Images.ToList()) // Create a copy to avoid modification during iteration
            {
                await _photoService.DeletePhotoAsync(Path.GetFileName(image.Url)); // Delete image file
                _context.ProductImages.Remove(image); // Remove from database
            }

            // Handle main image deletion (if present)
            if (!string.IsNullOrEmpty(product.MainImageUrl))
            {
                var mainImageFilename = Path.GetFileName(product.MainImageUrl);
                await _photoService.DeletePhotoAsync(mainImageFilename);
            }

            // Remove the product
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }


    }
}