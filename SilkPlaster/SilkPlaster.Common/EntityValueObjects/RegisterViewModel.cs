using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.Common.EntityValueObjects
{
    public class RegisterViewModel
    {
        [Required,
        DisplayName("İsim"),
        StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string FirstName { get; set; }

        [Required,
        DisplayName("Soyisim"),
        StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string LastName { get; set; }

        [Required,
        DisplayName("E-posta"),
        StringLength(70, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Email { get; set; }

        [Required,
        DisplayName("Parola"),
        StringLength(64, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Password { get; set; }

        [Required,
        DisplayName("Parola Tekrar"),
        Compare("Password", ErrorMessage = "{0} alanı ile {1} alanı eşleşmemektedir."),
        StringLength(64, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string RePassword { get; set; }

    }
}
