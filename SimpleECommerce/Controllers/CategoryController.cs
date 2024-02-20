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
    public class CategoryController : ControllerBase
    {
        private readonly SimpleECommerceDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService; 

        public CategoryController(SimpleECommerceDbContext context, IMapper mapper, IPhotoService photoService)
        {
            _context = context;
            _mapper = mapper;
            _photoService = photoService;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            return Ok(categoryDtos);
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        //[HttpGet("{id}", Name = nameof(GetCategory))]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            var categoryDto = _mapper.Map<CategoryDto>(category);
            return Ok(categoryDto);
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<IActionResult> PostCategory([FromForm] CategoryCreateDto categoryCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
             

            var category = _mapper.Map<Category>(categoryCreateDto);

            // Handle category image upload (if provided)
            if (categoryCreateDto.Image != null)
            {
                var photoResult = await _photoService.AddPhotoAsync(categoryCreateDto.Image);
                category.ImageUrl = photoResult.Url;
            }

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var categoryDto = _mapper.Map<CategoryDto>(category);

            return Ok(new
            {
                data = categoryDto,
                success = true,
                message = "Added successfully"
            });
            //return CreatedAtRoute(nameof(GetCategory), new { id = category.Id }, categoryDto);
        }


        [HttpPut("{Id}")]
        public async Task<IActionResult> PutCategory(int Id, CategoryUpdateDto categoryUpdateDto)
        {
            if (Id != categoryUpdateDto.Id)
            {
                return BadRequest("Id mismatch");
            }

            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == Id);

            if (category == null)
            {
                return NotFound();
            }

            // Map update data (excluding Id for safety)
            _mapper.Map(categoryUpdateDto, category);
      

           

            // Handle category image update (if provided)
            if (categoryUpdateDto.Image != null)
            {
                // Delete the old image if it exists
                if (!string.IsNullOrEmpty(category.ImageUrl))
                {
                    await _photoService.DeletePhotoAsync(Path.GetFileName(category.ImageUrl));
                }

                // Upload and update the new image
                var photoResult = await _photoService.AddPhotoAsync(categoryUpdateDto.Image);
                category.ImageUrl = photoResult.Url;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Categories/5
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteCategory(int Id)
        {
            var category = await _context.Categories
                .Include(c => c.Products) // Eager load child products
                .FirstOrDefaultAsync(c => c.Id == Id);

            if (category == null)
            {
                return NotFound();
            }

            // Require manual deletion of associated products
            if (category.Products.Any())
            {
                return BadRequest("Category has associated products. Please delete or move them before deleting the category.");
            }

            // Delete the category image (if present)
            if (!string.IsNullOrEmpty(category.ImageUrl))
            {
                var filePath = Path.Combine(Environment.CurrentDirectory, "wwwroot", category.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    await _photoService.DeletePhotoAsync(Path.GetFileName(category.ImageUrl));
                }
            }

            // Delete the category from database
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }


    }
}
