using SilkPlaster.BusinessLayer.Concrete.Result;
using SilkPlaster.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.BusinessLayer.Abstract
{
    public interface IBasketManager
    {
        BusinessLayerResult<Basket> AddProductInBasket(int memberId, int productId, int productCount);
        BusinessLayerResult<Basket> DeleteBasketItem(Basket obj);
        Basket GetBasketItemWithByMemberId(int basketItemId, int memberId);
        Basket GetBasketItemWithByProductId(int productId, int memberId);
        List<Basket> GetBasketItemsByMemberId(int memberId);
        int GetBasketItemsCountByMemberId(int memberId);
        BusinessLayerResult<Basket> DecreaseProductCount(int loggedInMemberId, int productId, int productCount);
        void DeleteBasketItemById(int Id);
        Basket GetBasketItemById(int Id);
    }
}
