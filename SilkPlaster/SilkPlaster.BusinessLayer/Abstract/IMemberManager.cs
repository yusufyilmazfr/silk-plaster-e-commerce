using SilkPlaster.BusinessLayer.Concrete.Result;
using SilkPlaster.Common.EntityValueObjects;
using SilkPlaster.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.BusinessLayer.Abstract
{
    public interface IMemberManager
    {
        Member GetMemberById(int memberId);
        Member GetMemberByEmail(string email);
        Member GetMemberWithEmailAndPassword(string email, string password);
        Member GetMemberByPassword(int memberId, string password);
        BusinessLayerResult<Member> UpdateMember(Member member);
        BusinessLayerResult<Member> Register(RegisterViewModel obj);
        BusinessLayerResult<Member> CreateRandomPasswordForMemberByEmail(string email);
    }
}
