using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyLearning.Models;

namespace MyLearning.Services
{
    public class ProductRepository: IProductRepository
    {
        private readonly MyLearningDbContext _myLearningDbContext;


        public ProductRepository(MyLearningDbContext myLearningDbContext)
        {
            _myLearningDbContext = myLearningDbContext;
        }


        public async Task<Product> AddProduct(Product product)
        {
            await _myLearningDbContext.Product.AddAsync(product);
            await SaveChange();
            return product;

        }


        public async Task<bool> DeleteProduct(Product product)
        {
            _myLearningDbContext.Product.Remove(product);
            var result = await SaveChange();
            return result;

        }


        public async Task<ICollection<Product>> GetAllProducts()
        {
            var products = await _myLearningDbContext.Product.OrderByDescending(p => p.ProductName).ToListAsync();
            return products;
        }


        public async Task<Product> GetProductById(int id)
        {
            var product = await _myLearningDbContext.Product.Where(p => p.Id == id).FirstOrDefaultAsync();
            return product;
        }


        public async Task<bool> UpdateProduct(Product product)
        {
            _myLearningDbContext.Product.Update(product);
            var result = await SaveChange();
            return result;
        }

        public async Task<bool> CheckProductExist(int id)
        {
            var userExist = await _myLearningDbContext.Product.AnyAsync(p => p.Id == id);
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
