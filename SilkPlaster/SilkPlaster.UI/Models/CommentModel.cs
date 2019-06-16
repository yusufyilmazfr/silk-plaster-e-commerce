using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SilkPlaster.UI.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public byte StarCount { get; set; }
        public int ParentId { get; set; }

        public DateTime AddedDate { get; set; }

        public MemberDetailsModel Member { get; set; }
    }
}