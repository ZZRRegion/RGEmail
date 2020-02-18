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
using System.Windows.Media;
using Newtonsoft.Json;

namespace RGMail.ViewModels
{
    public class MainWindowViewModel:ViewModelBase
    {
        private string message;
        /// <summary>
        /// 用户留言
        /// </summary>
        [JsonIgnore]
        public string Message
        {
            get => this.message;
            set => this.SetProperty(ref this.message, value);
        }
        private TimeSpan useTime = TimeSpan.FromSeconds(1);
        /// <summary>
        /// 使用时长
        /// </summary>
        public TimeSpan UseTime
        {
            get => this.useTime;
            set => this.SetProperty(ref this.useTime, value);
        }
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
        private ObservableCollection<QQModel> tos = new ObservableCollection<QQModel>();
        public ObservableCollection<QQModel> Tos
        {
            get => this.tos;
            set => this.SetProperty(ref this.tos, value);
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
            string result = Newtonsoft.Json.JsonConvert.SerializeObject(this, Formatting.Indented);
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
            if(this.Tos.Count == 0)
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
            if (this.Tos.Count == 0)
            {
                return "收件人列表不能为空,请添加！";
            }
            if (string.IsNullOrWhiteSpace(this.Name))
            {
                return "发件人名称不能为空，请修改！";
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
        private bool setSubjectTime;
        public bool SetSubjectTime
        {
            get => this.setSubjectTime;
            set => this.SetProperty(ref this.setSubjectTime, value);
        }
        private bool setSubjectRandom;
        public bool SetSubjectRandom
        {
            get => this.setSubjectRandom;
            set => this.SetProperty(ref this.setSubjectRandom, value);
        }
        private int subjectCount = 1;
        /// <summary>
        /// 主题前面随机字母数量
        /// </summary>
        public int SubjectCount
        {
            get => this.subjectCount;
            set => this.SetProperty(ref this.subjectCount, value);
        }
        private bool setBodyTime;
        public bool SetBodyTime
        {
            get => this.setBodyTime;
            set => this.SetProperty(ref this.setBodyTime, value);
        }
        private bool setBodyRandom;
        public bool SetBodyRandom
        {
            get => this.setBodyRandom;
            set => this.SetProperty(ref this.setBodyRandom, value);
        }
        private int bodyCount = 2;
        /// <summary>
        /// 正文内容前随机字母数量
        /// </summary>
        public int BodyCount
        {
            get => this.bodyCount;
            set => this.SetProperty(ref this.bodyCount, value);
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
        private int sendInterval = 1;
        /// <summary>
        /// 发送间隔
        /// </summary>
        public int SendInterval
        {
            get => this.sendInterval;
            set => this.SetProperty(ref this.sendInterval, value);
        }
        private bool setBodyYan;
        /// <summary>
        /// 添加每日一言
        /// </summary>
        public bool SetBodyYan
        {
            get => this.setBodyYan;
            set => this.SetProperty(ref this.setBodyYan, value);
        }
        private bool refreshFuLi;
        public bool RefreshFuLi
        {
            get => this.refreshFuLi;
            set => this.SetProperty(ref this.refreshFuLi, value);
        }
        private string sceneryImg = "https://api.ooopn.com/image/infinity/api.php"; 
        /// <summary>
        /// 美景美图
        /// </summary>
        public string SceneryImg
        {
            get => this.sceneryImg;
            set => this.SetProperty(ref this.sceneryImg, value);
        }
        private string sogouImg = "https://api.ooopn.com/image/sogou/api.php";
        /// <summary>
        /// 搜狗美图
        /// </summary>
        public string SogouImg
        {
            get => this.sogouImg;
            set => this.SetProperty(ref this.sogouImg, value);
        }
        private string fuLiImg = "https://api.ooopn.com/image/beauty/api.php";
        /// <summary>
        /// 福利图
        /// </summary>
        public string FuLiImg
        {
            get => this.fuLiImg;
            set => this.SetProperty(ref this.fuLiImg, value);
        }
        private string backImg = "https://api.ooopn.com/image/beauty/api.php?" + DateTime.Now.Ticks;
        /// <summary>
        /// 背景图
        /// </summary>
        public string BackImg
        {
            get => this.backImg;
            set => this.SetProperty(ref this.backImg, value);
        }
        private int tabIndex = 0;
        public int TabIndex
        {
            get => this.tabIndex;
            set => this.SetProperty(ref this.tabIndex, value);
        }
        private bool addQQName;
        /// <summary>
        /// 添加QQ名称
        /// </summary>
        public bool AddQQName
        {
            get => this.addQQName;
            set => this.SetProperty(ref this.addQQName, value);
        }
        private ObservableCollection<string> sendNames = new ObservableCollection<string>();
        /// <summary>
        /// 发件人名称
        /// </summary>
        public ObservableCollection<string> SendNames
        {
            get => this.sendNames;
            set => this.SetProperty(ref this.sendNames, value);
        }
        private bool useSendNames = true;
        /// <summary>
        /// 是否循环使用发件人名称列表
        /// </summary>
        public bool UseSendNames
        {
            get => this.useSendNames;
            set => this.SetProperty(ref this.useSendNames, value);
        }
    }
}
