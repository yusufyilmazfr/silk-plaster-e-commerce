using SilkPlaster.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.DataAccessLayer.Abstract
{
    public interface IBasketDal : IRepository<Basket>
    {
        List<Basket> GetBasketItemsByMemberId(int memberId);
        void DeleteRange(List<int> IdList);
    }
}
