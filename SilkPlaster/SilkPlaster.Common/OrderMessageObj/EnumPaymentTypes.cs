using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.Common.OrderMessageObj
{
    public enum EnumPaymentTypes
    {
        //[Display(Name = "Kredi Kartı")]
        //CreditCart = 1,

        [Display(Name = "Kapıda Ödeme")]
        PayAtTheDoor = 2,

        //[Display(Name = "EFT/Havale")]
        //EFT = 3
    }
}
