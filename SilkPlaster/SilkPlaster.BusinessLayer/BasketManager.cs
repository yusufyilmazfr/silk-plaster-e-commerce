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
    public class BasketManager : ManagerBase<Basket>
    {
        public new BusinessLayerResult<Basket> Delete(Basket obj)
        {
            BusinessLayerResult<Basket> layerResult = new BusinessLayerResult<Basket>();

            layerResult.Result = base.Find(i => i.MemberId == obj.MemberId && i.ProductId == obj.ProductId);

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
