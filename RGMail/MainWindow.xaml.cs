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
        public MainWindow()
        {
            InitializeComponent();
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
                string fileName = ofd.FileName;
                this.ViewModel.Error = "导入中...";
                this.ViewModel.To = await FileUtil.ReadEmailLines(fileName);
                this.ViewModel.Error = "导入成功！";
                if (this.ViewModel.IsAuto)
                {
                    this.btnSend_Click(this.btnSend, new RoutedEventArgs());
                }
            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            string result = this.ViewModel.Verification();
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
            if (this.ViewModel.SubjectAddTime)
            {
                mailModel.Subject += DateTime.Now;
            }
            if (this.ViewModel.BodyAddTime)
            {
                mailModel.Body += DateTime.Now;
            }
            (bool state, string msg) = MailUtil.SendMailUse(mailModel);
            RGCommon.MsgInfo(msg);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.ViewModel.Save();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
