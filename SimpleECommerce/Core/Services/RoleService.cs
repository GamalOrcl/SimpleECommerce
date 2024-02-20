using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SimpleECommerce.Core.Entities;
using SimpleECommerce.Core.Helpers;
using SimpleECommerce.Core.Interfaces;

namespace SimpleECommerce.Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<List<ApplicationUser>> SearchUsersAsync(string searchCriteria)
        {
            var users = await _userManager.Users
                .Where(u => u.UserName.Contains(searchCriteria) || u.Email.Contains(searchCriteria))
                .ToListAsync();

            return users;
        }

        public async Task<PaginatedList<ApplicationUser>> GetUsersAsync(int pageNumber, int pageSize)
        {
            var users = await _userManager.Users
                .OrderBy(u => u.UserName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalUsers = await _userManager.Users.CountAsync();

            return new PaginatedList<ApplicationUser>(users, totalUsers, pageNumber, pageSize);
        }

        public async Task<IdentityResult> CreateRoleAsync(string roleName)
        {
            var role = new IdentityRole(roleName);
            return await _roleManager.CreateAsync(role);
        }

        public async Task<IdentityResult> UpdateRoleAsync(IdentityRole role)
        {
            return await _roleManager.UpdateAsync(role);
        }

        public async Task<IdentityResult> DeleteRoleAsync(IdentityRole role)
        {
            return await _roleManager.DeleteAsync(role);
        }

        public async Task<IdentityRole> GetRoleByIdAsync(string roleId)
        {
            return await _roleManager.FindByIdAsync(roleId);
        }

        public async Task<IdentityRole> GetRoleByNameAsync(string roleName)
        {
            return await _roleManager.FindByNameAsync(roleName);
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        public async Task<IList<IdentityRole>> GetAllRolesAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                return await _userManager.GetUsersInRoleAsync(role.Name);
            }
            return null;
        }

        public async Task<IdentityResult> AddUserToRoleAsync(ApplicationUser user, string roleName)
        {
            return await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<IdentityResult> RemoveUserFromRoleAsync(ApplicationUser user, string roleName)
        {
            return await _userManager.RemoveFromRoleAsync(user, roleName);
        }

        public async Task<bool> IsUserInRoleAsync(ApplicationUser user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }


    }
}
