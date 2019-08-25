using SilkPlaster.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.Entities.Concrete
{
    public class Slider : EntityBase, IEntity
    {
        [Required,
        DisplayName("Ad"),
        MaxLength(50, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Name { get; set; }

        [Required,
        DisplayName("Açıklama"),
        MaxLength(50, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Description { get; set; }

        [Required,
        DisplayName("Fotoğraf")]
        public string Image { get; set; }

        [Required,
        DisplayName("Yönleneceği Adres")]
        public string RedirectAddress { get; set; }
    }
}
