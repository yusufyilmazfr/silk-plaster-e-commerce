using SilkPlaster.BusinessLayer.Abstract;
using SilkPlaster.BusinessLayer.Concrete.Result;
using SilkPlaster.DataAccessLayer.Abstract;
using SilkPlaster.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.BusinessLayer.Concrete
{
    public class InComingMailManager : IInComingMailManager
    {
        private IInComingMailDal _inComingMailDal { get; set; }

        public InComingMailManager(IInComingMailDal inComingMailDal)
        {
            _inComingMailDal = inComingMailDal;
        }

        public int Insert(InComingMail inComingMail)
        {
            _inComingMailDal.Insert(inComingMail);
            return _inComingMailDal.Save();
        }
    }
}
