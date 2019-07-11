using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.Common.OrderMessageObj
{
    public enum EnumOrderState
    {
        [Display(Name = "Onay Bekliyor.")]
        Waiting = 0,

        [Display(Name = "Ürün Onaylandı, Hazırlanıyor.")]
        Preparing = 1,

        [Display(Name = "Ürün Kargoya Verildi.")]
        SendToCargo = 2,

        [Display(Name = "Ürün Teslim Edildi.")]
        Completed = 3,

        [Display(Name = "Ürün İade Edildi.")]
        Dispatch = 4,

        [Display(Name = "Sipariş İptal Edildi.")]
        Canceled = 5,

    }
}
