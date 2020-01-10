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
using System.Text.RegularExpressions;

namespace RGMail.ViewModels
{
    public class MainWindowViewModel:ViewModelBase
    {
        private ObservableCollection<Model.SendMail> send = new ObservableCollection<SendMail>();
        /// <summary>
        /// 发件人邮箱
        /// </summary>
        public ObservableCollection<Model.SendMail> Send
        {
            get => this.send;
            set => this.SetProperty(ref this.send, value);
        }
        /// <summary>
        /// 收件人邮箱
        /// </summary>
        private ObservableCollection<string> to = new ObservableCollection<string>();
        public ObservableCollection<string> To
        {
            get => this.to;
            set => this.SetProperty(ref this.to, value);
        }
        private string smtpHost = "smtp.jngjz.xyz";
        /// <summary>
        /// SMTP地址
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
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
       
        private string name = Environment.MachineName;
        /// <summary>
        /// 发件人姓名
        /// </summary>
        public string Name
        {
            get => this.name;
            set => this.SetProperty(ref this.name, value);
        }
        
        const string config = "config.json";
        public void Save()
        {
            string result = Newtonsoft.Json.JsonConvert.SerializeObject(this);
            File.WriteAllText(config, result);
        }
        /// <summary>
        /// 验证数据
        /// </summary>
        /// <returns></returns>
        public string Verification()
        {
            if(this.sendInterval < 1)
            {
                return "发送间隔时间不能小于1";
            }
            if(this.send.Count == 0)
            {
                return "发件人不能为空！";
            }
            if(this.to.Count == 0)
            {
                return "收件人不能为空！";
            }
            Regex reg = new Regex(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
            if (string.IsNullOrWhiteSpace(this.SMTPHost))
            {
                return "SMTP地址不能为空！";
            }
            if (string.IsNullOrWhiteSpace(this.Subject))
            {
                return "主题不能为空！";
            }
            if (string.IsNullOrWhiteSpace(this.Body))
            {
                return "正文内容不能为空！";
            }
            if (this.To.Count == 0)
            {
                return "收件人列表不能为空,请添加！";
            }
            return null;
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
        private bool subjectAddTime;
        /// <summary>
        /// 主题后加日期
        /// </summary>
        public bool SubjectAddTime
        {
            get => this.subjectAddTime;
            set => this.SetProperty(ref this.subjectAddTime, value);
        }
        private bool bodyAddTime;
        /// <summary>
        /// 正文内容加日期
        /// </summary>
        public bool BodyAddTime
        {
            get => this.bodyAddTime;
            set => this.SetProperty(ref this.bodyAddTime, value);
        }
        /// <summary>
        /// 导入收件人邮箱提示
        /// </summary>
        private string reciveEmail;
        [Newtonsoft.Json.JsonIgnore]
        public string ReciveEamil
        {
            get => this.reciveEmail;
            set => this.SetProperty(ref this.reciveEmail, value);
        }
        private string sendEmail;
        [Newtonsoft.Json.JsonIgnore]
        /// <summary>
        /// 导入发件人邮箱提示
        /// </summary>
        public string SendEmail
        {
            get => this.sendEmail;
            set => this.SetProperty(ref this.sendEmail, value);
        }
        private string runEmail;
        [Newtonsoft.Json.JsonIgnore]
        /// <summary>
        /// 发送成功提示
        /// </summary>
        public string RunEmail
        {
            get => this.runEmail;
            set => this.SetProperty(ref this.runEmail, value);
        }
        private string error;
        [Newtonsoft.Json.JsonIgnore]
        public string Error
        {
            get => this.error;
            set => this.SetProperty(ref this.error, value);
        }
        private bool isPause;
        /// <summary>
        /// 是否暂停发送
        /// </summary>
        public bool IsPause
        {
            get => this.isPause;
            set => this.SetProperty(ref this.isPause, value);
        }
        private double process;
        [Newtonsoft.Json.JsonIgnore]
        public double Process
        {
            get => this.process;
            set => this.SetProperty(ref this.process, value);
        }
        private int sendInterval;
        /// <summary>
        /// 发送间隔
        /// </summary>
        public int SendInterval
        {
            get => this.sendInterval;
            set => this.SetProperty(ref this.sendInterval, value);
        }
    }
}
