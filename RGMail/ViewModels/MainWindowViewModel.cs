using RGMail.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.XPath;
using System.Xml.Linq;
using System.Reflection;

namespace RGMail.ViewModels
{
    public class MainWindowViewModel:ViewModelBase
    {
        /// <summary>
        /// 收件人邮箱
        /// </summary>
        private List<string> to = new List<string>();
        public List<string> To
        {
            get => this.to;
            set => this.SetProperty(ref this.to, value);
        }
        private string smtpHost = "smtp.jngjz.xyz";
        /// <summary>
        /// SMTP地址
        /// </summary>
        public string SMTPHost
        {
            get => this.smtpHost;
            set => this.SetProperty(ref this.smtpHost, value);
        }
        private string body = "正文内容" + DateTime.Now;
        /// <summary>
        /// 邮件内容
        /// </summary>
        public string Body
        {
            get => this.body;
            set => this.SetProperty(ref this.body, value);
        }
        private string subject = "节日快乐";
        /// <summary>
        /// 邮件主题
        /// </summary>
        public string Subject
        {
            get => this.subject;
            set => this.SetProperty(ref this.subject, value);
        }
        private MailPriority mailPriority = MailPriority.High;
        /// <summary>
        /// 邮件优先级
        /// </summary>
        public MailPriority Priority
        {
            get => this.mailPriority;
            set => this.SetProperty(ref this.mailPriority, value);
        }
        private string mailAddress = "55@jngjz.xyz";
        /// <summary>
        /// 用户名，也就是发邮箱地址
        /// </summary>
        public string MailAddress
        {
            get => this.mailAddress;
            set => this.SetProperty(ref this.mailAddress, value);
        }
        private string password = "123456789";
        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            get => this.password;
            set => this.SetProperty(ref this.password, value);
        }
        private string name = Environment.MachineName;
        /// <summary>
        /// 发件人姓名
        /// </summary>
        public string Name
        {
            get => this.name;
            set => this.SetProperty(ref this.name, value);
        }
        private string error;
        public string Error
        {
            get => this.error;
            set => this.SetProperty(ref this.error, value);
        }
        const string config = "config.json";
        public void Save()
        {
            string result = Newtonsoft.Json.JsonConvert.SerializeObject(this);
            File.WriteAllText(config, result);
        }
        public static MainWindowViewModel ReadConfig()
        {
            if (File.Exists(config))
            {
                string result = File.ReadAllText(config);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<MainWindowViewModel>(result);
            }
            return new MainWindowViewModel();
        }
    }
}
