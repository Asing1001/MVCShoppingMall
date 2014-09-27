using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WecareMVC.Models
{
    public static class ExtensionMethod
    {
        public static string Truncate(this string s, int length)
        {
            if (s.Length<=length)
            {
                return s;
            }
            return s.Substring(0,length)+"...";
        }     
    }
}