using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RGMail
{
    public static class MailUtil
    {
        public static async Task SendMailUse(Model.MailModel mail)
        {
            MailMessage msg = new MailMessage();
            foreach(string item in mail.To)
            {
                msg.To.Add(item);
            }
            msg.From = new MailAddress(mail.MailAddress, mail.Name, Encoding.UTF8);
            msg.Subject = mail.Subject;//邮件标题    
            msg.SubjectEncoding = Encoding.UTF8;//邮件标题编码    
            msg.Body = mail.Body;//邮件内容    
            msg.BodyEncoding = Encoding.UTF8;//邮件内容编码    
            msg.IsBodyHtml = false;//是否是HTML邮件    
            msg.Priority = mail.Priority;//邮件优先级    

            SmtpClient client = new SmtpClient();
            client.SendCompleted += (ss, ee) => {
                if(ee.Error is SmtpFailedRecipientException smtpex)
                {
                    RGCommon.Main.ViewModel.Error = smtpex.Message;
                }
                else
                {
                    RGCommon.Main.ViewModel.RunEmail = "发送成功！";
                }
            };
            client.Credentials = new System.Net.NetworkCredential(mail.MailAddress, mail.Password);
            //注册的邮箱和密码    
            client.Host = mail.SMTPHost;
            object userState = msg;
            try
            {
                //client.SendAsync(msg, userState);
                await client.SendMailAsync(msg);
            }
            catch (SmtpException ex)
            {
                RGCommon.Main.ViewModel.Error = ex.Message;
            }
            return;
        }
    }
}
