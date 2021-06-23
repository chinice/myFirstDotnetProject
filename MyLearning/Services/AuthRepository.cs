using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyLearning.Models;
using MyLearning.Utils;

namespace MyLearning.Services
{
    public class AuthRepository: IAuthRepository
    {
        private readonly MyLearningDbContext _myLearningDbContext;
        
        public AuthRepository(MyLearningDbContext myLearningDbContext)
        {
            _myLearningDbContext = myLearningDbContext;
        }
        
        public async Task<User> Authenticate(string userName, string password)
        {
            var user = await _myLearningDbContext.User.SingleOrDefaultAsync(u => u.UserName == userName && u.Password == password);
            return user;
        }

        public async Task<User> Register(User user)
        {
            await _myLearningDbContext.User.AddAsync(user);
            await new DbUtil().SaveChanges(_myLearningDbContext);
            return user;
        }

        public async Task<bool> PasswordUpdate(User user)
        {
            _myLearningDbContext.User.Update(user);
            var result = await new DbUtil().SaveChanges(_myLearningDbContext);
            return result;
        }
    }
}