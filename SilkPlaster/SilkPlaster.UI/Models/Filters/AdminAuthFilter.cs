using SilkPlaster.UI.Models.Helpers;
using SilkPlaster.UI.Models.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SilkPlaster.UI.Models.Filters
{
    public class AdminAuthFilter : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (CurrentSession.Get<AdminSessionModel>("Admin") == null)
            {
                string returnUrl = filterContext.HttpContext.Request.Url.AbsoluteUri;

                filterContext.Result = new RedirectToRouteResult
                    (
                    new System.Web.Routing.RouteValueDictionary(new { controller = "Home", action = "Index", returnUrl = returnUrl })
                    );
            }
        }
    }
}