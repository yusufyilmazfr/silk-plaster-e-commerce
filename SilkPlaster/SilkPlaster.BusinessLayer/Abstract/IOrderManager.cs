using SilkPlaster.BusinessLayer.Concrete.Result;
using SilkPlaster.Common.EntityValueObjects;
using SilkPlaster.Common.OrderMessageObj;
using SilkPlaster.Entities;
using SilkPlaster.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.BusinessLayer.Abstract
{
    public interface IOrderManager
    {
        Order GetOrderById(int Id);
        Order GetOrderDetailById(int Id);
        List<Order> GetOrdersByMemberId(int Id);
        Order GetOrderDetailByMemberId(int orderId, int memberId);
        List<Order> GetAllOrdersWithDetails();
        void CreateNewOrder(OrderViewModel orderViewModel);
        BusinessLayerResult<Order> CheckOrder(OrderViewModel orderViewModel);
        BusinessLayerResult<Order> ChangeTrackingNumberAndState(int orderId, string orderTrackingNumber, EnumOrderState orderState);
    }
}
