using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Threading;
using RGMail.Model;
using RGMail.Utils;

namespace RGMail
{
    public static class FileUtil
    {
        /// <summary>
        /// 导入收件人
        /// </summary>
        public static event Action<string> ImportReciveEmailEvent;
        /// <summary>
        /// 导入发件人
        /// </summary>
        public static event Action<string> ImportSendEmailEvent;
        /// <summary>
        /// 导入收件人
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="dst"></param>
        /// <returns></returns>
        public static async Task ReadEmailLines(string fileName, IList<QQModel> dst, CancellationToken cancellationToken)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException($"文件不存在：{fileName}");
            }
            Regex reg = new Regex(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
            StreamReader sr = new StreamReader(fileName);
            string line = null;
            long count = 1;
            while ((line = await sr.ReadLineAsync()) != null)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;
                System.Windows.Forms.Application.DoEvents();
                string msg = string.Empty;
                if (reg.IsMatch(line))
                {
                    if(dst.FirstOrDefault(item => item.QQ == line) == null)
                    {
                        QQModel model = new QQModel()
                        {
                            QQ = line,
                        };
                        if (line.EndsWith("@qq.com"))
                        {
                            try
                            {
                                string qq = line.Replace("@qq.com", "");
                                QQModel tempModel = await NetUtil.GetQQImage(qq);
                                model.Name = tempModel.Name;
                                model.ImgUrl = tempModel.ImgUrl;
                            }
                            finally { }
                        }
                        dst.Add(model);
                        msg = $"成功导入条数：{count++}";
                    }
                }
                else
                {
                    msg = $"邮箱不正确：{line}";
                }
                ImportReciveEmailEvent?.BeginInvoke(msg, null, null);
            }
            sr.Close();
        }
        /// <summary>
        /// 导入发件人
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="dst"></param>
        /// <returns></returns>
        public static async Task ReadSendEmailLines(string fileName, IList<Model.SendMail> dst, CancellationToken cancellationToken)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException($"文件不存在：{fileName}");
            }
            Regex reg = new Regex(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
            StreamReader sr = new StreamReader(fileName);
            long count = 1;
            string line;
            while ((line = await sr.ReadLineAsync()) != null)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;
                System.Windows.Forms.Application.DoEvents();
                string[] items = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                string msg = string.Empty;
                if (items != null && items.Length == 2)
                {
                    if (reg.IsMatch(items[0]))
                    {
                        Model.SendMail sendMail = new Model.SendMail()
                        {
                            Address = items[0],
                            Password = items[1],
                        };
                        dst.Add(sendMail);
                        msg = $"成功导入条数：{count++}";
                    }
                    else
                    {
                        msg = $"邮箱号不正确:{items[0]}";
                    }
                }
                else
                {
                    msg = $"邮箱密码格式不正确,正确示例：zhang3@jngjz.xyz  123456789，邮箱号与密码之间空格隔开";
                }
                ImportSendEmailEvent?.BeginInvoke(msg, null, null);
            }
            sr.Close();
        }
    }
}
