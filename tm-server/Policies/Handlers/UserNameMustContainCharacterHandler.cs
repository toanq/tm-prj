using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using tm_server.Policies.Requirements;

namespace tm_server.Policies.Handlers
{
    public class UserNameMustContainCharacterHandler : AuthorizationHandler<UserNameMustContainCharacterRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            UserNameMustContainCharacterRequirement requirement
        )
        {
            var userNameClaim = context.User.FindFirst(u => u.Type == ClaimTypes.Name);
            if (userNameClaim == null) return Task.CompletedTask;

            if (userNameClaim.Value.Contains(requirement.RequiredCharacter))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
