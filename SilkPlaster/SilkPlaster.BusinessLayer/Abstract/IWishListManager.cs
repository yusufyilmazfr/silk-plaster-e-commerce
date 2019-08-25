using SilkPlaster.BusinessLayer.Concrete.Result;
using SilkPlaster.Entities;
using SilkPlaster.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.BusinessLayer.Abstract
{
    public interface IWishListManager
    {
        WishList GetWishListItemByMemberId(int memberId, int productId);
        BusinessLayerResult<WishList> AddProductInWishList(WishList obj);
        BusinessLayerResult<WishList> DeleteProductInWishList(WishList obj);
        int GetMyWishListCountByMemberId(int memberId);
        List<WishList> GetMyWishListItemsByMemberId(int memberId);
    }
}
