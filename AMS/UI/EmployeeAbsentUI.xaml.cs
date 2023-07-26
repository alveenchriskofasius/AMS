using AMS.Proxy;
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
namespace AMS.UI
{
    /// <summary>
    /// Interaction logic for EmployeeAbsentUI.xaml
    /// </summary>
    public partial class EmployeeAbsentUI : BaseUI
    {
        public EmployeeAbsentUI()
        {
            InitializeComponent();
        }
        public void FillGrid(FilterVO filter, int id)
        {
            try
            {
                GridAbsent.ItemsSource = AbsentProxy.DataList(filter, id);
            }
            catch (Exception e)
            {
                Main.ShowMessage("Gagal tarik data", e.Message, Message.Error);
            }
        }
    }
}
