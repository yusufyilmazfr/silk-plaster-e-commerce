using SilkPlaster.DataAccessLayer.Abstract;
using SilkPlaster.DataAccessLayer.Concrete.EntityFramework.Context;
using SilkPlaster.DataAccessLayer.Concrete.EntityFramework.Repository;
using SilkPlaster.Entities;
using SilkPlaster.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.DataAccessLayer.Concrete.EntityFramework
{
    public class EfOrderDal : EfEntityRepository<Order>, IOrderDal
    {
        public EfOrderDal(DbContext context) : base(context)
        {
        }

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
