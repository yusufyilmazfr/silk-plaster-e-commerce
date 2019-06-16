using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SilkPlaster.UI.Models
{
    public class ProductDetailsModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public decimal LastPrice { get; set; }
        public decimal NewPrice { get; set; }
        public string FirstImage { get; set; }
        public DateTime AddedDate { get; set; }
        public bool IsContinued { get; set; }
        public bool InStock { get; set; }

        public List<ProductImagesModel> Images { get; set; }
        public List<CommentModel> Comments { get; set; }
    }
}