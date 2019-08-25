using SilkPlaster.BusinessLayer.Concrete.Result;
using SilkPlaster.Entities;
using SilkPlaster.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.BusinessLayer.Abstract
{
    public interface ICommentManager
    {
        Comment GetCommentById(int commentId);
        BusinessLayerResult<Comment> AddComment(Comment comment);
        BusinessLayerResult<Comment> UpdateComment(Comment comment);
        List<Comment> GetCommentsWithProductsAndMembers();
        Comment GetCommentWithMemberById(int Id);
        int RemoveComment(Comment comment);
    }
}
