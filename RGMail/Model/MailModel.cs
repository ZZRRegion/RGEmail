using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RGMail.Model
{
    /// <summary>
    /// 发送邮件实体
    /// </summary>
    public class MailModel
    {
        /// <summary>
        /// 收件人邮箱
        /// </summary>
        public List<string> To { get; set; } = new List<string>();
        /// <summary>
        /// SMTP地址
        /// </summary>
        public string SMTPHost { get; set; }
        /// <summary>
        /// 邮件内容
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 邮件标题
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 邮件优先级
        /// </summary>
        public MailPriority Priority { get; set; }
        /// <summary>
        /// 用户名，也就是发邮箱地址
        /// </summary>
        public string MailAddress { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 发件人姓名
        /// </summary>
        public string Name { get; set; }
    }
}