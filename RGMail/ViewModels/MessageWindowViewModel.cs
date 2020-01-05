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
    }
}
