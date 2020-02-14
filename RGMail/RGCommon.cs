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
        private static log4net.ILog log = log4net.LogManager.GetLogger(nameof(RGCommon));
        /// <summary>
        /// 版本日期显示，绑定显示
        /// </summary>
        public static string DispalyTitle => "邮件群发[V0.1.12,2020-2-14 18:00:00]";
        public static MainWindow Main { get; set; }
        public static void MsgInfo(string info)
        {
            View.MessageWindow messageWindow = new View.MessageWindow();
            messageWindow.ViewModel.IsAutoClose = true;
            messageWindow.ViewModel.Info = info;
            messageWindow.ShowDialog();
        }
        /// <summary>
        /// 随机数量,绑定有使用到
        /// </summary>
        public static List<int> RandomNum { get; set; } = new List<int>() { 1, 2, 3, 4, 5, 6 };
        /// <summary>
        /// 控制台日志
        /// </summary>
        /// <param name="msg"></param>
        public static void Error(Exception ex)
        {
            log.Error("", ex);
        }
    }
}
