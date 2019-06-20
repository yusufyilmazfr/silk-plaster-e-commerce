using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SilkPlaster.UI.Models
{
    public class MemberDetailsModel
    {
        public int Id { get; set; }

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
        DataType(DataType.EmailAddress, ErrorMessage = "{0} alanı E-posta formatında olmalıdır"),
        StringLength(70, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Email { get; set; }

    }
}