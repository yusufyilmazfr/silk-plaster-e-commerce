using SilkPlaster.BusinessLayer.Concrete.Result;
using SilkPlaster.Common.EntityValueObjects;
using SilkPlaster.Entities;
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
        List<Order> GetAllOrdersWithDetails();
        void CreateNewOrder(OrderViewModel orderViewModel);
        BusinessLayerResult<Order> CheckOrder(OrderViewModel orderViewModel);
    }
}
