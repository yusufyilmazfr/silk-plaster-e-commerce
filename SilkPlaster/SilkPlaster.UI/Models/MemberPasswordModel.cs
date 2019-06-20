using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SilkPlaster.UI.Models
{
    public class MemberPasswordModel
    {
        [Required,
        DisplayName("Şu anki parola"),
        StringLength(64, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Password { get; set; }

        [Required,
        DisplayName("Yeni parola"),
        StringLength(64, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string NewPassword { get; set; }

        [Required,
        DisplayName("Yeni parola tekrar"),
        Compare("NewPassword", ErrorMessage = "Yeni girdiğiniz parolalar eşleşmiyor."),
        StringLength(64, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string NewRePassword { get; set; }
    }
}