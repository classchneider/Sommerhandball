using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using static StringLib.StringFunctions;

namespace StdLib
{
    public class FileHandlingStd
    {
        public static string PathFolderFile(string path, string folder, string file)
        {
            return path + "/" + folder + "/" + file;
        }

        public static string PathFolder(string path, string folder)
        {
            return path + "/" + folder;
        }

        public static string AddFileExtension(string text, string extension)
        {
            return AddExtension(text, '.', extension);
        }

        public static IEnumerable<string> GetFileNames(string path)
        {
            IEnumerable<string> files = null;
            List<string> resultfiles = new List<string>();
            try
            {
                files = Directory.EnumerateFiles(path).Where(x => x.Substring(x.Length - 5, 5) != ".temp");
            }
            catch
            {
                // Do nothing;
                return resultfiles;
            }

            foreach (string file in files)
            {
                resultfiles.Add(FileName(file));
            }
            return resultfiles;
        }

        public static string FileName(string path)
        {
            return LastPart(path, '/');
        }

        public static string Extension(string nameOrPath)
        {
            return LastPart(nameOrPath, '.');
        }

        public static object LoadFromFilePath(string path, Type type)
        {
            object obj = null;
            Stream stream = null;
            if (File.Exists(path))
            {
                try
                {
                    stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
                    XmlSerializer serializer = new XmlSerializer(type);
                    obj = serializer.Deserialize(stream);
                }
                catch (Exception ex)
                {
                    //try
                    //{
                    //    // Xml deserialize not working, try binary
                    //    BinaryFormatter formatter = new BinaryFormatter();
                    //    if (stream.Length > 0)
                    //    {
                    //        stream.Position = 0;
                    //        obj = formatter.Deserialize(stream);
                    //    }
                    //}
                    //catch (Exception)
                    //{
                    //    throw new Exception("Error path: " + path, ex);
                    //}
                    throw new Exception("Error path: " + path, ex);
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }
            }

            return obj;
        }
    }
}
