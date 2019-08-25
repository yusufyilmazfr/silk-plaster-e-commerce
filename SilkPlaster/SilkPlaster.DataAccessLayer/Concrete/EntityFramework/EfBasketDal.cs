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
    public class EfBasketDal : EfEntityRepository<Basket>, IBasketDal
    {
        public EfBasketDal(DbContext context) : base(context)
        {
        }

        public void DeleteRange(List<int> IdList)
        {
            foreach (var item in IdList)
            {
                Basket basket = Find(i => i.Id == item);
                Delete(basket);
            }
        }

        public List<Basket> GetBasketItemsByMemberId(int memberId)
        {
            return ListQueryable()
                .Include("Product")
                .Include("Member")
                .Where(i => i.MemberId == memberId)
                .ToList();
        }
    }
}
