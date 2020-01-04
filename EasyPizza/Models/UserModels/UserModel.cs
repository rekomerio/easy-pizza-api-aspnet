using System;
using EasyPizza.Entities;

namespace EasyPizza.Models.UserModels
{
    public class UserModel
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public UserGroup UserGroup { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
