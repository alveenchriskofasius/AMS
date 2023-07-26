using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AMS.VO;
using Helper;

namespace AMS.Proxy
{
    internal class EmployeeProxy
    {
        public static List<EmployeeVO> DataList(FilterVO filter)
        {
            List<EmployeeVO> employees = new List<EmployeeVO>();

            object[] parameters = {
                "@Name",filter.FullName,
                "@PositionID", filter.PositionID,
                "@ReligionID", filter.ReligionID,
                "@EmployeeStatusID", filter.EmployeeStatusID,
                "@Active", filter.Active
            };

            DataSet result = DBHelper.ExecuteProcedure("uspEmployeeListGet", parameters);

            foreach (DataRow dataRow in result.Tables[0].Rows)
            {
                employees.Add(new EmployeeVO(dataRow));
            }

            // has results
            return employees;

        }

        public static void Save(EmployeeVO employee)
        {
            if (employee.ID == 0)
            {
                object[] parameters = {
                    "@FullName",employee.FullName,
                    "@NIK",employee.NIK,
                    "@BloodTypeID",employee.BloodTypeID,
                    "@ReligionID",employee.ReligionID,
                    "@PlaceDOB",employee.PlaceDOB,
                    "@DOB",employee.Date,
                    "@Gender",employee.Gender,
                    "@PositionID",employee.PositionID,
                    "@EmployeeStatusID",employee.EmployeeStatusID,
                    "@MaritalStatusID",employee.MaritalStatusID,
                    "@Email",employee.Email,
                    "@Telephone", employee.Telephone,
                    "@Phone",employee.Phone,
                    "@Address",employee.Address,
                    "@Note",employee.Note,
                    "@Active",employee.Active,
                    "@Image",employee.URLImage,
                    "@Username",employee.Username,
                    "@Password",employee.Password
                };

                DataSet result = DBHelper.ExecuteProcedure("uspEmployeeAdd", parameters);

                if (result.Tables.Count > 0)
                {
                    DataTable lastTable = result.Tables[result.Tables.Count - 1];
                    if (lastTable.Rows.Count == 1)
                    {
                        // retrieve the ID
                        employee.ID = Convert.ToInt32(lastTable.Rows[0]["ID"]);
                    }
                }

            }
            else
            {
                object[] parameters = {
                    "@ID", employee.ID,
                    "@FullName",employee.FullName,
                    "@NIK",employee.NIK,
                    "@BloodTypeID",employee.BloodTypeID,
                    "@ReligionID",employee.ReligionID,
                    "@PlaceDOB",employee.PlaceDOB,
                    "@DOB",employee.Date,
                    "@Gender",employee.Gender,
                    "@PositionID",employee.PositionID,
                    "@EmployeeStatusID",employee.EmployeeStatusID,
                    "@MaritalStatusID",employee.MaritalStatusID,
                    "@Email",employee.Email,
                    "@Telephone", employee.Telephone,
                    "@Phone",employee.Phone,
                    "@Address",employee.Address,
                    "@Note",employee.Note,
                    "@Active",employee.Active,
                    "@Image",employee.URLImage,
                    "@Username",employee.Username,
                    "@Password",employee.Password
                };

                DBHelper.ExecuteProcedure("uspEmployeeUpdate", parameters);
            }
        }

        public static void Delete(EmployeeVO employee)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                object[] parameters = {
                    "@ID", employee.ID
                };

                DBHelper.ExecuteProcedure("uspEmployeeDelete", parameters);

                scope.Complete();
            }
        }
        public static EmployeeVO Data(int ID)
        {
            EmployeeVO employee = new EmployeeVO();
            object[] parameters = {
                "@ID", ID

            };
            DataSet result = DBHelper.ExecuteProcedure("uspEmployeeGet", parameters);

            employee.SetValue(result.Tables[0].Rows[0]);

            // has results
            return employee;
        }

    }
}
