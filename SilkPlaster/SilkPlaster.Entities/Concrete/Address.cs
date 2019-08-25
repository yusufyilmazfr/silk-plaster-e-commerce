using SilkPlaster.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.Entities.Concrete
{
    [Table("Addresses")]
    public class Address : EntityBase, IEntity
    {
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

        [DisplayName("T.C. No)"),
        StringLength(11, MinimumLength = 11, ErrorMessage = "{0} alanı {1} karakter olmalıdır.")]
        public string CitizenshipNumber { get; set; }

        [DisplayName("Firma Adı* (Fatura Firmaya Tahsis Edilecekse)"),
        StringLength(500, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string CompanyName { get; set; }

        [DisplayName("Vergi Numarası* (Fatura Firmaya Tahsis Edilecekse)"),
        StringLength(11, MinimumLength = 10, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string TaxNumber { get; set; }

        [DisplayName("Vergi Dairesi* (Fatura Firmaya Tahsis Edilecekse)"),
        StringLength(100, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string TaxAdministration { get; set; }

        [Required]
        public int CountyId { get; set; }
        public County County { get; set; }

        [Required]
        public int CityId { get; set; }
        public City City { get; set; }

        [Required]
        public int MemberId { get; set; }
        public Member Member { get; set; }

        public List<Order> Orders { get; set; }
    }
}
