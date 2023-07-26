using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

using AMS.Proxy;
using AMS.VO;
namespace AMS.UI
{
    /// <summary>
    /// Interaction logic for EmployeeGridUI.xaml
    /// </summary>
    public partial class EmployeeSelectorUI : BaseUI
    {
        FilterVO _filter;
        FilterVO _oldFilter;

        public EmployeeSelectorUI()
        {
            InitializeComponent();
        }

        public event EventHandler DataChanged;
        public event EventHandler AddEmployee;
        public event EventHandler SearchClicked;
        protected virtual void OnSearchClicked(EventArgs e)
        {
            EventHandler handler = SearchClicked;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        #region Event declarations
        protected virtual void OnDataChanged(EventArgs e)
        {
            EventHandler handler = DataChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected virtual void OnAddEmployee(EventArgs e)
        {
            EventHandler handler = AddEmployee;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        #endregion
        public void FillGrid()
        {
            try
            {
                List<AbsentVO> absents = AbsentProxy.DataListAbsent(_filter);
                foreach (AbsentVO absent in absents)
                {
                    absent.Picture = GetImagesFromDatabase(absent.URLImage);
                }

                GridEmployee.ItemsSource = absents;

                _oldFilter.SetValue(_filter);


            }
            catch (Exception e)
            {

                Main.ShowMessage("Gagal tarik data", e.Message);
            }
        }
        private ImageSource GetImagesFromDatabase(byte[] pictureIdByte)
        {
            ImageSource image = null;
            try
            {
                if (pictureIdByte.Length > 0)
                {
                    using (var stream = new MemoryStream(pictureIdByte))
                    {
                        image = BitmapFrame.Create(stream,
                                BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                    }
                }
            }
            catch (Exception e)
            {
                Main.ShowMessage("Gagal tarik gambar", e.Message);

            }
            return image;

        }
        DispatcherTimer _typingTimer;
        private void TextName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_typingTimer == null)
            {
                _typingTimer = new DispatcherTimer();
                _typingTimer.Interval = TimeSpan.FromMilliseconds(500);

                _typingTimer.Tick += new EventHandler(this.HandleTypingTimerTimeout);
            }

            _typingTimer.Stop(); // Resets the timer
            _typingTimer.Start();

        }
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            FillGrid();
        }
        private void HandleTypingTimerTimeout(object sender, EventArgs e)
        {
            DispatcherTimer timer = sender as DispatcherTimer;

            if (timer == null)
            {
                return;
            }

            // Perform search when filter is changed 
            if (!_oldFilter.Compare(_filter))
            {
                FillGrid();
            }

            // The timer must be stopped! We want to act only once per keystroke.
            timer.Stop();
        }
        private void BaseUI_Loaded(object sender, RoutedEventArgs e)
        {
            _filter = new FilterVO();
            _oldFilter = new FilterVO();
            DataContext = _filter;
        }

        private void ButtonAction_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            switch (button.Name)
            {
                case "ButtonAdd":
                    OnAddEmployee(new EventArgs());
                    break;
                case "ButtonReset":
                    FilterVO newFilter = new FilterVO();
                    _filter.SetValue(newFilter);
                    FillGrid(); // apply filters
                    break;
                case "ButtonSearch":
                    OnSearchClicked(new EventArgs());
                    break;
                case "ButtonRefresh":
                    FillGrid();
                    break;

            }
        }

        private void EmployeeForm_DataChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AbsentVO absent = GridEmployee.SelectedItem as AbsentVO;
            PhotoUI photo = new PhotoUI
            {
                Main = this,
                Source = absent.Picture

            };

            Main.ShowOverlay();
            photo.ShowDialog();
        }

        private void GridEmployee_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AbsentVO absent = GridEmployee.SelectedItem as AbsentVO;
            EmployeeAbsentUI employeeAbsent = new EmployeeAbsentUI();
            PopupUI popup = new PopupUI();
            popup.Main = this.Main;
            employeeAbsent.Main = popup.Main;
            popup.Title = "Master karyawan | List Absen";
            popup.PanelPopup.Width = 800;
            popup.PanelPopup.Height = 850;
            popup.PanelMain.Children.Add(employeeAbsent);

            if (absent != null)
            {
                employeeAbsent.FillGrid(_filter, absent.EmployeeID);
            }
            popup.Main.ShowOverlay();
            popup.ShowDialog();
        }
    }
}
