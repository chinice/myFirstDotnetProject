using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyLearning.Models;

namespace MyLearning.Services
{
    public interface IProductRepository
    {
        public Task<Product> AddProduct(Product product);

        public Task<ICollection<Product>> GetAllProducts();

        public Task<Product> GetProductById(int id);

        public Task<bool> UpdateProduct(Product product);

        public Task<bool> DeleteProduct(Product product);

        public Task<bool> CheckProductExist(int id);
    }
}
