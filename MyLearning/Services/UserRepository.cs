using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyLearning.Models;
using MyLearning.Utils;

namespace MyLearning.Services
{
    public class UserRepository: IUserRepository
    {
        private readonly MyLearningDbContext _myLearningDbContext;
        
        public UserRepository(MyLearningDbContext myLearningDbContext)
        {
            _myLearningDbContext = myLearningDbContext;
        }
        
        public async Task<User> AddUser(User user)
        {
            await _myLearningDbContext.User.AddAsync(user);
            await new DbUtil().SaveChanges(_myLearningDbContext);
            return user;
        }

        public async Task<bool> DeleteUser(User user)
        {
            _myLearningDbContext.User.Remove(user);
            var result = await new DbUtil().SaveChanges(_myLearningDbContext);
            return result;
        }

        public async Task<ICollection<User>> GetAllUsers()
        {
            var users = await _myLearningDbContext.User.OrderByDescending(u => u.Name).ToListAsync();
            return users;
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _myLearningDbContext.User.Where( u => u.Id == id ).FirstOrDefaultAsync();
            return user;
        }

        public async Task<bool> UpdateUser(User user)
        {
            _myLearningDbContext.User.Update(user);
            var result = await new DbUtil().SaveChanges(_myLearningDbContext);
            return result;
        }

        public async Task<bool> CheckUserExist(int id)
        {
            var userExist = await _myLearningDbContext.User.AnyAsync(u => u.Id == id);
            return userExist;
        }

        public async Task<bool> CheckUserExistByUserName(string username)
        {
            var userExist = await _myLearningDbContext.User.AnyAsync(u => u.UserName == username);
            return userExist;
        }
    }
}
