using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.Entities.Concrete
{
    public class EntityBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, Column(TypeName = "datetime2")]
        public DateTime AddedDate { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ModifiedDate { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
