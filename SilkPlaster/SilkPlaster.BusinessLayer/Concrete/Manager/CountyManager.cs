using SilkPlaster.BusinessLayer.Abstract;
using SilkPlaster.DataAccessLayer.Abstract;
using SilkPlaster.Entities;
using SilkPlaster.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.BusinessLayer.Concrete.Manager
{
    public class CountyManager : ICountyManager
    {
        private ICountyDal _countyDal { get; set; }

        public CountyManager(ICountyDal countyDal)
        {
            _countyDal = countyDal;
        }

        public List<County> GetAllByCityId(int cityId)
        {
            return _countyDal.GetAll(i => i.CityId == cityId);
        }
    }
}
