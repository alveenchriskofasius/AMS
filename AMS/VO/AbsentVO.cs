using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AMS.VO
{
    internal class AbsentVO : BaseVO
    {
        private int _id = 0;
        public int ID
        {
            get { return _id; }
            set
            {
                SetProperty(ref _id, value);
            }
        }

        private DateTime _date = DateTime.Now;
        public DateTime Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
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

        private string _status = "";
        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        private string _content = "";
        public string Content
        {
            get { return _content; }
            set { SetProperty(ref _content, value); }
        }

        private int _employeeID;
        public int EmployeeID
        {
            get { return _employeeID; }
            set
            {
                SetProperty(ref _employeeID, value);
            }
        }

        private string _employeeName = "";
        public string EmployeeName
        {
            get { return _employeeName; }
            set { SetProperty(ref _employeeName, value); }
        }

        private bool _isUploaded = false;
        public bool IsUploaded
        {
            get { return _isUploaded; }
            set { SetProperty(ref _isUploaded, value); }
        }

        public ImageSource Picture { get; set; }
        private byte[] _urlImage = new byte[0];

        public byte[] URLImage
        {
            get { return _urlImage; }
            set { SetProperty(ref _urlImage, value); }
        }

        private string _positionName;
        public string PositionName
        {
            get { return _positionName; }
            set { SetProperty(ref _positionName, value); }
        }

        private string _url = "/Resource/Image/No-image.png";
        public string ImagePath
        {
            get { return _url; }
            set { SetProperty(ref _url, value); }
        }

        public AbsentVO()
        {
            Content = "Check In";
            Status = "Out";
        }
        private void SetDefaultValues()
        {
        }
        public AbsentVO(AbsentVO absent)
        {
            SetDefaultValues();
        }

        public AbsentVO(DataRow dataRow)
        {
            SetValue(dataRow);
        }
        public override string GetError(string columnName)
        {
            throw new NotImplementedException();
        }
    }
}
