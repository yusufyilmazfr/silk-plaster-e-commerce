using SilkPlaster.BusinessLayer.Abstract;
using SilkPlaster.BusinessLayer.Result;
using SilkPlaster.Common.Message;
using SilkPlaster.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.BusinessLayer
{
    public class WishListManager : ManagerBase<WishList>
    {
        public new BusinessLayerResult<WishList> Insert(WishList obj)
        {
            BusinessLayerResult<WishList> layerResult = new BusinessLayerResult<WishList>();

            layerResult.Result = base.Find(i => i.MemberId == obj.MemberId && i.ProductId == obj.ProductId);

            if (layerResult.Result != null)
            {
                layerResult.AddError(ErrorMessageCode.ObjectAlreadyExists, "Ürün beğendiklerinize daha önceden eklenmiştir!");
                return layerResult;
            }

            int count = base.Insert(obj);

            if (count == 0)
            {
                layerResult.AddError(ErrorMessageCode.FailedToAddRecord, "Ürün eklenemedi!");
            }
            return layerResult;
        }

        public new BusinessLayerResult<WishList> Delete(WishList obj)
        {
            BusinessLayerResult<WishList> layerResult = new BusinessLayerResult<WishList>();

            layerResult.Result = base.Find(i => i.ProductId == obj.ProductId && i.MemberId == obj.MemberId);

            if (layerResult.Result == null)
            {
                layerResult.AddError(ErrorMessageCode.ObjectNotFound, "Böyle bir ürün bulunmamaktadır!");
                return layerResult;
            }

            int count = base.Delete(layerResult.Result);

            if (count == 0)
            {
                layerResult.AddError(ErrorMessageCode.FailedToDeleteRecord, "Ürün silinemedi!");
            }
            return layerResult;
        }
    }
}
