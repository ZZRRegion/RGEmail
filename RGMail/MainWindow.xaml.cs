using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Threading;
using RGMail.Utils;
using System.IO;

namespace RGMail
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public ViewModels.MainWindowViewModel ViewModel { get; set; }
        private CancellationTokenSource CancellationTokenSourceImportRecive;
        private CancellationTokenSource CancellationTokenSourceImportSend;
        /// <summary>
        /// 取消发送
        /// </summary>
        private bool isCancelSend;
        public MainWindow()
        {
            InitializeComponent();
            FileUtil.ImportReciveEmailEvent += (msg) => {
                this.ViewModel.ReciveEamil = msg;
            };
            FileUtil.ImportSendEmailEvent += (msg) => {
                this.ViewModel.SendEmail = msg;
            };
            RGCommon.Main = this;
            this.ViewModel = ViewModels.MainWindowViewModel.ReadConfig();
            this.DataContext = this.ViewModel;
        }
        private async void btnImport_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "txt|*.txt";
            bool? result = ofd.ShowDialog();
            if(result.HasValue && result.Value)
            {
                this.isCancelSend = true;
                if(this.CancellationTokenSourceImportRecive != null)
                {
                    this.CancellationTokenSourceImportRecive.Cancel();
                }
                this.CancellationTokenSourceImportRecive = new CancellationTokenSource();
                string fileName = ofd.FileName;
                this.ViewModel.To.Clear();
                await FileUtil.ReadEmailLines(fileName, this.ViewModel.To, this.CancellationTokenSourceImportRecive.Token);
                this.isCancelSend = false;
            }
        }

        private async void btnSend_Click(object sender, RoutedEventArgs e)
        {
            string result = this.ViewModel.Verification();
            if (!string.IsNullOrWhiteSpace(result))
            {
                RGCommon.MsgInfo(result);
                return;
            }
            this.ViewModel.IsPause = false;
            int indexSEmail = 0;//当前使用发件人邮箱的下标
            int indexSend = 0;//当前发送的第几条
            int allCount = this.ViewModel.To.Count;//总共条数
            Model.SendMail[] sendLst = new Model.SendMail[this.ViewModel.Send.Count];
            this.ViewModel.Send.CopyTo(sendLst, 0);
            string[] reciveLst = new string[this.ViewModel.To.Count];
            this.ViewModel.To.CopyTo(reciveLst, 0);
            foreach(string item in reciveLst)
            {
                
                if (this.isCancelSend)
                {
                    this.ViewModel.RunEmail = "正在导入收件/发件人邮箱，已停止发送！";
                    break;
                }
                pause:
                if (this.ViewModel.IsPause)
                {
                    this.ViewModel.RunEmail = "已暂停发送";
                    await Task.Delay(100);
                    goto pause;
                }
                indexSend++;
                this.ViewModel.Process = (indexSend * 100.0 / allCount);
                System.Windows.Forms.Application.DoEvents();
                if (indexSEmail >= sendLst.Length)
                    indexSEmail = 0;
                Model.SendMail sendMail = sendLst[indexSEmail++];
                this.ViewModel.RunEmail = $"正在使用{sendMail.Address}发送到:{item}";
                Model.MailModel mailModel = new Model.MailModel()
                {
                    To = new List<string>() { item },
                    Subject = this.ViewModel.Subject,
                    Body = this.ViewModel.Body,
                    SMTPHost = this.ViewModel.SMTPHost,
                    MailAddress = sendMail.Address,
                    Name = this.ViewModel.Name,
                    Password = sendMail.Password,
                    Priority = System.Net.Mail.MailPriority.Normal,
                };
                if (this.ViewModel.SetSubjectTime)
                {
                    mailModel.Subject += DateTime.Now;
                }
                if (this.ViewModel.SetBodyTime)
                {
                    mailModel.Body += DateTime.Now;
                }
                if (this.ViewModel.SetSubjectRandom)
                {
                    int subjectCount = this.ViewModel.SubjectCount;
                    if (subjectCount <= 0)
                        subjectCount = 1;
                    mailModel.Subject = RandomUtil.GenerateRandomLetter(subjectCount) + mailModel.Subject;
                }
                if (this.ViewModel.SetBodyRandom)
                {
                    int bodyCount = this.ViewModel.BodyCount;
                    if (bodyCount <= 0)
                        bodyCount = 1;
                    mailModel.Body = RandomUtil.GenerateRandomLetter(bodyCount) + mailModel.Body;
                }
                if (this.ViewModel.SetBodyYan)
                {
                    try
                    {
                        string yan = await NetUtil.GetYan();
                        mailModel.Body += yan;
                    }
                    catch(Exception ex)
                    {
                        this.ViewModel.RunEmail = ex.Message;
                    }
                }
                await MailUtil.SendMailUse(mailModel);
                await Task.Delay(1000 * this.ViewModel.SendInterval);
            }
            if(!this.isCancelSend)
            RGCommon.MsgInfo("发送完成！");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.ViewModel.Save();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string url = await NetUtil.GetImageURL();
                this.img.Source = new BitmapImage(new Uri(url));
                await Task.Run(async() => {
                    while (true)
                    {
                        if (this.ViewModel.RefreshFuLi)
                        {
                            string str = RandomUtil.GenerateRandomLetter(20);
                            this.ViewModel.FuLiImg = $"https://api.ooopn.com/image/beauty/api.php?{str}";
                        }
                        await Task.Delay(10000);
                    }
                });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async void btnImportSend_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "txt|*.txt";
            bool? result = ofd.ShowDialog();
            if (result.HasValue && result.Value)
            {
                this.isCancelSend = true;
                if(this.CancellationTokenSourceImportSend != null)
                {
                    this.CancellationTokenSourceImportSend.Cancel();
                }
                this.CancellationTokenSourceImportSend = new CancellationTokenSource();
                await Task.Delay(1000);

                string fileName = ofd.FileName;
                this.ViewModel.Send.Clear();
                await FileUtil.ReadSendEmailLines(fileName, this.ViewModel.Send, this.CancellationTokenSourceImportSend.Token);
                this.isCancelSend = false;
            }
        }
    }
}
