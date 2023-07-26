using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace AMS.Resource.Theme.JMP
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class Menu : ResourceDictionary
    {
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            //PopupUI popup = AMS.Tool.UIHelper.GetAscendantByType((Visual)sender, typeof(PopupUI)) as PopupUI;

            MenuItem menuItem = Tool.UIHelper.GetAscendantByType((Visual)sender, typeof(MenuItem)) as MenuItem;
            //Menu menu = Tool.UIHelper.GetAscendantByType(menuItem, typeof(Menu)) as Menu;

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                // Put long-running operation here
                menuItem.RaiseEvent(new RoutedEventArgs(MenuItem.CheckedEvent));
            }));

            (menuItem.Parent as MenuItem).IsSubmenuOpen = false;
            //menu.IsOpen = false;
        }
    }
}
