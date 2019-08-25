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
    [Table("Counties")]
    public class County : EntityBase, IEntity
    {
        [Required,
        DisplayName("İlçe Adı"),
        StringLength(14, ErrorMessage = "İlçe ismi max. 14 karakter olmalıdır!")]
        public string Name { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }
    }
}
