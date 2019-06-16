using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.Common.Message
{
    public class ErrorMessageObject
    {
        public ErrorMessageCode ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
