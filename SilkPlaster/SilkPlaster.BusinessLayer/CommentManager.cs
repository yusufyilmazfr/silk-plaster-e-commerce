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
        public new BusinessLayerResult<Comment> Insert(Comment obj)
        {
            BusinessLayerResult<Comment> layerResult = new BusinessLayerResult<Comment>();

            int count = base.Insert(obj);

            if (count == 0)
            {
                layerResult.AddError(ErrorMessageCode.FailedToAddRecord, "Kayıt eklenemedi!");
            }

            return layerResult;
        }
    }
}
