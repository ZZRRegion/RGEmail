using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace RGMail
{
    public static class FileUtil
    {
        public static List<string> ReadEmailLines(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException($"文件不存在：{fileName}");
            }
            Regex reg = new Regex(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
            string[] lines = File.ReadAllLines(fileName);
            List<string> lst = new List<string>();
            foreach (string item in lines)
            {
                if (reg.IsMatch(item))
                {
                    lst.Add(item);
                }
            }
            return lst;
        }
    }
}
