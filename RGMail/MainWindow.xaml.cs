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

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "txt|*.txt";
            bool? result = ofd.ShowDialog();
            if(result.HasValue && result.Value)
            {
                string fileName = ofd.FileName;
                this.ViewModel.To = FileUtil.ReadEmailLines(fileName);
            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            Regex reg = new Regex(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
            bool b = reg.IsMatch(this.ViewModel.MailAddress);
            if (!b)
            {
                RGCommon.MsgInfo("发件人邮箱不合法！");
                return;
            }
            if (string.IsNullOrWhiteSpace(this.ViewModel.Password))
            {
                RGCommon.MsgInfo("发件人密码不能为空！");
                return;
            }
            if (string.IsNullOrWhiteSpace(this.ViewModel.SMTPHost))
            {
                RGCommon.MsgInfo("SMTP地址不能为空！");
                return;
            }
            if (string.IsNullOrWhiteSpace(this.ViewModel.Subject))
            {
                RGCommon.MsgInfo("主题不能为空！");
                return;
            }
            if (string.IsNullOrWhiteSpace(this.ViewModel.Body))
            {
                RGCommon.MsgInfo("正文内容不能为空！");
                return;
            }
            if(this.ViewModel.To.Count == 0)
            {
                RGCommon.MsgInfo("收件人列表不能为空,请添加！");
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
            View.MessageWindow messageWindow = new View.MessageWindow();
            messageWindow.ViewModel.Info = msg;
            messageWindow.ShowDialog();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.ViewModel.Save();
        }
    }
}
