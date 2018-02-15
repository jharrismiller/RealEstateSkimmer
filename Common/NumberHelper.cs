using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public static class NumberHelper
    {
        public static decimal? GetDecimal(string value)
        {
            decimal val;
            if (decimal.TryParse(value, out val))
            {
                return val;
            }
            return null;
        }
        public static int? GetNumber(string value)
        {
            int val;
            if (int.TryParse(value, out val))
            {
                return val;
            }
            else if (value != null)
            {
                var validNum = new StringBuilder();
                foreach (var c in value)
                {
                    if (char.IsNumber(c))
                        validNum.Append(c);
                    else if (c == '.')
                        break;
                }
                if (int.TryParse(validNum.ToString(), out val))
                {
                    return val;
                }
            }
            return null;
        }

    }
}
