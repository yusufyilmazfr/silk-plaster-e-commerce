using SilkPlaster.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.Entities
{
    [Table("Addresses")]
    public class Address : EntityBase, IEntity
    {
        [Required,
        DisplayName("Ad"),
        StringLength(50, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Name { get; set; }

        [Required]
        public int CountyId { get; set; }
        public County County { get; set; }

        [Required]
        public int MemberId { get; set; }
        public Member Member { get; set; }

        public List<Order> Orders { get; set; }
    }
}
