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

namespace RGMail
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public ViewModels.MainWindowViewModel ViewModel { get; set; }
        private Thread thread;
        public MainWindow()
        {
            InitializeComponent();
            RGCommon.Main = this;
            this.ViewModel = ViewModels.MainWindowViewModel.ReadConfig();
            this.DataContext = this.ViewModel;
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "txt|*.txt";
            bool? result = ofd.ShowDialog();
            if(result.HasValue && result.Value)
            {
                string fileName = ofd.FileName;
                this.ViewModel.To = FileUtil.ReadEmailLines(fileName);
                if (this.ViewModel.IsAuto)
                {
                    this.btnSend_Click(this.btnSend, new RoutedEventArgs());
                }
            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            string result = this.ViewModel.Val();
            if (!string.IsNullOrWhiteSpace(result))
            {
                RGCommon.MsgInfo(result);
                return;
            }
            Model.MailModel mailModel = new Model.MailModel()
            {
                To = this.ViewModel.To,
                Subject = this.ViewModel.Subject,
                Body = this.ViewModel.Body.Replace("${DateTime}", DateTime.Now.ToString()),
                SMTPHost = this.ViewModel.SMTPHost,
                MailAddress = this.ViewModel.MailAddress,
                Name = this.ViewModel.Name,
                Password = this.ViewModel.Password,
                Priority = this.ViewModel.Priority,
            };
            (bool state, string msg) = MailUtil.SendMailUse(mailModel);
            RGCommon.MsgInfo(msg);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.ViewModel.Save();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.thread = new Thread(this.Run);
            this.thread.IsBackground = true;
            this.thread.Start();
        }
        private void Run()
        {
            while (true)
            {
                if (this.ViewModel.IsIntervalSend)
                {
                    string result = this.ViewModel.Val();
                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        this.ViewModel.Error = result;
                        Thread.Sleep(this.ViewModel.IntervalTime * 1000);
                        continue;
                    }
                    Model.MailModel mailModel = new Model.MailModel()
                    {
                        To = this.ViewModel.To,
                        Subject = this.ViewModel.Subject,
                        Body = this.ViewModel.Body + Environment.NewLine + "发送时间：" + DateTime.Now,
                        SMTPHost = this.ViewModel.SMTPHost,
                        MailAddress = this.ViewModel.MailAddress,
                        Name = this.ViewModel.Name,
                        Password = this.ViewModel.Password,
                        Priority = this.ViewModel.Priority,
                    };
                    try
                    {
                        (bool state, string msg) = MailUtil.SendMailUse(mailModel);
                        this.ViewModel.Error = msg;
                    }
                    catch(Exception ex)
                    {
                        this.ViewModel.Error = ex.Message;
                    }
                    Thread.Sleep(this.ViewModel.IntervalTime * 1000);
                }
                Thread.Sleep(this.ViewModel.IntervalTime * 1000);
            }
        }
    }
}
