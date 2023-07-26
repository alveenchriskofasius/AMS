using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using AMS.Proxy;
using AMS.VO;

namespace AMS.UI
{
    /// <summary>   
    /// Interaction logic for MainUI.xaml
    /// </summary>

    public partial class MainUI : Window, IBaseUI
    {

        //private UserVO _user = new UserVO();
        public bool IsDirty { get; set; }

        private string _error = "";
        private string _connectedTo = "";


        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register("Type", typeof(Message), typeof(MainUI), new PropertyMetadata(Message.Normal));

        public Message Type
        {
            get { return (Message)this.GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public MainUI(BaseUI userControl = null)
        {
            InitializeComponent();
            this.Left = (System.Windows.SystemParameters.PrimaryScreenWidth - this.Width) / 2;
            this.Top = (System.Windows.SystemParameters.PrimaryScreenHeight - this.Height) / 2;
            if (userControl != null)
            {
                userControl.Main = this;
                Title = userControl.Title + _connectedTo;
                PanelMain.Children.Add(userControl);
            }

        }

        private void Main_Loaded(object sender, RoutedEventArgs e)
        {
            // display username & icon
            SetProfile();
            SetMenuVisibility();
        }
        private void SetMenuVisibility()
        {
            User.Visibility = Master.Visibility = UserProxy.CurrentUser.Role("Admin") || UserProxy.CurrentUser.Role("System") ? Visibility.Visible : Visibility.Collapsed;
            Employee.Visibility = Absent.Visibility = UserProxy.CurrentUser.Role("Admin") || UserProxy.CurrentUser.Role("Karyawan") ? Visibility.Visible : Visibility.Collapsed;

        }

        private void Main_Unloaded(object sender, RoutedEventArgs e)
        {
        }


        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;

            if (menuItem.Name == "Logout")
            {
                LoginUI loginUI = new LoginUI();
                loginUI.Show();
                this.Close();
            }
            else
            {
                string userControlName = menuItem.Name;

                BaseUI userControl = UserControl(userControlName);
                // assign parent
                userControl.Main = this;
                Title = userControl.Title;
                PanelMain.Children.Clear();
                PanelMain.Children.Add(userControl);
            }
        }

        public void ShowMessage(string message, Message type = Message.Normal)
        {
            ShowMessage(message, "", type);
        }

        public void ShowMessage(string message, string error)
        {
            ShowMessage(message, error, Message.Error);
        }

        public void ShowMessage(string message, string error, Message type)
        {
            LabelMessage.Content = message;
            Type = type;
            _error = error;

            string imageSource = "";

            switch (type)
            {
                case Message.Normal:
                    imageSource = @"/Resource/Image/icon-checkmark-32.png";
                    break;
                case Message.Error:
                    imageSource = @"/Resource/Image/icon-cross-mark-32.png";
                    break;
                case Message.Info:
                    imageSource = @"/Resource/Image/icon-info-32.png";
                    break;
            }

            MessageIcon.Source = new BitmapImage(new Uri(imageSource, UriKind.Relative));

            // If type normal, info - show panel briefly then hide
            // If type error - show but don't hide
            Storyboard animatePanel = Resources[type != Message.Error ? "SBShowHideMessage" : "SBShowMessage"] as Storyboard;
            animatePanel.Begin(PanelMessage);

        }
        public void ShowOverlay()
        {
            Storyboard animatePanel = Resources["SBShowOverlay"] as Storyboard;
            animatePanel.Begin(PanelOverlay);
        }

        public void HideOverlay()
        {
            Storyboard animatePanel = Resources["SBHideOverlay"] as Storyboard;
            animatePanel.Begin(PanelOverlay);
        }
        private void ButtonAction_Click(object sender, RoutedEventArgs e)
        {
            Button actionButton = sender as Button;

            switch (actionButton.Name)
            {
                case "ViewError":
                    MessageBox.Show(_error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;

            }

            // Hide Panel
            Storyboard animatePanel = Resources["SBHideMessage"] as Storyboard;
            animatePanel.Begin(PanelMessage);

        }
#if DEBUG
        private static bool fullScreen = false;
#else
        private static bool fullScreen = true;
#endif
        private void Main_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F11)
            {
                if (!fullScreen)
                {
                    WindowStyle = WindowStyle.None;
                    WindowState = WindowState.Maximized;
                }
                else
                {
                    WindowStyle = WindowStyle.SingleBorderWindow;
                    WindowState = WindowState.Normal;
                }

                fullScreen = !fullScreen;
            }
        }


        public void SetProfile()
        {
            LabelUserName.Content = UserProxy.CurrentUser.FullName;
            ImageProfile.Source = new BitmapImage(new Uri(UserProxy.CurrentUser.Gender == "P" ? @"/Resource/Image/icon-user-female2-50.png" : @"/Resource/Image/icon-user-male2-50.png", UriKind.Relative));
        }

        private BaseUI UserControl(string name)
        {
            BaseUI userControl = new BaseUI();
            switch (name)
            {
                case "User":
                    userControl = new EmployeeUI();
                    break;
                case "Absent":
                    userControl = new AbsentUI();
                    break;
            }

            return userControl;

        }

    }
}
