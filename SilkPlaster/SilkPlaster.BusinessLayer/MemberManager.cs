using SilkPlaster.BusinessLayer.Abstract;
using SilkPlaster.BusinessLayer.Result;
using SilkPlaster.Common.EntityValueObjects;
using SilkPlaster.Common.Message;
using SilkPlaster.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.BusinessLayer
{
    public class MemberManager : ManagerBase<Member>
    {
        public BusinessLayerResult<Member> Register(RegisterViewModel obj)
        {
            BusinessLayerResult<Member> layerResult = new BusinessLayerResult<Member>();
            layerResult.Result = Find(i => i.Email == obj.Email);

            if (layerResult.Result != null)
            {
                layerResult.AddError(ErrorMessageCode.EmailAlreadyExists, "Böyle bir E-posta adresi zaten kayıtlıdır.");

                return layerResult;
            }

            int insertCount = base.Insert(new Member
            {
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                Email = obj.Email,
                Password = obj.Password
            });

            if (insertCount > 0)
            {
                layerResult.Result = Find(i => i.Email == obj.Email && i.Password == obj.Password);
            }
            return layerResult;
        }

        public new BusinessLayerResult<Member> Update(Member obj)
        {
            BusinessLayerResult<Member> layerResult = new BusinessLayerResult<Member>();
            Member member = base.Find(i => i.Id != obj.Id && i.Email == obj.Email);

            layerResult.Result = obj;

            if (member != null)
            {
                if (member.Email == obj.Email)
                {
                    layerResult.AddError(ErrorMessageCode.EmailAlreadyExists, "Böyle bir E-posta zaten kayıtlı!");
                }
                return layerResult;
            }

            layerResult.Result = base.Find(i => i.Id == obj.Id);

            if (layerResult.Result == null)
            {
                layerResult.AddError(ErrorMessageCode.UserNotFound, "Böyle bir kullanıcı bulunmamaktadır!");
                return layerResult;
            }

            layerResult.Result.FirstName = obj.FirstName;
            layerResult.Result.LastName = obj.LastName;
            layerResult.Result.Email = obj.Email;
            layerResult.Result.Password = obj.Password;

            int count = base.Update(layerResult.Result);

            if (count == 0)
            {
                layerResult.AddError(ErrorMessageCode.FailedToModifiedRecord, "Kullanıcı düzenlenemedi!");
            }

            return layerResult;
        }
    }
}
