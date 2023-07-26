using AMS.VO;
using Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AMS.Proxy
{
    internal class AbsentProxy
    {
        public static List<AbsentVO> DataList(FilterVO filter, int ID)
        {
            List<AbsentVO> absents = new List<AbsentVO>();

            object[] parameters = {
                "@EmployeeID", ID,
                "@Date",filter.Date
            };

            DataSet result = DBHelper.ExecuteProcedure("uspAbsentCheckGet", parameters);

            foreach (DataRow dataRow in result.Tables[1].Rows)
            {
                absents.Add(new AbsentVO(dataRow));
            }

            // has results
            return absents;

        }
        public static List<AbsentVO> DataListAbsent(FilterVO filter)
        {
            List<AbsentVO> absents = new List<AbsentVO>();

            object[] parameters = {
                "@Name",filter.FullName,
                "@Date", filter.Date
            };

            DataSet result = DBHelper.ExecuteProcedure("uspAbsentListGet", parameters);

            foreach (DataRow dataRow in result.Tables[0].Rows)
            {
                absents.Add(new AbsentVO(dataRow));
            }

            // has results
            return absents.GroupBy(u => u.EmployeeID).Select(grp => grp.ToList().Last()).ToList();

        }

        public static AbsentVO Data(int ID)
        {
            AbsentVO absent = new AbsentVO();
            object[] parameters = {
                "@EmployeeID", ID,
                "@Date",DateTime.Now
            };
            DataSet result = DBHelper.ExecuteProcedure("uspAbsentCheckGet", parameters);
            if (result.Tables[0].Rows.Count > 0)
            {
                absent.SetValue(result.Tables[0].Rows[0]);
            }

            // has results
            return absent;
        }

        public static void Save(AbsentVO absent)
        {
            if (absent.ID == 0)
            {
                object[] parameters = {
                    "@Date",absent.Date,
                    "@CheckIn",absent.CheckIn,
                    "@CheckOut",absent.CheckOut,
                    "@EmployeeID",absent.EmployeeID,
                    "@Status",absent.Status,
                    "@Image",absent.URLImage
            };

                DataSet result = DBHelper.ExecuteProcedure("uspAbsentAdd", parameters);

                if (result.Tables.Count > 0)
                {
                    DataTable lastTable = result.Tables[result.Tables.Count - 1];
                    if (lastTable.Rows.Count == 1)
                    {
                        // retrieve the ID
                        absent.ID = Convert.ToInt32(lastTable.Rows[0]["ID"]);
                    }
                }

            }
            else
            {
                object[] parameters = {
                    "@ID", absent.ID,
                    "@Date",absent.Date,
                    "@CheckIn",absent.CheckIn,
                    "@CheckOut",absent.CheckOut,
                    "@EmployeeID",absent.EmployeeID,
                    "@Status",absent.Status,
                    "@Image",absent.URLImage
                };

                DBHelper.ExecuteProcedure("uspAbsentUpdate", parameters);
            }
        }
    }
}
