using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tm_server.Models;

namespace tm_server.Services
{
    public interface IPermissionManager
    {
        Task<EntityEntry> CreateAsync(Permission permission);
        Task<bool> ValidateAsync(string user, string permission);
        Task<bool> ValidateAsync(AppUser user, string permission);
        Task<bool> ValidateAsync(AppUser user, Permission permission);
        Task<IList<string>> GetAllOfAsync(AppUser user);
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
        
        public async Task<bool> ValidateAsync(string user, string permission)
        {
            if (permission == null || permission.Length == 0) return false;
            if (user == null || user.Length == 0) return false;

            var currPermission = await _context.Permissions.FirstOrDefaultAsync(p => p.Name == permission);
            var currUser = await _context.Users.FirstOrDefaultAsync(p => p.UserName == user);

            return await CheckUserPermission(currUser, currPermission);
        }

        public async Task<bool> ValidateAsync(AppUser user, string permission) {
            if (permission == null || permission.Length == 0) return false;

            var currPermission = await _context.Permissions.FirstOrDefaultAsync(p => p.Name == permission);
            return await CheckUserPermission(user, currPermission);
        }

        public async Task<bool> ValidateAsync(AppUser user, Permission permission)
        {
            return await CheckUserPermission(user, permission);
        }

        public async Task<IList<string>> GetAllOfAsync(AppUser user)
        {
            return await _context.UserPermissions.Where(up => up.UserId == user.Id)
                .Join(_context.Permissions,
                    up => up.PermissionId,
                    p => p.Id,
                    (up, p) => p.Name).ToListAsync();
        } 

        private async Task<bool> CheckUserPermission(AppUser user, Permission permission)
        {
            var currUserPermission = await _context.UserPermissions.FirstOrDefaultAsync(uprmstn =>
                uprmstn.UserId == user.Id && uprmstn.PermissionId == permission.Id);
            return currUserPermission != null;
        }
    }
}
