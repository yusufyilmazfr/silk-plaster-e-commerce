using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.Common.EntityValueObjects
{
    public class AddressViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required,
        DisplayName("Adres Başlığı"),
        StringLength(50, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Name { get; set; }

        [Required,
        DisplayName("İsim"),
        StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string FirstName { get; set; }

        [Required,
        DisplayName("Soyisim"),
        StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string LastName { get; set; }

        [Required,
        DisplayName("Adres"),
        StringLength(250, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Telefon Numarası")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "{0} alanı 5xxxxxxxxx formatında olmalıdır!")]
        public string PhoneNumber { get; set; }

        [Required]
        [DisplayName("T.C. No"),
        StringLength(11, MinimumLength = 11, ErrorMessage = "{0} alanı {1} karakter olmalıdır.")]
        public string CitizenshipNumber { get; set; }

        [Required]
        [DisplayName("Firma Adı"),
        StringLength(500, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string CompanyName { get; set; }

        [Required]
        [DisplayName("Vergi Numarası"),
        StringLength(10, MinimumLength = 10, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string TaxNumber { get; set; }

        [Required]
        [DisplayName("Vergi Dairesi"),
        StringLength(100, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string TaxAdministration { get; set; }

        [Required]
        public int CountyId { get; set; }

        [Required]
        public int CityId { get; set; }

        [Required]
        public int MemberId { get; set; }
    }
}
