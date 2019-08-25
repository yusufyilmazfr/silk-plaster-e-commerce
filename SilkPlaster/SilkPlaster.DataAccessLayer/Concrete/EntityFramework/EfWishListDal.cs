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
    public class EfWishListDal : EfEntityRepository<WishList>, IWishListDal
    {
        public EfWishListDal(DbContext context) : base(context)
        {
        }

        public int GetMyWishListCountByMemberId(int memberId)
        {
            return ListQueryable().Include("Product").Include("Member").Where(i => i.Member.Id == memberId).Count();
        }

        public List<WishList> GetMyWishListItemsByMemberId(int memberId)
        {
            return ListQueryable().Include("Product").Include("Member").Where(i => i.Member.Id == memberId).ToList();
        }
    }
}
