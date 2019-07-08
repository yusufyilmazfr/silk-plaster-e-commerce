using SilkPlaster.BusinessLayer.Concrete.Result;
using SilkPlaster.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.BusinessLayer.Abstract
{
    public interface ISliderManager
    {
        Slider GetSliderById(int SliderId);
        List<Slider> GetAll();
        BusinessLayerResult<Slider> UpdateSlider(Slider slider);
        BusinessLayerResult<Slider> AddSlider(Slider slider);
        int RemoveSlider(Slider slider);
    }
}
