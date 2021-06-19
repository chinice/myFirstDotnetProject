using System;
using System.ComponentModel.DataAnnotations;

namespace MyLearning.Dtos
{
    public class ProductCreateDto
    {
        [Required(ErrorMessage = "Product name is required")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Product description is required")]
        public string ProductDescription { get; set; }

        [Required(ErrorMessage = "Product amount is required")]
        public decimal ProductAmount { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        public int Quantity { get; set; } = 0;
    }
}
