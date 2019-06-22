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
    public class CommentManager : ManagerBase<Comment>
    {
        BusinessLayerResult<Comment> layerResult = new BusinessLayerResult<Comment>();

        public new BusinessLayerResult<Comment> Insert(Comment obj)
        {
            int count = base.Insert(obj);

            if (count == 0)
            {
                layerResult.AddError(ErrorMessageCode.FailedToAddRecord, "Kayıt eklenemedi!");
            }

            return layerResult;
        }

        public new BusinessLayerResult<Comment> Update(Comment obj)
        {
            layerResult.Result = base.Find(i => i.Id == obj.Id);

            if (layerResult.Result == null)
            {
                layerResult.AddError(ErrorMessageCode.ObjectNotFound, "Böyle bir yorum bulunmamaktadır!");
                return layerResult;
            }

            layerResult.Result.Text = obj.Text;
            layerResult.Result.IsValid = obj.IsValid;
            layerResult.Result.StarCount = obj.StarCount;

            int count = base.Update(layerResult.Result);

            if (count == 0)
            {
                layerResult.AddError(ErrorMessageCode.FailedToModifiedRecord, "Yorum güncellenemedi!");
            }
            return layerResult;
        }
    }
}
