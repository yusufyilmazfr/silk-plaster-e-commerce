using SilkPlaster.BusinessLayer.Abstract;
using SilkPlaster.BusinessLayer.Concrete.Result;
using SilkPlaster.Common.HelperClasses;
using SilkPlaster.Common.Message;
using SilkPlaster.DataAccessLayer.Abstract;
using SilkPlaster.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.BusinessLayer.Concrete
{
    public class WishListManager : IWishListManager
    {
        private IWishListDal _wishListDal { get; set; }

        public WishListManager(IWishListDal wishListDal)
        {
            _wishListDal = wishListDal;
        }

        public BusinessLayerResult<WishList> AddProductInWishList(WishList obj)
        {
            BusinessLayerResult<WishList> layerResult = new BusinessLayerResult<WishList>();

            layerResult.Result = GetWishListItemByMemberId(obj.MemberId, obj.ProductId);

            if (!ObjectHelper.ObjectIsNull(layerResult.Result))
            {
                layerResult.AddError(ErrorMessageCode.ObjectAlreadyExists, "Ürün beğendiklerinize daha önceden eklenmiştir!");
                return layerResult;
            }

            int count = _wishListDal.Insert(obj);

            if (count == 0)
            {
                layerResult.AddError(ErrorMessageCode.FailedToAddRecord, "Ürün eklenemedi!");
            }
            return layerResult;
        }

        public BusinessLayerResult<WishList> DeleteProductInWishList(WishList obj)
        {
            BusinessLayerResult<WishList> layerResult = new BusinessLayerResult<WishList>();

            layerResult.Result = GetWishListItemByMemberId(obj.MemberId, obj.ProductId);

            if (ObjectHelper.ObjectIsNull(layerResult.Result))
            {
                layerResult.AddError(ErrorMessageCode.ObjectNotFound, "Böyle bir ürün bulunmamaktadır!");
                return layerResult;
            }

            int count = _wishListDal.Delete(layerResult.Result);

            if (count == 0)
            {
                layerResult.AddError(ErrorMessageCode.FailedToDeleteRecord, "Ürün silinemedi!");
            }
            return layerResult;
        }

        public WishList GetWishListItemByMemberId(int memberId, int productId)
        {
            return _wishListDal.Find(i => i.MemberId == memberId && i.ProductId == productId);
        }

        public int GetMyWishListCountByMemberId(int memberId)
        {
            return _wishListDal.GetMyWishListCountByMemberId(memberId);
        }

        public List<WishList> GetMyWishListItemsByMemberId(int memberId)
        {
            return _wishListDal.GetMyWishListItemsByMemberId(memberId);
        }
    }
}
