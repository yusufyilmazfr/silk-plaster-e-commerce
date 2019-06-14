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
    [Table("Orders")]
    public class Order : EntityBase, IEntity
    {
        [Required,
        DisplayName("Sipariş No"),
        StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string OrderNumber { get; set; }

        [Required,
        DisplayName("Adres"),
        StringLength(250, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Address { get; set; }

        [Required,
        DisplayName("Açıklama"),
        StringLength(250, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Description { get; set; }

        public string Status { get; set; }

        [Required]
        public int MemberId { get; set; }
        public Member Member { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }
    }

}
