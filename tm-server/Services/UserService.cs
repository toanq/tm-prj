using tm_server.Models;

namespace tm_server.Services
{
    public interface IUserService
    {
        AppUser GetUserById(string usrId);
    }
    public class UserService : IUserService
    {
        private readonly TMContext _context;
        public UserService(TMContext context)
        {
            _context = context;
        }

        public AppUser GetUserById(string usrId)
        {
            return _context.Users.Find(usrId);
        }
    }
}
