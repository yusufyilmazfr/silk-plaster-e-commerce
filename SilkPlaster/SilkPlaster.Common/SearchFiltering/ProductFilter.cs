using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.Common.SearchFiltering
{
    public class ProductFilter
    {
        public int? CategoryId { get; set; }
        public bool? IsContinued { get; set; }
        public bool? IsFeatured { get; set; }
    }
}
