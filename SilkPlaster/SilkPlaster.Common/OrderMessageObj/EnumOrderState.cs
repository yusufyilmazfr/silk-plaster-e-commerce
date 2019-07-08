using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.Common.OrderMessageObj
{
    public enum EnumOrderState
    {
        Waiting = 0,
        Preparing = 1,
        Unpaid = 2,
        Dispatch = 3,
        Completed = 4
    }
}
