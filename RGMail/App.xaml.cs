using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RGMail
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.InitLog();
        }
        private void InitLog()
        {
            string locationPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string m_strPath = locationPath + "\\config\\log4net.config";
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(m_strPath));
        }
    }
}
