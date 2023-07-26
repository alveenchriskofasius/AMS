using AMS.VO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;
using AMS.Proxy;
namespace AMS.UI
{
    /// <summary>
    /// Interaction logic for AbsentUI.xaml
    /// </summary>
    public partial class AbsentUI : BaseUI
    {
        AbsentVO _absent;
        FilterVO _filter;
        FilterVO _oldFilter;
        DispatcherTimer timer;
        public AbsentUI()
        {
            InitializeComponent();
        }
        #region Command Definitions
        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_absent != null)
            {
                e.CanExecute = _absent.IsUploaded;
                e.Handled = true;
            }
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Save();
            e.Handled = true;
        }


        #endregion
        private void ButtonImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                ImageUpload.Source = new BitmapImage(new Uri(op.FileName));
                _absent.URLImage = ReadFile(op.FileName);
                _absent.IsUploaded = true;
                AbsentProxy.Save(_absent);
            }
        }
        private byte[] ReadFile(string sPath)
        {
            //Initialize byte array with a null value initially.
            byte[] data = null;

            //Use FileInfo object to get file size.
            FileInfo fInfo = new FileInfo(sPath);
            long numBytes = fInfo.Length;

            //Open FileStream to read file
            FileStream fStream = new FileStream(sPath, FileMode.Open, FileAccess.Read);

            //Use BinaryReader to read file stream into byte array.
            BinaryReader br = new BinaryReader(fStream);

            //When you use BinaryReader, you need to supply number of bytes to read from file.
            //In this case we want to read entire file. So supplying total number of bytes.
            data = br.ReadBytes((int)numBytes);
            return data;
        }

        private void BaseUI_Loaded(object sender, RoutedEventArgs e)
        {
            _absent = new AbsentVO();
            _filter = new FilterVO();
            _oldFilter = new FilterVO();

            DataContext = _absent;
            DatePicker.DataContext = _filter;

            Init();
            FillGrid();
        }
        private void Save()
        {
            if (!_absent.CheckIn.Equals(string.Empty) && !_absent.CheckOut.Equals(string.Empty))
            {
                _absent.SetValue(new AbsentVO { EmployeeID = UserProxy.CurrentUser.EmployeeID, URLImage = _absent.URLImage });
            }

            if (_absent.Status.Equals("Out"))
            {
                _absent.CheckIn = DateTime.Now.ToString("HH:mm");
                _absent.Status = "In";
            }
            else
            {
                _absent.CheckOut = DateTime.Now.ToString("HH:mm");
                _absent.Status = "Out";
            }


            _absent.Content = _absent.Status.Equals("Out") ? "Check In" : "Check Out";

            AbsentProxy.Save(_absent);
            FillGrid();
        }
        private void Init()
        {
            timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                TextTime.Text = DateTime.Now.ToString("HH:mm:ss");
            }, this.Dispatcher);
            _absent.SetValue(AbsentProxy.Data(UserProxy.CurrentUser.EmployeeID));
            _absent.EmployeeID = UserProxy.CurrentUser.EmployeeID;
            _absent.Content = _absent.Status.Equals("Out") ? "Check In" : "Check Out";
            GetImagesFromDatabase(_absent.URLImage);
        }
        void GetImagesFromDatabase(byte[] pictureIdByte)
        {
            try
            {

                if (pictureIdByte.Length > 0)
                {
                    using (var stream = new MemoryStream(pictureIdByte))
                    {
                        _absent.Picture = BitmapFrame.Create(stream,
                                                BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                    }
                    ImageUpload.Source = _absent.Picture;
                    _absent.IsUploaded = true;
                }
            }
            catch (Exception e)
            {
                Main.ShowMessage("Gagal tarik gambar", e.Message, Message.Error);
            }
        }
        private void FillGrid()
        {
            try
            {
                GridAbsent.ItemsSource = AbsentProxy.DataList(_filter, UserProxy.CurrentUser.EmployeeID);
                _oldFilter.SetValue(_filter);
            }
            catch (Exception e)
            {
                Main.ShowMessage("Gagal tarik data", e.Message, Message.Error);
            }
        }

        private void DatePicker_LostFocus(object sender, RoutedEventArgs e)
        {
            // Perform search when filter is changed 
            FillGrid();

        }
    }
}
