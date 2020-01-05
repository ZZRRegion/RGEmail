using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGMail.ViewModels
{
    public class MessageWindowViewModel:ViewModelBase
    {
        private string info;
        public string Info
        {
            get => this.info;
            set => this.SetProperty(ref this.info, value);
        }
        private bool isAutoClose;
        public bool IsAutoClose
        {
            get => this.isAutoClose;
            set => this.SetProperty(ref this.isAutoClose, value);
        }
        private int autoCloseTime = 3000;
        public int AutoCloseTime
        {
            get => this.autoCloseTime;
            set => this.SetProperty(ref this.autoCloseTime, value);
        }
    }
}
