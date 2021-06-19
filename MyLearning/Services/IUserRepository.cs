using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyLearning.Models;

namespace MyLearning.Services
{
    public interface IUserRepository
    {
        public Task<User> AddUser(User user);

        public Task<bool> UpdateUser(User user);

        public Task<User> GetUser(int id);

        public Task<ICollection<User>> GetAllUsers();

        public Task<bool> DeleteUser(User user);

        public Task<bool> CheckUserExist(int id);

        public Task<bool> CheckUserExistByUserName(string username);
    }
}
