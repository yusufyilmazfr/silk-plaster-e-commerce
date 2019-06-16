using SilkPlaster.Common.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.BusinessLayer.Result
{
    public class BusinessLayerResult<T>
    {
        public BusinessLayerResult()
        {
            Errors = new List<ErrorMessageObject>();
        }

        public List<ErrorMessageObject> Errors { get; set; }
        public T Result { get; set; }

        public void AddError(ErrorMessageCode messageCode, string message)
        {
            Errors.Add(new ErrorMessageObject { ErrorCode = messageCode, ErrorMessage = message });
        }
    }
}
