using SilkPlaster.DataAccessLayer.Abstract;
using SilkPlaster.DataAccessLayer.Concrete.EntityFramework.Context;
using SilkPlaster.DataAccessLayer.Concrete.EntityFramework.Repository;
using SilkPlaster.Entities;
using SilkPlaster.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.DataAccessLayer.Concrete.EntityFramework
{
    public class EfCommentDal : EfEntityRepository<Comment>, ICommentDal
    {
        public EfCommentDal(DbContext context) : base(context)
        {
        }

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
