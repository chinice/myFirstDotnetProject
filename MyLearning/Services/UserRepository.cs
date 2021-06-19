using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyLearning.Models;

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
            await SaveChange();
            return user;
        }

        public async Task<bool> DeleteUser(User user)
        {
            _myLearningDbContext.User.Remove(user);
            var result = await SaveChange();
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
            var result = await SaveChange();
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

        private async Task<bool> SaveChange()
        {
            var isSaved = await _myLearningDbContext.SaveChangesAsync();
            if (isSaved == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
