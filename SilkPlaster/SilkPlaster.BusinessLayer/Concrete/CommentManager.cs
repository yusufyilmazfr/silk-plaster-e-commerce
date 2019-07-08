using SilkPlaster.BusinessLayer.Abstract;
using SilkPlaster.BusinessLayer.Concrete.Result;
using SilkPlaster.Common.HelperClasses;
using SilkPlaster.Common.Message;
using SilkPlaster.DataAccessLayer.Abstract;
using SilkPlaster.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.BusinessLayer.Concrete
{
    public class CommentManager : ICommentManager
    {
        private ICommentDal _commentDal { get; set; }
        private BusinessLayerResult<Comment> _layerResult { get; set; }

        public CommentManager(ICommentDal commentDal)
        {
            _commentDal = commentDal;
            _layerResult = new BusinessLayerResult<Comment>();
        }

        public BusinessLayerResult<Comment> AddComment(Comment comment)
        {
            int count = _commentDal.Insert(comment);

            if (count == 0)
            {
                _layerResult.AddError(ErrorMessageCode.FailedToAddRecord, "Kayıt eklenemedi!");
            }

            return _layerResult;
        }

        public BusinessLayerResult<Comment> UpdateComment(Comment comment)
        {
            _layerResult.Result = GetCommentById(comment.Id);

            if (ObjectHelper.ObjectIsNull(_layerResult.Result))
            {
                _layerResult.AddError(ErrorMessageCode.ObjectNotFound, "Böyle bir yorum bulunmamaktadır!");
                return _layerResult;
            }

            _layerResult.Result.Text = comment.Text;
            _layerResult.Result.IsValid = comment.IsValid;
            _layerResult.Result.StarCount = comment.StarCount;

            int count = _commentDal.Update(_layerResult.Result);

            if (count == 0)
            {
                _layerResult.AddError(ErrorMessageCode.FailedToModifiedRecord, "Yorum güncellenemedi!");
            }
            return _layerResult;
        }

        public Comment GetCommentById(int commentId)
        {
            return _commentDal.Find(i => i.Id == commentId);
        }

        public List<Comment> GetCommentsWithProductsAndMembers()
        {
            return _commentDal.GetCommentsWithProductsAndMembers();

        }

        public Comment GetCommentWithMemberById(int Id)
        {
            return _commentDal.GetCommentWithMemberById(Id);
        }

        public int RemoveComment(Comment comment)
        {
            return _commentDal.Delete(comment);
        }
    }
}
