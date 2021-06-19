using System;
using System.ComponentModel.DataAnnotations;

namespace MyLearning.Models
{
    public class Product
    {
        public Product()
        {
        }


        public int Id { get; set; }

        [MaxLength(160)]
        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public decimal ProductAmount { get; set; }

        public int Quantity { get; set; } = 0;

        public int Status { get; set; } = 1;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public User User { get; set; }
    }
}
