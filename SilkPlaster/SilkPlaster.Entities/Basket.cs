using SilkPlaster.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.Entities
{
    [Table("Baskets")]
    public class Basket : EntityBase, IEntity
    {
        public int ProductCount { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int MemberId { get; set; }
        public Member Member { get; set; }
    }
}
