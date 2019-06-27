using SilkPlaster.BusinessLayer.Abstract;
using SilkPlaster.BusinessLayer.Result;
using SilkPlaster.Common.EntityValueObjects;
using SilkPlaster.Common.Message;
using SilkPlaster.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.BusinessLayer
{
    public class AddressManager : ManagerBase<Address>
    {
        BusinessLayerResult<Address> _layerResult = new BusinessLayerResult<Address>();

        public new BusinessLayerResult<Address> Insert(AddressViewModel obj)
        {
            int count = base.Insert(new Address
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

            if (count == 0)
            {
                _layerResult.AddError(ErrorMessageCode.FailedToAddRecord, "Adres eklenemedi!");
            }
            return _layerResult;
        }

        public new BusinessLayerResult<AddressViewModel> Update(AddressViewModel obj)
        {
            BusinessLayerResult<AddressViewModel> layerResult = new BusinessLayerResult<AddressViewModel>();

            Address address = base.Find(i => i.Id == obj.Id && i.MemberId == obj.MemberId);

            if (address == null)
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

            int count = base.Update(address);

            if (count == 0)
            {
                layerResult.AddError(ErrorMessageCode.FailedToModifiedRecord, "Adres güncellenemedi!");
            }

            return layerResult;

        }

        public new BusinessLayerResult<Address> Delete(Address obj)
        {
            Address address = base.Find(i => i.MemberId == obj.MemberId && i.Id == obj.Id);

            if (address == null)
            {
                _layerResult.AddError(ErrorMessageCode.ObjectNotFound, "Böyle bir adres bulunmamaktadır!");
                return _layerResult;
            }

            int count = base.Delete(address);

            if (count == 0)
            {
                _layerResult.AddError(ErrorMessageCode.FailedToDeleteRecord, "Adres silinemedi!");
            }
            return _layerResult;
        }

        public new BusinessLayerResult<AddressViewModel> Find(Expression<Func<Address, bool>> where)
        {
            BusinessLayerResult<AddressViewModel> layerResult = new BusinessLayerResult<AddressViewModel>();

            layerResult.Result = base.ListQueryable()
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

            if (layerResult.Result == null)
            {
                layerResult.AddError(ErrorMessageCode.ObjectNotFound, "Böyle bir adres bulunmamaktadır!");
            }

            return layerResult;
        }
    }
}
