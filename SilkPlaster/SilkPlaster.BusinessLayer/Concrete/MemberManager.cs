using SilkPlaster.BusinessLayer.Abstract;
using SilkPlaster.BusinessLayer.Concrete.Result;
using SilkPlaster.Common.EntityValueObjects;
using SilkPlaster.Common.HelperClasses;
using SilkPlaster.Common.Message;
using SilkPlaster.DataAccessLayer.Abstract;
using SilkPlaster.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.BusinessLayer.Concrete
{
    public class MemberManager : IMemberManager
    {
        private IMemberDal _memberDal { get; set; }
        private BusinessLayerResult<Member> _layerResult;

        public MemberManager(IMemberDal memberDal)
        {
            _memberDal = memberDal;
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
            string currentPassword = MD5Helper.Create(obj.Password);

            _memberDal.Insert(new Member
            {
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                Email = obj.Email,
                Password = currentPassword
            });

            int insertCount = _memberDal.Save();

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
            int count = _memberDal.Save();

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
            int count = _memberDal.Save();

            if (count == 0)
                _layerResult.AddError(ErrorMessageCode.FailedToAddRecord, "Yeni parola oluşturulamadı!");

            _layerResult.Result = member;
            _layerResult.Result.Password = newPassword;

            return _layerResult;
        }
    }
}
