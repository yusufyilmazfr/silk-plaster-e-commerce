using SilkPlaster.BusinessLayer.Abstract;
using SilkPlaster.BusinessLayer.Concrete.Result;
using SilkPlaster.Common.HelperClasses;
using SilkPlaster.Common.Message;
using SilkPlaster.DataAccessLayer.Abstract;
using SilkPlaster.DataAccessLayer.Abstract.UnitOfWork;
using SilkPlaster.Entities;
using SilkPlaster.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.BusinessLayer.Concrete.Manager
{
    public class SliderManager : ISliderManager
    {
        private ISliderDal _sliderDal { get; set; }
        private IUnitOfWork _unitOfWork { get; set; }
        private BusinessLayerResult<Slider> _layerResult { get; set; }

        public SliderManager(ISliderDal sliderDal, IUnitOfWork unitOfWork)
        {
            _sliderDal = sliderDal;
            _unitOfWork = unitOfWork;
            _layerResult = new BusinessLayerResult<Slider>();
        }

        public BusinessLayerResult<Slider> UpdateSlider(Slider obj)
        {
            Slider slider = GetSliderById(obj.Id);

            if (ObjectHelper.ObjectIsNull(slider))
            {
                _layerResult.AddError(ErrorMessageCode.ObjectNotFound, "Böyle bir slider bulunmamaktadır!");
                return _layerResult;
            }

            slider.Name = obj.Name;
            slider.Description = obj.Description;
            slider.RedirectAddress = obj.RedirectAddress;
            slider.Image = obj.Image;
            slider.AddedDate = obj.AddedDate;

            _sliderDal.Update(slider);

            int count = _unitOfWork.SaveChanges();

            if (count == 0)
            {
                _layerResult.AddError(ErrorMessageCode.FailedToModifiedRecord, "slider düzenlenemedi!");
            }

            return _layerResult;
        }

        public BusinessLayerResult<Slider> AddSlider(Slider obj)
        {
            _sliderDal.Insert(obj);

            int count = _unitOfWork.SaveChanges();

            if (count == 0)
            {
                _layerResult.AddError(ErrorMessageCode.FailedToAddRecord, "Slider eklenemedi!");
                return _layerResult;
            }

            _layerResult.Result = obj;
            return _layerResult;
        }

        public Slider GetSliderById(int SliderId)
        {
            return _sliderDal.Find(i => i.Id == SliderId);
        }

        public List<Slider> GetAll()
        {
            return _sliderDal.GetAll();
        }

        public int RemoveSlider(Slider slider)
        {
            _sliderDal.Delete(slider);

            return _unitOfWork.SaveChanges();
        }
    }
}
