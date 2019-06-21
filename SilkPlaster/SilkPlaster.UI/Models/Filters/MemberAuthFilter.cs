using SilkPlaster.UI.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SilkPlaster.UI.Models.Filters
{
    public class MemberAuthFilter : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!CurrentSession.MemberIsLogged)
            {
                string returnUrl = filterContext.HttpContext.Request.Url.AbsoluteUri;

                filterContext.Result = new RedirectToRouteResult
                    (
                        new System.Web.Routing.RouteValueDictionary(new { controller = "Account", action = "Login", returnUrl = returnUrl })
                    );
            }
        }
    }
}