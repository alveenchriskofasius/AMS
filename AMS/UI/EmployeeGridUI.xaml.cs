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
using AMS.VO;
using AMS.Proxy;
using System.Windows.Threading;

namespace AMS.UI
{
    /// <summary>
    /// Interaction logic for EmployeeGridUI.xaml
    /// </summary>
    public partial class EmployeeGridUI : BaseUI
    {
        EmployeeVO _employee;
        FilterVO _filter;
        FilterVO _oldFilter;
        public EmployeeGridUI()
        {
            InitializeComponent();
        }

        private void BaseUI_Loaded(object sender, RoutedEventArgs e)
        {
            _employee = new EmployeeVO();
            _filter = new FilterVO();
            _oldFilter = new FilterVO();
            DataContext = _filter;
            FillComboLookup(ComboPosition, "Position", "Semua Posisi");
            FillComboLookup(ComboReligion, "Religion", "Semua Agama");
            FillComboLookup(ComboEmployeeStatus, "EmployeeStatus", "Semua Status");
        }
        #region Event Handler
        public event EventHandler<DataChangedEventArgs> RowDoubleClicked;
        #endregion
        #region Event declarations

        protected virtual void OnRowDoubleClicked(DataChangedEventArgs e)
        {
            EventHandler<DataChangedEventArgs> handler = RowDoubleClicked;
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
                GridEmployee.ItemsSource = EmployeeProxy.DataList(_filter);
                _oldFilter.SetValue(_filter);
            }
            catch (Exception ex)
            {
                Main.ShowMessage("Gagal Tarik data", ex.Message);
            }
        }
        private void ButtonAction_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            switch (button.Name)
            {
                case "ButtonReset":
                    FilterVO newFilter = new FilterVO();
                    _filter.SetValue(newFilter);
                    FillGrid(); // apply filters
                    break;

                case "ButtonRefresh":
                    FillGrid();
                    break;

            }
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

        private void GridEmployee_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EmployeeVO employee = GridEmployee.SelectedItem as EmployeeVO;
            if (employee != null)
            {
                OnRowDoubleClicked(new DataChangedEventArgs(employee));
            }
        }

        private void Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Perform search when filter is changed 
            if (!_oldFilter.Compare(_filter))
            {
                FillGrid();
            }
        }

        private void Check_Checked(object sender, RoutedEventArgs e)
        {
            // Perform search when filter is changed 
            if (!_oldFilter.Compare(_filter))
            {
                FillGrid();
            }
        }
    }
}
