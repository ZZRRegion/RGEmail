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
using System.Windows.Shapes;

namespace RGMail.View
{
    /// <summary>
    /// MessageWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MessageWindow : Window
    {
        public ViewModels.MessageWindowViewModel ViewModel { get; set; } = new ViewModels.MessageWindowViewModel();
        public MessageWindow()
        {
            InitializeComponent();
            this.DataContext = this.ViewModel;
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch { }
            base.OnMouseLeftButtonDown(e);
        }
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.ViewModel.IsAutoClose)
            {
                System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer()
                {
                    Interval = TimeSpan.FromMilliseconds(this.ViewModel.AutoCloseTime),
                };
                timer.Tick += (ss, ee) => {
                    timer.Stop();
                    this.Close();
                };
                timer.Start();
            }
        }
    }
}
