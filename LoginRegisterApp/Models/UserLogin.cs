﻿using System.ComponentModel.DataAnnotations;

namespace LoginRegisterApp.Models
{
    public class UserLogin
    {
        [Required(ErrorMessage = "User name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

    }
}
