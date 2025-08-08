using System;

namespace StandardFunctionsWindows
{
    public static class FileHandlingWindows
    {
        public static void DatabasePath()
        {
            string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = (System.IO.Path.GetDirectoryName(executable));

            path = (System.IO.Path.GetDirectoryName(path));
            path = (System.IO.Path.GetDirectoryName(path));
            path = (System.IO.Path.GetDirectoryName(path));

            //path = MinusLastPart(path, '\\', 3);
            path = path + "\\DB";
            AppDomain.CurrentDomain.SetData("DataDirectory", path);
        }
    }

    public static class StringFunctions
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
