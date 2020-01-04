﻿using System.ComponentModel.DataAnnotations;

namespace EasyPizza.Models.UserModels
{
    public class AuthenticateModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
