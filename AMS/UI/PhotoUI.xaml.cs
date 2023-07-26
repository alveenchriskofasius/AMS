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

namespace AMS.UI
{
    /// <summary>
    /// Interaction logic for PhotoUI.xaml
    /// </summary>
    public partial class PhotoUI : Window
    {
        public PhotoUI()
        {
            InitializeComponent();
        }
        EmployeeSelectorUI _main;
        ImageSource _source;

        public EmployeeSelectorUI Main
        {
            set { _main = value; }
        }
        public ImageSource Source
        {
            get { return _source; }
            set { _source = value; }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ImageView.Source = Source;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _main.Main.HideOverlay();
        }
    }
}
