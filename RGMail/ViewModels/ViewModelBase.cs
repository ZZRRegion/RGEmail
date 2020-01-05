using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RGMail.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        protected void SetProperty<T>(ref T property, T value, [CallerMemberName]string propertyName = "")
        {
            property = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
