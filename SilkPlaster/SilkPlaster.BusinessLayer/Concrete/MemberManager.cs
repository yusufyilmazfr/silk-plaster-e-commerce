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

            int insertCount = _memberDal.Insert(new Member
            {
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                Email = obj.Email,
                Password = obj.Password
            });

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

            _layerResult.Result.FirstName = obj.FirstName;
            _layerResult.Result.LastName = obj.LastName;
            _layerResult.Result.Email = obj.Email;
            _layerResult.Result.Password = obj.Password;

            int count = _memberDal.Update(_layerResult.Result);

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
            return _memberDal.Find(i => i.Email == email && i.Password == password);
        }

        public Member GetMemberByPassword(int memberId, string password)
        {
            return _memberDal.Find(i => i.Id == memberId && i.Password == password);
        }
    }
}
