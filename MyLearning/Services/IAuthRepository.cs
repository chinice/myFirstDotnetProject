using System.Threading.Tasks;
using MyLearning.Models;

namespace MyLearning.Services
{
    public interface IAuthRepository
    {
        public Task<User> Authenticate(string userName, string password);

        public Task<User> Register(User user);

        public Task<bool> PasswordUpdate(User user);
    }
}