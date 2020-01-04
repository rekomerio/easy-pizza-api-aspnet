using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPizza.Models.RestaurantModels
{
    public class CreateModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Phone { get; set; }
        public string PostalCode { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public string Header { get; set; }
    }
}
