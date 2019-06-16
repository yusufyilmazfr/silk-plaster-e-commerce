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
        public BusinessLayerResult<Member> Register(RegisterViewModel model)
        {
            BusinessLayerResult<Member> layerResult = new BusinessLayerResult<Member>();
            layerResult.Result = Find(i => i.Email == model.Email);

            if (layerResult.Result != null)
            {
                layerResult.AddError(ErrorMessageCode.EmailAlreadyExists, "Böyle bir E-posta adresi zaten kayıtlıdır.");

                return layerResult;
            }

            int insertCount = Insert(new Member
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = model.Password
            });

            if (insertCount > 0)
            {
                layerResult.Result = Find(i => i.Email == model.Email && i.Password == model.Password);
            }
            return layerResult;
        }
    }
}
