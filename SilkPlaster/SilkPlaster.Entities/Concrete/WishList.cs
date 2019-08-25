using SilkPlaster.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.Entities.Concrete
{
    [Table("WishLists")]
    public class WishList : EntityBase, IEntity
    {
        public Member Member { get; set; }
        public int MemberId { get; set; }

        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}
