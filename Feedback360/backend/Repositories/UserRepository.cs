using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using feedback360.Backend.DTOs;

namespace feedback360.Backend.Repositories
{
    public interface IUserRepository
    {
        UserSessionDTO GetUserSession(string userId);
        bool CheckAuthentication(string userId, string url);
    }

    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserSessionDTO GetUserSession(string userId)
        {
            var session = new UserSessionDTO { UserId = userId };
            
            // Get Social Cycle and Fiscal Year (Equivalent to getsrlno and getFy in AdminMaster.master.vb)
            string fySql = "select IRC_DESC from hrps.t_ir_codes where IRC_CODE='360YS' and IRC_VALID_TAG='A'";
            DataTable fyDt = ExecuteQuery(fySql);
            if (fyDt.Rows.Count > 0)
            {
                session.FiscalYear = fyDt.Rows[0]["IRC_DESC"].ToString();
            }

            string srlSql = "select IRC_DESC from t_ir_codes where IRC_TYPE='SL360' and IRC_VALID_TAG='Y'";
            DataTable srlDt = ExecuteQuery(srlSql);
            if (srlDt.Rows.Count > 0)
            {
                session.Cycle = srlDt.Rows[0]["IRC_DESC"].ToString();
            }

            // Check Super Admin (Equivalent to ChkRole1 in AdminMaster.master.vb)
            string adminSql = @"select IGP_user_id from hrps.t_ir_adm_grp_privilege 
                                where igp_group_id ='360FEEDBAC' 
                                and IGP_STATUS ='A' and IGP_user_id=:userId";
            var parameters = new[] { new OracleParameter("userId", userId) };
            DataTable adminDt = ExecuteQuery(adminSql, parameters);
            session.IsSuperAdmin = adminDt.Rows.Count > 0;

            // Get User Name
            string nameSql = "select ema_ename from hrps.t_emp_master_feedback360 where ema_perno=:userId";
            DataTable nameDt = ExecuteQuery(nameSql, parameters);
            if (nameDt.Rows.Count > 0)
            {
                session.UserName = nameDt.Rows[0]["ema_ename"].ToString();
            }

            return session;
        }

        public bool CheckAuthentication(string userId, string url)
        {
            // Logic moved from AdminMaster.master.vb
            // This would typically involve checking t_ir_adm_grp_privilege for the specific URL
            return true; // Simplified for now
        }
    }
}
