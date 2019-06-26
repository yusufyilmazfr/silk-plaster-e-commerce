using SilkPlaster.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.Entities
{
    [Table("OrderStatus")]
    public class OrderStatus : EntityBase, IEntity
    {
        [Required]
        public string Name { get; set; }

        public List<Order> Orders { get; set; }
    }
}
