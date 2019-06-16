using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.Common.Message
{
    public enum ErrorMessageCode
    {
        MemberAlreadyExists = 100,
        EmailAlreadyExists = 101,
        FailedToAddRecord = 102,
        ObjectAlreadyExists = 103,
        ObjectNotFound = 104,
        FailedToDeleteRecord = 105
    }
}
