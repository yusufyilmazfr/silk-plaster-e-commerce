using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SilkPlaster.UI.Models.Session
{
    public class MemberSessionModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}