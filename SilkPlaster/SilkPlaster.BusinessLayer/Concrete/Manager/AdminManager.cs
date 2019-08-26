using SilkPlaster.BusinessLayer.Abstract;
using SilkPlaster.BusinessLayer.Concrete.Result;
using SilkPlaster.Common.HelperClasses;
using SilkPlaster.Common.Services.Hash;
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
        private IHashGeneratorService _hashGeneratorService { get; set; }

        public AdminManager(IAdminDal adminDal, IHashGeneratorService hashGeneratorService)
        {
            _adminDal = adminDal;
            _hashGeneratorService = hashGeneratorService;
        }

        public Admin GetAdminWithEmailAndPassword(string email, string password)
        {
            string currentPassword = _hashGeneratorService.GenerateHash(password);
            return _adminDal.Find(i => i.Email == email && i.Password == currentPassword);
        }
    }
}
