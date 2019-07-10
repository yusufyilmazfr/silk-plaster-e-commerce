using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.Common.HelperClasses
{
    public static class EnumHelper
    {
        public static TEnum ConvertValueToEnumObject<TEnum, TValue>(TValue value)
        {
            return (TEnum)(Enum.Parse(typeof(TEnum), value.ToString()));
        }
    }
}
