using SilkPlaster.BusinessLayer.Abstract;
using SilkPlaster.BusinessLayer.Concrete.Result;
using SilkPlaster.Common.EntityValueObjects;
using SilkPlaster.Common.HelperClasses;
using SilkPlaster.Common.Message;
using SilkPlaster.DataAccessLayer.Abstract;
using SilkPlaster.DataAccessLayer.Abstract.UnitOfWork;
using SilkPlaster.DataAccessLayer.Concrete.EntityFramework;
using SilkPlaster.DataAccessLayer.Concrete.EntityFramework.Context;
using SilkPlaster.DataAccessLayer.Concrete.EntityFramework.UnitOfWork;
using SilkPlaster.Entities;
using SilkPlaster.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.BusinessLayer.Concrete.Manager
{
    public class MemberManager : IMemberManager
    {
        private IMemberDal _memberDal { get; set; }
        private IUnitOfWork _unitOfWork { get; set; }
        private BusinessLayerResult<Member> _layerResult;

        public MemberManager(IUnitOfWork unitOfWork, IMemberDal memberDal)
        {
            _memberDal = memberDal;
            _unitOfWork = unitOfWork;
            _layerResult = new BusinessLayerResult<Member>();
        }

        public void Test()
        {
            var member = _memberDal.Find(i => i.Id == 1);

            member.FirstName = "Yusuf";
            member.LastName = "Yılmaz";

            _unitOfWork.SaveChanges();


            #region CreateInstance
            //var memberRepository = _unitOfWork.GetRepository<Member>();
            //var member = memberRepository.Find(i => i.Id == 1);

            //member.FirstName = "Selami";

            //_unitOfWork.SaveChanges();

            #endregion

            #region WithConcreteObjects
            //DatabaseContext databaseContext = new DatabaseContext();

            //UnitOfWork unitOfWork = new UnitOfWork(databaseContext);
            //EfMemberDal efMemberDal = new EfMemberDal(databaseContext);
            //EfCategoryDal efCategoryDal = new EfCategoryDal(databaseContext);

            //var member = efMemberDal.Find(i => i.Id == 1);
            //member.FirstName = "Yusuf";
            //member.LastName = "Yılmaz";

            //efCategoryDal.Insert(new Category
            //{
            //});

            //unitOfWork.SaveChanges();
            #endregion
        }

        #region OtherOperations
        public BusinessLayerResult<Member> Register(RegisterViewModel obj)
        {
            _layerResult.Result = GetMemberByEmail(obj.Email);

            if (!ObjectHelper.ObjectIsNull(_layerResult.Result))
            {
                _layerResult.AddError(ErrorMessageCode.EmailAlreadyExists, "Böyle bir E-posta adresi zaten kayıtlıdır.");

                return _layerResult;
            }

            string currentPassword = MD5Helper.Create(obj.Password);

            _memberDal.Insert(new Member
            {
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                Email = obj.Email,
                Password = currentPassword
            });

            int insertCount = _unitOfWork.SaveChanges();

            if (insertCount > 0)
            {
                _layerResult.Result = GetMemberWithEmailAndPassword(obj.Email, obj.Password);
            }
            return _layerResult;
        }

        public BusinessLayerResult<Member> UpdateMember(Member obj)
        {
            Member member = _memberDal.Find(i => i.Id != obj.Id && i.Email == obj.Email); //Daha sonradan extract method haline getirilecek!

            _layerResult.Result = obj;

            if (!ObjectHelper.ObjectIsNull(member))
            {
                if (member.Email == obj.Email)
                {
                    _layerResult.AddError(ErrorMessageCode.EmailAlreadyExists, "Böyle bir E-posta zaten kayıtlı!");
                }
                return _layerResult;
            }

            _layerResult.Result = GetMemberById(obj.Id);

            if (ObjectHelper.ObjectIsNull(_layerResult.Result))
            {
                _layerResult.AddError(ErrorMessageCode.UserNotFound, "Böyle bir kullanıcı bulunmamaktadır!");
                return _layerResult;
            }

            string currentPassword = MD5Helper.Create(obj.Password);

            _layerResult.Result.FirstName = obj.FirstName;
            _layerResult.Result.LastName = obj.LastName;
            _layerResult.Result.Email = obj.Email;
            _layerResult.Result.Password = currentPassword;

            _memberDal.Update(_layerResult.Result);

            int count = _unitOfWork.SaveChanges();

            if (count == 0)
            {
                _layerResult.AddError(ErrorMessageCode.FailedToModifiedRecord, "Kullanıcı düzenlenemedi!");
            }

            return _layerResult;
        }

        public Member GetMemberById(int memberId)
        {
            return _memberDal.Find(i => i.Id == memberId);
        }

        public Member GetMemberByEmail(string email)
        {
            return _memberDal.Find(i => i.Email == email);
        }

        public Member GetMemberWithEmailAndPassword(string email, string password)
        {
            string currentPassword = MD5Helper.Create(password);
            return _memberDal.Find(i => i.Email == email && i.Password == currentPassword);
        }

        public Member GetMemberByPassword(int memberId, string password)
        {
            string currentPassword = MD5Helper.Create(password);
            return _memberDal.Find(i => i.Id == memberId && i.Password == currentPassword);
        }

        public BusinessLayerResult<Member> CreateRandomPasswordForMemberByEmail(string email)
        {
            Member member = GetMemberByEmail(email);

            if (ObjectHelper.ObjectIsNull(member))
            {
                _layerResult.AddError(ErrorMessageCode.ObjectNotFound, "Böyle bir kullanıcı bulunmamaktadır!");
                return _layerResult;
            }

            Random random = new Random();
            string newPassword = random.Next(0, 2600).ToString() + random.Next(5, 98652) + random.Next(6, 982);
            member.Password = MD5Helper.Create(newPassword);

            _memberDal.Update(member);

            int count = _unitOfWork.SaveChanges();

            if (count == 0)
                _layerResult.AddError(ErrorMessageCode.FailedToAddRecord, "Yeni parola oluşturulamadı!");

            _layerResult.Result = member;
            _layerResult.Result.Password = newPassword;

            return _layerResult;
        }

        #endregion
    }
}
