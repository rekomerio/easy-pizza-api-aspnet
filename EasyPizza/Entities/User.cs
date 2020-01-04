using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyPizza.Entities
{
    public enum UserGroup
    {
        Admin,
        RestaurantOwner,
        Normal
    }
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-increment primary key
        public long Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public UserGroup UserGroup { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
