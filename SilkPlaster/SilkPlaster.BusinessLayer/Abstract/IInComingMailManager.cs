using SilkPlaster.BusinessLayer.Concrete.Result;
using SilkPlaster.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.BusinessLayer.Abstract
{
    public interface IInComingMailManager
    {
        int Insert(InComingMail ınComingMail);
    }
}
