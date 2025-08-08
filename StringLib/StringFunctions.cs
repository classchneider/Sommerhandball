using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringLib
{
    public class StringFunctions
    {
        public static string LastPart(string text, char delimiter)
        {
            int pathindex = text.LastIndexOf(delimiter);
            if (pathindex >= 0)
            {
                return text.Substring(pathindex + 1);
            }
            else
            {
                return text;
            }
        }

        public static string MinusLastPart(string text, char delimiter)
        {
            int pathindex = text.LastIndexOf(delimiter);
            if (pathindex >= 0)
            {
                return text.Substring(0, pathindex);
            }
            else
            {
                return text;
            }
        }

        public static string AddExtension(string text, char delimiter, string extension)
        {
            if (extension != "")
            {
                return text + delimiter + extension;
            }
            else
            {
                return text;
            }
        }
    }
}

