using System.Threading.Tasks;
using Datingapp.api.Models;

namespace Datingapp.api.Data
{
    public interface IAuthRepository
    {
         Task<User> Login (string username, string password);
         Task<bool> UserExists(string username);
        Task<User> Register(User user, string password);
    }
}