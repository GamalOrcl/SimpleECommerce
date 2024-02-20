using Microsoft.AspNetCore.Identity;
using SimpleECommerce.Core.Entities;
using SimpleECommerce.Core.Helpers;

namespace SimpleECommerce.Core.Interfaces
{
    public interface IRoleService
    {
        Task<List<ApplicationUser>> SearchUsersAsync(string searchCriteria);
        Task<PaginatedList<ApplicationUser>> GetUsersAsync(int pageNumber, int pageSize);
        Task<IdentityResult> CreateRoleAsync(string roleName);
        Task<IdentityResult> UpdateRoleAsync(IdentityRole role);
        Task<IdentityResult> DeleteRoleAsync(IdentityRole role);
        Task<IdentityRole> GetRoleByIdAsync(string roleId);
        Task<IdentityRole> GetRoleByNameAsync(string roleName);
        Task<bool> RoleExistsAsync(string roleName);
        Task<IList<IdentityRole>> GetAllRolesAsync();
        Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName);
        Task<IdentityResult> AddUserToRoleAsync(ApplicationUser user, string roleName);
        Task<IdentityResult> RemoveUserFromRoleAsync(ApplicationUser user, string roleName);
        Task<bool> IsUserInRoleAsync(ApplicationUser user, string roleName);

    }
}
