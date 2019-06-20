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
    public class SliderManager : ManagerBase<Slider>
    {
        BusinessLayerResult<Slider> layerResult = new BusinessLayerResult<Slider>();

        public new BusinessLayerResult<Slider> Update(Slider obj)
        {
            Slider slider = base.Find(i => i.Id == obj.Id);

            if (slider == null)
            {
                layerResult.AddError(ErrorMessageCode.ObjectNotFound, "Böyle bir slider bulunmamaktadır!");
                return layerResult;
            }

            slider.Name = obj.Name;
            slider.Description = obj.Description;
            slider.RedirectAddress = obj.RedirectAddress;
            slider.Image = obj.Image;
            slider.AddedDate = obj.AddedDate;

            int count = base.Update(slider);

            if (count == 0)
            {
                layerResult.AddError(ErrorMessageCode.FailedToModifiedRecord, "slider düzenlenemedi!");
            }
            return layerResult;

        }

        public new BusinessLayerResult<Slider> Insert(Slider obj)
        {
            int count = base.Insert(obj);

            if (count == 0)
            {
                layerResult.AddError(ErrorMessageCode.FailedToAddRecord, "Slider eklenemedi!");
                return layerResult;
            }

            layerResult.Result = obj;
            return layerResult;
        }
    }
}
