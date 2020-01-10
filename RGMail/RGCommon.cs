using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGMail
{
    public static class RGCommon
    {
        public static string DispalyTitle => "邮件群发[V0.1.3,2020-1-10 23:30:00]";
        public static MainWindow Main { get; set; }
        public static void MsgInfo(string info)
        {
            View.MessageWindow messageWindow = new View.MessageWindow();
            messageWindow.ViewModel.IsAutoClose = true;
            messageWindow.ViewModel.Info = info;
            messageWindow.ShowDialog();
        }
    }
}
