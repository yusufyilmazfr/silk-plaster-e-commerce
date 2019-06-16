using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SilkPlaster.UI.Models
{
    public class BasketModel
    {
        public int Id { get; set; }
        public ProductDetailsModel Product { get; set; }
        public int Quantity { get; set; }
    }
}