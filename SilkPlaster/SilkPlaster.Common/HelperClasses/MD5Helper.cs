using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.Common.HelperClasses
{
    public class MD5Helper
    {
        public static string Create(string text)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] btr = Encoding.UTF8.GetBytes(text);
            btr = md5.ComputeHash(btr);

            StringBuilder sb = new StringBuilder();

            foreach (byte ba in btr)
            {
                sb.Append(ba.ToString("x2").ToLower());
            }

            return sb.ToString();
        }
    }
}
