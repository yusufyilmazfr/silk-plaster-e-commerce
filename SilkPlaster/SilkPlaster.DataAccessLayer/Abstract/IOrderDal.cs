﻿using SilkPlaster.DataAccessLayer.Abstract.Repository;
using SilkPlaster.Entities;
using SilkPlaster.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.DataAccessLayer.Abstract
{
    public interface IOrderDal : IRepository<Order>
    {
        List<Order> GetAllOrdersWithDetails();
        Order GetOrderDetailById(int Id);
        Order GetOrderDetailMemberId(int orderId, int memberId);
    }
}
