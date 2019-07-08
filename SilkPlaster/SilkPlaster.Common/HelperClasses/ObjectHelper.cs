using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.Common.HelperClasses
{
    public static class ObjectHelper
    {
        public static bool ObjectIsNull(object obj)
        {
            return obj == null ? true : false;
        }
    }
}
