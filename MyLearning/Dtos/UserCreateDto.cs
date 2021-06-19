using System;
using System.ComponentModel.DataAnnotations;

namespace MyLearning.Dtos
{
    public class UserCreateDto
    {
        [Required(ErrorMessage = "Username is required")]
        [MinLength(5, ErrorMessage = "Username cannot be less than 5 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Name is required")]
        //[RegularExpression(@"^[A-Z]")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
    }
}
