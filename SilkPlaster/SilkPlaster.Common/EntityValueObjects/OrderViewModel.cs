using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.Common.EntityValueObjects
{
    public class OrderViewModel
    {
        public string OrderNumber { get; set; }
        public string Description { get; set; }
        public int AddressId { get; set; }
        public int MemberId { get; set; }
        public int OrderState { get; set; }
        public int PaymentType { get; set; }
        public string PaymentId { get; set; }
        public string PaymentToken { get; set; }
        public string ConservationId { get; set; }

        public List<OrderDetailsViewModel> OrderDetails { get; set; }
    }
}
