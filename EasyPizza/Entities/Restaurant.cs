using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using EasyPizza.Models;

namespace EasyPizza.Entities
{
    public class Restaurant
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-increment primary key
        public long Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PostalCode { get; set; }
        public string Avatar { get; set; }
        public string Header { get; set; }
        public float Rating { get; set; }
        public long UserId { get; set; } // Id of the owner account
        public DateTime CreatedAt { get; set; }
        public DateTime EditedAt { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
