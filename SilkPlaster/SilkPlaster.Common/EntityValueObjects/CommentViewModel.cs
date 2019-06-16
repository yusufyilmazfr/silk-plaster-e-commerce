using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.Common.EntityValueObjects
{
    public class CommentViewModel
    {
        [Required]
        public string Text { get; set; }
        [Required]
        public int StarCount { get; set; }

        public int ParentId { get; set; }

        [Required]
        public int MemberId { get; set; }
        [Required]
        public int ProductId { get; set; }
    }
}
