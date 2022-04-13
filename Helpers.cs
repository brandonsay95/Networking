using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module1Ep1
{
    public class Helpers
    {
        public static byte[] ConvertStringToBytes(string s) => System.Text.Encoding.ASCII.GetBytes(s);

        public static string ConvertBytesToString(byte[] b) => System.Text.Encoding.ASCII.GetString(b);
        public static string ConvertBytesToDisplayString(byte[] bytes , int pos , int offsett)
        {
            string returnString = "\n|HEX:";
            returnString += bytes.Skip(pos).Take(offsett).Select(o => o.ToString("X2")).Aggregate((a, b) => a + ':' + b) + "|";
            returnString += "\n|Decimal:";
            returnString += bytes.Skip(pos).Take(offsett).Select(o => o.ToString("000")).Aggregate((a, b) => a + ':' + b) + "|";
            returnString += "\n|ASCII:";
            returnString += Encoding.ASCII.GetString(bytes,pos,offsett) + "|";
            return returnString;
            //string hstring = "";
            //for (int i = 0; i < bytes.Length; i++)
            //{
            //    hstring += bytes[i].ToString("X2");
            //    if (i < bytes.Length - 1)
            //        hstring += ":";
            //}


        }
    }
}
