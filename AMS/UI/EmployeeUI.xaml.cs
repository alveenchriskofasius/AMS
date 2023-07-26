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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AMS.VO;
namespace AMS.UI
{
    /// <summary>
    /// Interaction logic for EmployeeUI.xaml
    /// </summary>
    public partial class EmployeeUI : BaseUI
    {
        public EmployeeUI()
        {
            InitializeComponent();
        }

        private void BaseUI_Loaded(object sender, RoutedEventArgs e)
        {
            EmployeeSelector.Main = Main;
            EmployeeGrid.Main = Main;
        }

        private void Popup(EmployeeVO employee)
        {
            EmployeeFormUI employeeForm = new EmployeeFormUI();
            PopupUI popup = new PopupUI();
            popup.Main = this.Main;
            employeeForm.Main = popup.Main;
            popup.Title = "Master karyawan | Form Karyawan";
            popup.PanelPopup.Width = 800;
            popup.PanelPopup.Height = 850;
            popup.PanelMain.Children.Add(employeeForm);
            employeeForm.DataChanged += Employee_DataChanged;

            if (employee != null)
            {
                employeeForm.FillForm(employee);
            }
            else
            {
                employeeForm.FillForm(null);
            }
            popup.Main.ShowOverlay();
            popup.ShowDialog();
        }
        private void EmployeeSelector_AddEmployee(object sender, EventArgs e)
        {
            Popup(null);
        }
        private void PanelOverlay_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // Hide Panel
            if (PanelOverlay.Opacity == 0.4)
            {
                ToggleSearchPanel();
            }
        }

        #region Helper

        private void ToggleSearchPanel()
        {
            if (PanelSearch.Visibility == Visibility.Collapsed)
            {
                // Show Panel
                Storyboard animatePanel = Application.Current.Resources["SBShowBottom"] as Storyboard;
                animatePanel.Begin(PanelSearch);
                animatePanel = Application.Current.Resources["SBOverlayFadeIn"] as Storyboard;
                animatePanel.Begin(PanelOverlay);
                EmployeeGrid.FillGrid();
            }
            else
            {
                // Hide Panel
                Storyboard animatePanel = Application.Current.Resources["SBHideBottom"] as Storyboard;
                animatePanel.Begin(PanelSearch);
                animatePanel = Application.Current.Resources["SBOverlayFadeOut"] as Storyboard;
                animatePanel.Begin(PanelOverlay);
            }
        }

        #endregion
        private void Employee_DataChanged(object sender, EventArgs e)
        {
            EmployeeGrid.FillGrid();
            EmployeeSelector.FillGrid();
        }

        private void EmployeeSelector_SearchClicked(object sender, EventArgs e)
        {
            ToggleSearchPanel();
        }

        private void EmployeeGrid_RowDoubleClicked(object sender, DataChangedEventArgs e)
        {
            Popup(e.Entity as EmployeeVO);
        }
    }
}
