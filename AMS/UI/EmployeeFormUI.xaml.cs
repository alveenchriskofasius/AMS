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
using AMS.Tool;
using Microsoft.Win32;
using System.IO;
using System.Text.RegularExpressions;

namespace AMS.UI
{
    /// <summary>
    /// Interaction logic for EmployeeFormUI.xaml
    /// </summary>
    public partial class EmployeeFormUI : BaseUI
    {
        EmployeeVO _employee;
        private int _noOfErrorsOnScreen;

        public event EventHandler DataChanged;

        #region Event declarations
        protected virtual void OnDataChanged(EventArgs e)
        {
            EventHandler handler = DataChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        public EmployeeFormUI()
        {
            InitializeComponent();
            _employee = new EmployeeVO();
            DataContext = _employee;
            FillComboLookup(ComboPosition, "Position");
            FillComboLookup(ComboReligion, "Religion");
            FillComboLookup(ComboMaritalStatus, "MaritalStatus");
            FillComboLookup(ComboEmployeeStatus, "EmployeeStatus");
            FillComboLookup(ComboBloodType, "BloodType");
        }


        #region Command Definitions
        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _noOfErrorsOnScreen == 0;
            e.Handled = true;
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Save();
            e.Handled = true;
        }

        #endregion
        public void Save()
        {
            try
            {
                EmployeeProxy.Save(_employee);
                // Notify saved data
                OnDataChanged(new EventArgs());

                // Reset form
                FillForm(null);

                // Show message
                Main.ShowMessage("Data berhasil disimpan");

            }
            catch (Exception ex)
            {
                Main.ShowMessage("Data gagal disimpan", ex.ToString());
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

        public void FillForm(EmployeeVO employee)
        {

            // new record
            if (employee == null)
            {
                employee = new EmployeeVO();
                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                var uri = new Uri(@"/Resource/Image/No-image.png", UriKind.Relative);
                myBitmapImage.UriSource = uri;
                myBitmapImage.DecodePixelWidth = 200;
                myBitmapImage.EndInit();
                ImageUpload.Source = myBitmapImage;
                _employee.SetValue(new EmployeeVO(employee));
            }
            else
            {
                _employee.SetValue(EmployeeProxy.Data(employee.ID));
                GetImagesFromDatabase(_employee.URLImage);
            }

            // Set radio button
            List<RadioButton> radioButtons = new List<RadioButton>();
            UIHelper.WalkLogicalTree(radioButtons, PanelGender);
            foreach (RadioButton rb in radioButtons)
            {
                if (rb.GroupName == "Gender")
                {
                    rb.IsChecked = rb.Content.ToString() == _employee.Gender;
                }
            }
            TextPassword.Password = _employee.Password;
            // Set focus on the first field for new employee
            if (employee.ID == 0)
            {
                TextFullName.Focus();
            }
            ButtonDelete.IsEnabled = employee.ID != 0 && employee.ID != UserProxy.CurrentUser.EmployeeID;
            TextUsername.IsEnabled = employee.ID == 0;
        }
        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                _noOfErrorsOnScreen++;
            else
                _noOfErrorsOnScreen--;
        }
        private void GenderButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton gender = sender as RadioButton;

            _employee.Gender = gender.Content.ToString();

        }
        private void ButtonAction_Click(object sender, RoutedEventArgs e)
        {

            Button button = sender as Button;

            switch (button.Name)
            {

                case "ButtonAdd":
                    FillForm(null);
                    break;

                case "ButtonDelete":
                    Delete();
                    break;

                case "ButtonPrint":
                    break;

            }

        }
        public void Delete()
        {

            if (_employee.ID == 0) return;

            if (MessageBox.Show("Hapus karyawan " + _employee.FullName + "?", "Konfirmasi Hapus", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    EmployeeProxy.Delete(_employee);

                    // Notify deleted data
                    OnDataChanged(new EventArgs());

                    // Reset form
                    FillForm(null);

                    // Display message
                    Main.ShowMessage("Data berhasil dihapus");

                }
                catch (Exception e)
                {
                    Main.ShowMessage("Data gagal dihapus", e.ToString());
                }
            }
        }
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
                _employee.URLImage = ReadFile(op.FileName);
            }
        }
        void GetImagesFromDatabase(byte[] pictureIdByte)
        {
            try
            {

                if (pictureIdByte.Length > 0)
                {
                    using (var stream = new MemoryStream(pictureIdByte))
                    {
                        _employee.Picture = BitmapFrame.Create(stream,
                                                BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                    }
                    ImageUpload.Source = _employee.Picture;
                }
            }
            catch (Exception e)
            {
                Main.ShowMessage("Gagal tarik gambar", e.Message, Message.Error);
            }
        }

        private void TextPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox password = sender as PasswordBox;
            _employee.Password = password.Password;
        }

        private void Text_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
