using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SilkPlaster.UI.Infrastructure.Ninject
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        public IKernel _kernel { get; set; }

        public NinjectControllerFactory(NinjectModule ninjectModule)
        {
            _kernel = new StandardKernel(ninjectModule);
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)_kernel.Get(controllerType);
        }
    }
}