using System;

namespace AMS.VO
{

    public class FilterVO : BaseVO
    {
        #region Property setters/getters
        private string _no = "";
        public string No
        {
            get { return _no; }
            set
            {
                SetProperty(ref _no, value);
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

        private int _positionID;
        public int PositionID
        {
            get { return _positionID; }
            set
            {
                SetProperty(ref _positionID, value);
            }
        }

        private int _religionID;
        public int ReligionID
        {
            get { return _religionID; }
            set
            {
                SetProperty(ref _religionID, value);
            }
        }

        private int _employeeStatusID;
        public int EmployeeStatusID
        {
            get { return _employeeStatusID; }
            set
            {
                SetProperty(ref _employeeStatusID, value);
            }
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

        private DateTime _date = DateTime.Now;
        public DateTime Date
        {
            get { return _date; }
            set
            {
                SetProperty(ref _date, value);
            }
        }

        #endregion

        public FilterVO()
        {
        }

        public FilterVO(FilterVO filter)
        {
            SetValue(filter);
        }

        #region BaseVO Members
        public override string GetError(string columnName)
        {
            string result = null;
            return result;
        }

        #endregion

    }

}
