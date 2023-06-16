using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rukn
{
    public static class Helper
    {
        public static string ConvertNumbers(int index, char seed = '\ufd50')
        {
            // The Uthmanic Hafs font has special visual for standard eastern arabic numbers,
            // while the regular visual is put in different Unicode range so the renderer will not
            // realize they are digits hence it won't render them from left to right, so I fix it manually
            bool needReverse = !char.IsDigit(seed);
            string str = index.ToString();
            char[] result = new char[str.Length];
            for (byte i = 0; i < str.Length; i++)
            {
                char c = str[i];
                int position = needReverse ? str.Length - i - 1 : i;
                result[position] = (char)(seed + char.GetNumericValue(c));
            }
            return new string(result);
        }
    }
}
