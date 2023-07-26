using System.Windows;
using System.Windows.Media.Animation;

namespace AMS.UI
{
    /// <summary>
    /// Interaction logic for PopupUI.xaml
    /// </summary>
    public partial class PopupUI : Window
    {
        public PopupUI()
        {
            InitializeComponent();
            this.Left = (System.Windows.SystemParameters.PrimaryScreenWidth - this.Width) / 2;
            this.Top = (System.Windows.SystemParameters.PrimaryScreenHeight - this.Height) / 2;

        }
        IBaseUI _main;
        public IBaseUI Main
        {
            get { return _main; }
            set
            {
                _main = value;
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Main != null) Main.HideOverlay();
        }

    }
}
