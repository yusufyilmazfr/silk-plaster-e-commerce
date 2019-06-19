using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SilkPlaster.UI.Models.Helpers
{
    public static class ImageHelper
    {

        public static bool IsTypeImage(string mimeType)
        {
            string[] mimeTypes = { "image/jpeg", "image/png", "image/gif" };

            return mimeTypes.Contains(mimeType);
        }

        public static string CreateUniqueString()
        {
            return String.Format("{0:yyyyMMddHHmmss}", DateTime.Now);
        }
    }
}