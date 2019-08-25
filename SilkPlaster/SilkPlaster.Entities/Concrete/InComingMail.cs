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
    [Table("InComingMails")]
    public class InComingMail : EntityBase, IEntity
    {
        [Required,
        DisplayName("İsim"),
        MaxLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır!")]
        public string PersonFirstName { get; set; }

        [Required,
        DisplayName("Soyisim"),
        MaxLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır!")]
        public string PersonLastName { get; set; }

        [Required,
        DisplayName("Email"),
        MaxLength(75, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır!")]
        public string PersonEmail { get; set; }

        //public string PhoneNumber { get; set; }

        [Required,
        DisplayName("Konu")]
        public string Subject { get; set; }

        [Required,
        DisplayName("Mesaj")]
        public string Message { get; set; }

    }
}
