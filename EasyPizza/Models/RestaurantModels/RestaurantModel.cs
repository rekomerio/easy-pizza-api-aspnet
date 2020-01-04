using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyPizza.Models.RestaurantModels
{
    public class RestaurantModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PostalCode { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public string Header { get; set; }
        public float Rating { get; set; }
        public long UserId { get; set; } // Id of the owner account
        public DateTime CreatedAt { get; set; }
        public DateTime EditedAt { get; set; }
    }
}
