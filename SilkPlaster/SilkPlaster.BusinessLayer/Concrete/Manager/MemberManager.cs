using SilkPlaster.BusinessLayer.Abstract;
using SilkPlaster.BusinessLayer.Concrete.Result;
using SilkPlaster.Common.EntityValueObjects;
using SilkPlaster.Common.HelperClasses;
using SilkPlaster.Common.Message;
using SilkPlaster.Common.Services.Hash;
using SilkPlaster.Common.Services.Mail;
using SilkPlaster.Common.Utilities.PasswordGenerator;
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
        private IHashGeneratorService _hashGeneratorService { get; set; }
        private IMailService _mailService { get; set; }
        private BusinessLayerResult<Member> _layerResult;

        public MemberManager(IUnitOfWork unitOfWork, IMemberDal memberDal, IHashGeneratorService hashGeneratorService, IMailService mailService)
        {
            _memberDal = memberDal;
            _unitOfWork = unitOfWork;
            _hashGeneratorService = hashGeneratorService;
            _mailService = mailService;
            _layerResult = new BusinessLayerResult<Member>();
        }

        public BusinessLayerResult<Member> Register(RegisterViewModel obj)
        {
            _layerResult.Result = GetMemberByEmail(obj.Email);

            if (!ObjectHelper.ObjectIsNull(_layerResult.Result))
            {
                _layerResult.AddError(ErrorMessageCode.EmailAlreadyExists, "Böyle bir E-posta adresi zaten kayıtlıdır.");

                return _layerResult;
            }

            string currentPassword = _hashGeneratorService.GenerateHash(obj.Password);

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

            string currentPassword = _hashGeneratorService.GenerateHash(obj.Password);

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
            string currentPassword = _hashGeneratorService.GenerateHash(password);

            return _memberDal.Find(i => i.Email == email && i.Password == currentPassword);
        }

        public Member GetMemberByPassword(int memberId, string password)
        {
            string currentPassword = _hashGeneratorService.GenerateHash(password);
            return _memberDal.Find(i => i.Id == memberId && i.Password == currentPassword);
        }

        public BusinessLayerResult<Member> SendNewPasswordByEmail(string email)
        {
            Member member = GetMemberByEmail(email);

            if (ObjectHelper.ObjectIsNull(member))
            {
                _layerResult.AddError(ErrorMessageCode.ObjectNotFound, "Böyle bir kullanıcı bulunmamaktadır!");
                return _layerResult;
            }

            string newPassword = GenerateRandomPassword.Generate();

            member.Password = _hashGeneratorService.GenerateHash(newPassword);
            _memberDal.Update(member);

            int count = _unitOfWork.SaveChanges();

            if (count == 0)
                _layerResult.AddError(ErrorMessageCode.FailedToAddRecord, "Yeni parola oluşturulamadı!");

            else
            {
                string body = $"Sayın {member.FirstName} {member.LastName}, yeni parolanız: {newPassword}";
                string to = email;
                string subject = "Nishplas - Yeni Parola";

                _mailService.SendMailAsync(body, to, subject);
            }

            return _layerResult;
        }
    }
}
