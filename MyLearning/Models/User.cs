using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MyLearning.Models
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public int Status { get; set; } = 1;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public ICollection<Product> Products { get; set; } 
    }
}
