using SilkPlaster.DataAccessLayer.Abstract;
using SilkPlaster.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.DataAccessLayer.Concrete
{
    public class CommentDal : EntityRepository<Comment>, ICommentDal
    {
        public List<Comment> GetCommentsWithProductsAndMembers()
        {
            return ListQueryable()
                    .Include("Product")
                    .Include("Member")
                    .OrderByDescending(i => i.AddedDate)
                    .ToList();
        }

        public Comment GetCommentWithMemberById(int Id)
        {
            return ListQueryable()
                .Include("Member")
                .Where(i => i.Id == Id)
                .FirstOrDefault();
        }
    }
}
