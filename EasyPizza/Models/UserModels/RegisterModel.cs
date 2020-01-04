using System.ComponentModel.DataAnnotations;
using EasyPizza.Entities;

namespace EasyPizza.Models.UserModels
{
    public class RegisterModel
    {
        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string Password { get; set; }
        // This must be deleted in production!
        public UserGroup UserGroup { get; set; }
    }
}
