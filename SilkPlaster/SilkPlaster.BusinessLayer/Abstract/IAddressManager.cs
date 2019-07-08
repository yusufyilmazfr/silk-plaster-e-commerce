using SilkPlaster.BusinessLayer.Concrete.Result;
using SilkPlaster.Common.EntityValueObjects;
using SilkPlaster.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.BusinessLayer.Abstract
{
    public interface IAddressManager
    {
        BusinessLayerResult<AddressViewModel> Find(Expression<Func<Address, bool>> where);
        BusinessLayerResult<Address> Insert(AddressViewModel obj);
        BusinessLayerResult<AddressViewModel> Update(AddressViewModel obj);
        BusinessLayerResult<Address> Delete(Address obj);
        Address GetAddressWithMemberId(int addressId, int memberId);
        List<Address> GetAddressesWithMemberId(int memberId);
    }
}
