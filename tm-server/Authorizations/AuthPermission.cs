using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using Microsoft.AspNetCore.Http;
using tm_server.Models;
using tm_server.Services;
using System.Security.Claims;
using System.ComponentModel;

namespace tm_server.Authorizations
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class AuthPermission : Attribute, IAuthorizationFilter
    {
        private readonly IList<string> _permission;
        public AuthPermission(params string[] Permission)
        {
            _permission = Permission ?? Array.Empty<string>();
        }

        public AuthPermission(string Permission)
        {
            _permission = new[] { Permission };
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var allowAnonymous = context
                .ActionDescriptor
                .EndpointMetadata
                .OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous) return;

            var pm = context.HttpContext.RequestServices.GetService(typeof(IPermissionManager)) as IPermissionManager;
            var currUser = context.HttpContext.User;
            var userName = currUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
            var prevent = true;
            IList<string> requiredPermission = new List<string>();
            foreach (var permission in _permission)
            {
                var valid = pm.ValidateAsync(userName, permission);
                valid.Wait();
                if (!valid.Result)
                    requiredPermission.Add(permission);
                else
                    prevent = false;
            }

            if (currUser == null || (_permission.Any() && prevent) )
            {
                context.Result = new JsonResult( new { Message = "Access denied", requiredPermission }) { StatusCode = StatusCodes.Status403Forbidden};
            }
        }
    }
}
