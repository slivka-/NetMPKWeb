﻿using System.ComponentModel.DataAnnotations;

namespace NetMPK.WebUI.Models
{
    public class RegisterModel : MainViewModel
    {
        public bool registerMessage { get; set; }

        [Required]
        [Display(Name = "Login")]
        [StringLength(25,ErrorMessage ="{0} może mieć maksymalnie 25 znaków.")]
        public string Login { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "{0} musi mieć co najmniej {2} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("Password", ErrorMessage = "Potwierdzenie nie zgadza się z hasłem.")]
        public string ConfirmPassword { get; set; }
    }
}