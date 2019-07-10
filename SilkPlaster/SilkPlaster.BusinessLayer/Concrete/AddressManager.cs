using SilkPlaster.BusinessLayer.Abstract;
using SilkPlaster.BusinessLayer.Concrete.Result;
using SilkPlaster.Common.EntityValueObjects;
using SilkPlaster.Common.HelperClasses;
using SilkPlaster.Common.Message;
using SilkPlaster.DataAccessLayer.Abstract;
using SilkPlaster.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.BusinessLayer.Concrete
{
    public class AddressManager : IAddressManager
    {
        public IAddressDal _addressDal { get; set; }
        private BusinessLayerResult<Address> _layerResult { get; set; }

        public AddressManager(IAddressDal addressDal)
        {
            _addressDal = addressDal;
        }

        public BusinessLayerResult<Address> Insert(AddressViewModel obj)
        {
            _layerResult = new BusinessLayerResult<Address>();
            _addressDal.Insert(new Address
            {
                MemberId = obj.MemberId,
                Name = obj.Name,
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                PhoneNumber = obj.PhoneNumber,
                CountyId = obj.CountyId,
                CityId = obj.CityId,
                Description = obj.Description,
                CompanyName = obj.CompanyName,
                TaxAdministration = obj.TaxAdministration,
                TaxNumber = obj.TaxNumber,
                CitizenshipNumber = obj.CitizenshipNumber
            });
            int count = _addressDal.Save();

            if (count == 0)
            {
                _layerResult.AddError(ErrorMessageCode.FailedToAddRecord, "Adres eklenemedi!");
            }
            return _layerResult;
        }

        public BusinessLayerResult<AddressViewModel> Update(AddressViewModel obj)
        {
            BusinessLayerResult<AddressViewModel> layerResult = new BusinessLayerResult<AddressViewModel>();
            Address address = GetAddressWithMemberId(obj.Id, obj.MemberId);

            if (ObjectHelper.ObjectIsNull(address))
            {
                layerResult.AddError(ErrorMessageCode.ObjectNotFound, "Böyle bir adres bulunmamaktadır!");
                return layerResult;
            }

            address.Name = obj.Name;
            address.FirstName = obj.FirstName;
            address.LastName = obj.LastName;
            address.Description = obj.Description;
            address.PhoneNumber = obj.PhoneNumber;
            address.CitizenshipNumber = obj.CitizenshipNumber;
            address.CompanyName = obj.CompanyName;
            address.CityId = obj.CityId;
            address.CountyId = obj.CountyId;
            address.TaxAdministration = obj.TaxAdministration;
            address.TaxNumber = obj.TaxNumber;
            address.Name = obj.Name;

            _addressDal.Update(address);
            int count = _addressDal.Save();

            if (count == 0)
            {
                layerResult.AddError(ErrorMessageCode.FailedToModifiedRecord, "Adres güncellenemedi!");
            }

            return layerResult;

        }

        public BusinessLayerResult<Address> Delete(Address obj)
        {
            Address address = GetAddressWithMemberId(obj.Id, obj.MemberId);
            _layerResult = new BusinessLayerResult<Address>();

            if (ObjectHelper.ObjectIsNull(address))
            {
                _layerResult.AddError(ErrorMessageCode.ObjectNotFound, "Böyle bir adres bulunmamaktadır!");
                return _layerResult;
            }

            _addressDal.Delete(address);
            int count = _addressDal.Save();

            if (count == 0)
            {
                _layerResult.AddError(ErrorMessageCode.FailedToDeleteRecord, "Adres silinemedi!");
            }
            return _layerResult;
        }

        public BusinessLayerResult<AddressViewModel> Find(Expression<Func<Address, bool>> where)
        {
            BusinessLayerResult<AddressViewModel> layerResult = new BusinessLayerResult<AddressViewModel>();

            //WILL BE CORRECT LATER!

            layerResult.Result = _addressDal
                .ListQueryable()
                .Where(where)
                .Select(i => new AddressViewModel
                {
                    Id = i.Id,
                    Name = i.Name,
                    FirstName = i.FirstName,
                    LastName = i.LastName,
                    Description = i.Description,
                    MemberId = i.MemberId,
                    CitizenshipNumber = i.CitizenshipNumber,
                    CompanyName = i.CompanyName,
                    CityId = i.CityId,
                    CountyId = i.CountyId,
                    PhoneNumber = i.PhoneNumber,
                    TaxAdministration = i.TaxAdministration,
                    TaxNumber = i.TaxNumber
                })
                .FirstOrDefault();

            if (ObjectHelper.ObjectIsNull(layerResult.Result))
            {
                layerResult.AddError(ErrorMessageCode.ObjectNotFound, "Böyle bir adres bulunmamaktadır!");
            }

            return layerResult;
        }

        public Address GetAddressWithMemberId(int addressId, int memberId)
        {
            return _addressDal.Find(i => i.Id == addressId && i.MemberId == memberId);
        }

        public List<Address> GetAddressesWithMemberId(int memberId)
        {
            return _addressDal.GetAll(i => i.MemberId == memberId);
        }
    }
}
