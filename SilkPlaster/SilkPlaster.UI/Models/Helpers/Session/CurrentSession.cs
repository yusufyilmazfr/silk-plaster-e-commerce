using SilkPlaster.UI.Models.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SilkPlaster.UI.Models.Helpers.Session
{
    public class CurrentSession
    {

        public static MemberSessionModel Member
        {
            get
            {
                return Get<MemberSessionModel>("Member");
            }
        }

        public static bool MemberIsLogged
        {
            get
            {
                return Member != null ? true : false;
            }
        }

        public static void Set<T>(string name, T obj)
        {
            HttpContext.Current.Session[name] = obj;
        }

        public static T Get<T>(string name)
        {
            return HttpContext.Current.Session[name] != null ? (T)HttpContext.Current.Session[name] : default(T);
        }

        public static void Remove(string name)
        {
            if (HttpContext.Current.Session[name] != null)
            {
                HttpContext.Current.Session.Remove(name);
            }
        }

        public static void ClearAllSession()
        {
            HttpContext.Current.Session.Clear();
        }
    }
}