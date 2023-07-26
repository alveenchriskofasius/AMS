using AMS.Proxy;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace AMS.VO
{

    public class EmployeeVO : BaseVO
    {
        #region Property setters/getters
        private int _id = 0;
        public int ID
        {
            get { return _id; }
            set
            {
                SetProperty(ref _id, value);
            }
        }


        private string _fullName = "";
        public string FullName
        {
            get { return _fullName; }
            set
            {
                SetProperty(ref _fullName, value);
            }
        }

        private string _nik = "";
        public string NIK
        {
            get { return _nik; }
            set
            {
                SetProperty(ref _nik, value);
            }
        }

        private int _bloodTypeID = 0;
        public int BloodTypeID
        {
            get { return _bloodTypeID; }
            set
            {
                SetProperty(ref _bloodTypeID, value);
            }
        }
        private int _religionID = 0;
        public int ReligionID
        {
            get { return _religionID; }
            set
            {
                SetProperty(ref _religionID, value);
            }
        }
        private string _religionName = "";
        public string ReligionName
        {
            get { return _religionName; }
            set
            {
                SetProperty(ref _religionName, value);
            }
        }
        private string _placeDOB = "";
        public string PlaceDOB
        {
            get { return _placeDOB; }
            set
            {
                SetProperty(ref _placeDOB, value);
            }
        }
        private DateTime _date = DateTime.Now;
        public DateTime Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }

        private string _gender;
        public string Gender
        {
            get { return _gender; }
            set
            {
                SetProperty(ref _gender, value);
            }
        }


        private int _positionID;
        public int PositionID
        {
            get { return _positionID; }
            set { SetProperty(ref _positionID, value); }
        }
        private string _positionName;
        public string PositionName
        {
            get { return _positionName; }
            set { SetProperty(ref _positionName, value); }
        }
        private int _employeeStatusID;
        public int EmployeeStatusID
        {
            get { return _employeeStatusID; }
            set { SetProperty(ref _employeeStatusID, value); }
        }
        private string _employeeStatusName = "";
        public string EmployeeStatusName
        {
            get { return _employeeStatusName; }
            set
            {
                SetProperty(ref _employeeStatusName, value);
            }
        }
        private int _maritalStatusID;
        public int MaritalStatusID
        {
            get { return _maritalStatusID; }
            set { SetProperty(ref _maritalStatusID, value); }
        }
        private string _email = "";
        public string Email
        {
            get { return _email; }
            set
            {
                SetProperty(ref _email, value);
            }
        }
        private string _telephone = "";
        public string Telephone
        {
            get { return _telephone; }
            set { SetProperty(ref _telephone, value); }
        }
        private string _phone = "";
        public string Phone
        {
            get { return _phone; }
            set { SetProperty(ref _phone, value); }
        }
        private string _address = "";
        public string Address
        {
            get { return _address; }
            set
            {
                SetProperty(ref _address, value);
            }
        }
        private string _note = "";
        public string Note
        {
            get { return _note; }
            set
            {
                SetProperty(ref _note, value);
            }
        }
        public ImageSource Picture { get; set; }
        private byte[] _urlImage = new byte[0];
        public byte[] URLImage
        {
            get { return _urlImage; }
            set { SetProperty(ref _urlImage, value); }
        }

        private string _url = "/Resource/Image/No-image.png";
        public string ImagePath
        {
            get { return _url; }
            set { SetProperty(ref _url, value); }
        }

        private bool _active = true;
        public bool Active
        {
            get { return _active; }
            set
            {
                SetProperty(ref _active, value);
            }
        }

        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                SetProperty(ref _username, value);
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                SetProperty(ref _password, value);
            }
        }

        private string _checkIn = "";
        public string CheckIn
        {
            get { return _checkIn; }
            set { SetProperty(ref _checkIn, value); }
        }

        private string _checkOut = "";
        public string CheckOut
        {
            get { return _checkOut; }
            set { SetProperty(ref _checkOut, value); }
        }
        #endregion

        public EmployeeVO()
        {
            SetDefaultValues();
        }
        private void SetDefaultValues()
        {
            // Init vars
            Gender = "L";
        }
        public EmployeeVO(EmployeeVO employee)
        {
            SetDefaultValues();
            SetValue(employee);
        }

        public EmployeeVO(DataRow dataRow)
        {
            SetValue(dataRow);
        }

        #region BaseVO Members
        public override string GetError(string columnName)
        {
            string result = null;
            switch (columnName)
            {
                case "FullName":
                    if (string.IsNullOrEmpty(FullName))
                    {
                        result = "Nama lengkap wajib diisi";
                    }
                    break;
                case "NIK":
                    if (string.IsNullOrEmpty(NIK))
                    {
                        result = "KTP wajib diisi";
                    }
                    else if (NIK.Length < 16 || NIK.Length > 16)
                    {
                        result = "KTP harus 16 digit";
                    }
                    break;
                case "BloodTypeID":
                    if (BloodTypeID == 0)
                    {
                        result = "Golongan Darah wajib diisi";
                    }
                    break;
                case "ReligionID":
                    if (ReligionID == 0)
                    {
                        result = "Agama wajib diisi";
                    }
                    break;
                case "PlaceDOB":
                    if (string.IsNullOrEmpty(PlaceDOB))
                    {
                        result = "Tempat Lahir wajib diisi";
                    }
                    break;
                case "Date":
                    DateTime dt_now = DateTime.Now;

                    DateTime dt_18 = Date.AddYears(18);
                    if (dt_18.Date >= dt_now.Date)
                    {
                        result = "Umur minimal 18 tahun";
                    }
                    break;
                case "Gender":
                    if (string.IsNullOrEmpty(Gender))
                    {
                        result = "Jenis Kelamin wajib diisi";
                    }
                    break;
                case "PositionID":
                    if (PositionID == 0)
                    {
                        result = "Posisi wajib diisi";
                    }
                    break;
                case "EmployeeStatusID":
                    if (EmployeeStatusID == 0)
                    {
                        result = "Status Karyawan wajib diisi";
                    }
                    break;
                case "MaritalStatusID":
                    if (MaritalStatusID == 0)
                    {
                        result = "Status Pernikahan wajib diisi";
                    }
                    break;
                case "Address":
                    if (string.IsNullOrEmpty(Address))
                    {
                        result = "Alamat wajib diisi";
                    }
                    break;
                case "Password":
                    if (string.IsNullOrEmpty(Password))
                    {
                        result = "Password wajib diisi";
                    }
                    break;
                case "Phone":
                    if (string.IsNullOrEmpty(Phone))
                    {
                        result = "Nomor HP wajib diisi";
                    }
                    else if (Phone.Length > 12)
                    {
                        result = "Nomor HP maksimal 12 digit";
                    }
                    else if (Phone.Length < 10)
                    {
                        result = "Nomor HP minimal 10 digit";
                    }
                    break;
                case "Email":
                    if (!Regex.IsMatch(Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                        result = "Email tidak valid";
                    if (string.IsNullOrEmpty(Email))
                        result = "Email wajib diisi";
                    break;

                case "Username":
                    if (ID == 0)
                    {
                        if (string.IsNullOrEmpty(Username))
                            result = "Username wajib diisi";
                        ObservableCollection<UserVO> users = UserProxy.Data();
                        foreach (UserVO user in users)
                        {
                            if (Username == user.Username && ID == 0)
                            {
                                result = "Username sudah ada!";
                            }
                        }
                    }

                    break;
            }

            return result;
        }

        #endregion

    }
}
