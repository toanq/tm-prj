using Microsoft.AspNetCore.Authorization;

namespace tm_server.Policies.Requirements
{
    public class UserNameMustContainCharacterRequirement : IAuthorizationRequirement
    {
        public UserNameMustContainCharacterRequirement(char requiredCharacter)
        {
            RequiredCharacter = requiredCharacter;
        }
        public char RequiredCharacter {get; set; }
    }
}
