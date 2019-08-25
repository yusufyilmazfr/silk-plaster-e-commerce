using SilkPlaster.BusinessLayer.Abstract;
using SilkPlaster.BusinessLayer.Concrete.Result;
using SilkPlaster.Common.HelperClasses;
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
    public class AdminManager : IAdminManager
    {
        private IAdminDal _adminDal { get; set; }

        public AdminManager(IAdminDal adminDal)
        {
            _adminDal = adminDal;
        }

        public Admin GetAdminWithEmailAndPassword(string email, string password)
        {
            string currentPassword = MD5Helper.Create(password);
            return _adminDal.Find(i => i.Email == email && i.Password == currentPassword);
        }
    }
}
