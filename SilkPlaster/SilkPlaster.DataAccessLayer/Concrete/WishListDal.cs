using SilkPlaster.DataAccessLayer.Abstract;
using SilkPlaster.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.DataAccessLayer.Concrete
{
    public class WishListDal : EntityRepository<WishList>, IWishListDal
    {
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
