using SilkPlaster.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.DataAccessLayer.Abstract
{
    public interface ICommentDal : IRepository<Comment>
    {
        List<Comment> GetCommentsWithProductsAndMembers();
        Comment GetCommentWithMemberById(int Id);
    }
}
