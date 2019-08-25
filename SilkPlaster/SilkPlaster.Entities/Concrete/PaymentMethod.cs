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
    [Table("PaymentMethods")]
    public class PaymentMethod : EntityBase, IEntity
    {
        [Required,
        DisplayName("Ödeme Yöntemi")]
        public string Name { get; set; }

        public List<Order> Orders { get; set; }
    }
}
