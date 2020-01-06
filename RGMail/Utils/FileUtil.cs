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
        public static async Task<List<string>> ReadEmailLines(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException($"文件不存在：{fileName}");
            }
            Regex reg = new Regex(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
            StreamReader sr = new StreamReader(fileName);
            string line = null;
            List<string> lst = new List<string>();
            long count = 0;
            while ((line = await sr.ReadLineAsync()) != null)
            {
                //await Task.Delay(1);
                System.Windows.Forms.Application.DoEvents();
                Console.WriteLine(count++);
                if (reg.IsMatch(line))
                {
                    if (!lst.Contains(line))
                        lst.Add(line);
                }
            }
            sr.Close();
            return lst;
        }
    }
}
