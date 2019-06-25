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
    [Table("Cities")]
    public class City : EntityBase, IEntity
    {
        [Required,
        DisplayName("Şehir Adı"),
        StringLength(14, ErrorMessage = "Şehir ismi max. 14 karakter olmalıdır!")]
        public string Name { get; set; }

        public List<County> Counties { get; set; }
    }
}
