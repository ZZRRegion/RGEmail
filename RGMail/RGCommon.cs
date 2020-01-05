using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGMail
{
    public static class RGCommon
    {
        public static string Version => "0.1.0";
        public static string VersionTime => "2020-1-5 10:40:00";
        public static MainWindow Main { get; set; }
        public static void MsgInfo(string info)
        {
            View.MessageWindow messageWindow = new View.MessageWindow();
            messageWindow.ViewModel.Info = info;
            messageWindow.ShowDialog();
        }
    }
}
