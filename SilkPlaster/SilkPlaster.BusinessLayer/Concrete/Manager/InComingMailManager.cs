using SilkPlaster.BusinessLayer.Abstract;
using SilkPlaster.BusinessLayer.Concrete.Result;
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
    public class InComingMailManager : IInComingMailManager
    {
        private IInComingMailDal _inComingMailDal { get; set; }
        private IUnitOfWork _unitOfWork { get; set; }

        public InComingMailManager(IInComingMailDal inComingMailDal, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _inComingMailDal = inComingMailDal;
        }

        public int Insert(InComingMail inComingMail)
        {
            _inComingMailDal.Insert(inComingMail);

            return _unitOfWork.SaveChanges();
        }
    }
}
