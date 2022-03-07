using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;
using tm_server.Models;

namespace tm_server.Services
{
    public interface IPermissionManager
    {
        Task<EntityEntry> CreateAsync(Permission permission);
        Task<bool> ValidateAsync(AppUser user, Permission permission);
        Task<bool> ValidateAsync(AppUser user, string permission);
    }
    public class PermissionManager : IPermissionManager
    {
        private readonly TMContext _context;
        public PermissionManager(
            TMContext context
        ) {
            _context = context;
        }
        public async Task<EntityEntry> CreateAsync(Permission permission)
        {
            return await _context.Permissions.AddAsync(permission);
        }

        public async Task<bool> ValidateAsync(AppUser user, Permission permission)
        {
            return await FindUserPermission(user, permission);
        }

        public async Task<bool> ValidateAsync(AppUser user, string permission) {
            if (permission == null || permission.Length == 0) return false;

            var currPermission = await _context.Permissions.FirstOrDefaultAsync(p => p.Name == permission);

            return await FindUserPermission(user, currPermission);
        }

        private async Task<bool> FindUserPermission(AppUser user, Permission permission)
        {
            var currUserPermission = await _context.UserPermissions.FirstOrDefaultAsync(uprmstn =>
                uprmstn.UserId == user.Id && uprmstn.PermissionId == permission.Id);
            return currUserPermission != null;
        }
    }
}
