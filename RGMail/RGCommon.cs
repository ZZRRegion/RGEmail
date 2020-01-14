using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGMail
{
    /// <summary>
    /// 通用
    /// </summary>
    public static class RGCommon
    {
        public static string DispalyTitle => "邮件群发[V0.1.5,2020-1-14 22:30:00]";
        public static MainWindow Main { get; set; }
        public static void MsgInfo(string info)
        {
            View.MessageWindow messageWindow = new View.MessageWindow();
            messageWindow.ViewModel.IsAutoClose = true;
            messageWindow.ViewModel.Info = info;
            messageWindow.ShowDialog();
        }
        /// <summary>
        /// 随机数量
        /// </summary>
        public static List<int> RandomNum { get; set; } = new List<int>() { 1, 2, 3, 4, 5, 6 };
    }
}
