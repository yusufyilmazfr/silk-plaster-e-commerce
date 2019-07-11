﻿using SilkPlaster.DataAccessLayer.Abstract;
using SilkPlaster.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.DataAccessLayer.Concrete
{
    public class OrderDal : EntityRepository<Order>, IOrderDal
    {
        public List<Order> GetAllOrdersWithDetails()
        {
            return ListQueryable()
                    .Include(i => i.OrderDetails)
                    .Include(i => i.Member)
                    .Include(i => i.Address)
                    .Include(i => i.Address.City)
                    .Include(i => i.Address.County)
                    .OrderByDescending(i => i.AddedDate)
                    .ToList();
        }

        public Order GetOrderDetailById(int Id)
        {
            return ListQueryable()
                    .Where(i => i.Id == Id)
                    .Include(i => i.Address)
                    .Include(i => i.Address.City)
                    .Include(i => i.Address.County)
                    .Include(i => i.Member)
                    .Include(i => i.OrderDetails)
                    .Include(i => i.OrderDetails.Select(k => k.Product))
                    .FirstOrDefault();
        }

        public Order GetOrderDetailMemberId(int orderId, int memberId)
        {
            return ListQueryable()
                    .Where(i => i.Id == orderId && i.MemberId == memberId)
                    .Include(i => i.Address)
                    .Include(i => i.Address.City)
                    .Include(i => i.Address.County)
                    .Include(i => i.Member)
                    .Include(i => i.OrderDetails)
                    .Include(i => i.OrderDetails.Select(k => k.Product))
                    .FirstOrDefault();
        }
    }
}
