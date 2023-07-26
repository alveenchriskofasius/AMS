using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using AMS.VO;
using Helper;
namespace AMS.Proxy
{
    internal static class RoleProxy
    {
        private static RoleVO Role { get; set; }

        public static List<RoleVO> Data(int userID)
        {
            List<RoleVO> roles = new List<RoleVO>();
            object[] parameters =
            {
                "@UserID", userID,
            };
            DataSet result = DBHelper.ExecuteProcedure("uspUserRoleGet", parameters);

            foreach (DataRow dataRow in result.Tables[0].Rows)
            {
                roles.Add(new RoleVO(dataRow));
            }
            // has results
            return roles;
        }
    }
}
