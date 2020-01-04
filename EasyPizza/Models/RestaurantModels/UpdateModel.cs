using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPizza.Models.RestaurantModels
{
    public class UpdateModel
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string PostalCode { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public string Header { get; set; }
    }
}
