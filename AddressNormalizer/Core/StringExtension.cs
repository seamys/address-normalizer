using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressNormalizer.Core
{
    public static class StringExtension
    {
        public static string TrimEnd(this string value, string trim)
        {
            int i = value.LastIndexOf(trim);
            if (i + trim.Length != value.Length)
                return value;
            return value.Substring(0, i);
        }
    }
}
