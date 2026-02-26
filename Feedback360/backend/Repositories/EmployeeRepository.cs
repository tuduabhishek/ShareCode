using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using feedback360.Backend.DTOs;

namespace feedback360.Backend.Repositories
{
    public interface IEmployeeRepository
    {
        IEnumerable<LookupDTO> GetDepartments(string adminId, bool isSuperAdmin);
        IEnumerable<LookupDTO> GetExecHeads(string adminId, bool isSuperAdmin);
        IEnumerable<LookupDTO> GetSubAreas(string adminId, bool isSuperAdmin);
        IEnumerable<LookupDTO> GetDesignations(string adminId, bool isSuperAdmin);
        IEnumerable<string> SearchEmployees(string prefix);
        EmployeeDTO GetEmployee(string perno, string year, string cycle);
        bool SaveEmployee(EmployeeDTO employee);
        bool UpdateEmployee(EmployeeDTO employee);
    }

    public class EmployeeRepository : BaseRepository, IEmployeeRepository
    {
        public IEnumerable<LookupDTO> GetDepartments(string adminId, bool isSuperAdmin)
        {
            string sql = "select distinct ema_dept_code ID, ema_dept_desc TEXT from hrps.t_emp_master_feedback360 where ema_comp_code='1000' and ema_dept_desc<>'Not found'";
            if (!isSuperAdmin) sql += " and ema_bhr_pno=:adminId";
            sql += " order by ema_dept_desc";
            
            var parameters = isSuperAdmin ? null : new[] { new OracleParameter("adminId", adminId) };
            return FetchLookups(sql, parameters);
        }

        public IEnumerable<LookupDTO> GetExecHeads(string adminId, bool isSuperAdmin)
        {
            string sql = "select distinct ema_exec_head ID, ema_exec_head_desc TEXT from hrps.t_emp_master_feedback360 where ema_exec_head_desc is not null and ema_exec_head<>'00000000' and ema_comp_code='1000'";
            if (!isSuperAdmin) sql += " and ema_bhr_pno=:adminId";
            sql += " order by ema_exec_head_desc";

            var parameters = isSuperAdmin ? null : new[] { new OracleParameter("adminId", adminId) };
            return FetchLookups(sql, parameters);
        }

        public IEnumerable<LookupDTO> GetSubAreas(string adminId, bool isSuperAdmin)
        {
            string sql = "select distinct EMA_PERS_SUBAREA ID, EMA_PERS_SUBAREA_DESC TEXT from hrps.t_emp_master_feedback360 where EMA_PERS_SUBAREA_DESC is not null and ema_comp_code='1000'";
            if (!isSuperAdmin) sql += " and ema_bhr_pno=:adminId";
            sql += " order by EMA_PERS_SUBAREA_DESC";

            var parameters = isSuperAdmin ? null : new[] { new OracleParameter("adminId", adminId) };
            return FetchLookups(sql, parameters);
        }

        public IEnumerable<LookupDTO> GetDesignations(string adminId, bool isSuperAdmin)
        {
            string sql = "select distinct EMA_DESGN_CODE ID, EMA_DESGN_DESC TEXT from hrps.t_emp_master_feedback360 where EMA_DESGN_DESC is not null and ema_comp_code='1000'";
            if (!isSuperAdmin) sql += " and ema_bhr_pno=:adminId";
            sql += " order by EMA_DESGN_DESC";

            var parameters = isSuperAdmin ? null : new[] { new OracleParameter("adminId", adminId) };
            return FetchLookups(sql, parameters);
        }

        private IEnumerable<LookupDTO> FetchLookups(string sql, OracleParameter[] parameters)
        {
            var list = new List<LookupDTO>();
            DataTable dt = ExecuteQuery(sql, parameters);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new LookupDTO { Id = row["ID"].ToString(), Text = row["TEXT"].ToString() });
            }
            return list;
        }

        public IEnumerable<string> SearchEmployees(string prefix)
        {
            var list = new List<string>();
            string sql = "select EMA_ENAME||'('||EMA_PERNO||')' empPerno from tips.T_empl_all where (EMA_PERNO like :prefix or upper(EMA_ENAME) like :prefix)";
            var parameters = new[] { new OracleParameter("prefix", "%" + prefix.ToUpper() + "%") };
            
            DataTable dt = ExecuteQuery(sql, parameters);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(row["empPerno"].ToString());
            }
            return list;
        }

        public EmployeeDTO GetEmployee(string perno, string year, string cycle)
        {
            string sql = "select * from hrps.T_EMP_MASTER_FEEDBACK360 where ema_perno = :perno and EMA_YEAR = :year and EMA_CYCLE = :cycle";
            var parameters = new[] { 
                new OracleParameter("perno", perno),
                new OracleParameter("year", year),
                new OracleParameter("cycle", cycle)
            };
            DataTable dt = ExecuteQuery(sql, parameters);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new EmployeeDTO {
                    Perno = row["ema_perno"].ToString(),
                    EmployeeName = row["ema_ename"].ToString(),
                    DesignationCode = row["ema_desgn_code"].ToString(),
                    Email = row["ema_email_id"].ToString(),
                    EqvLevel = row["ema_eqv_level"].ToString(),
                    ContactNo = row["ema_phone_no"].ToString(),
                    SubAreaCode = row["ema_pers_subarea"].ToString(),
                    ReportingTo = row["ema_reporting_to_pno"].ToString(),
                    BuhrNo = row["ema_bhr_pno"].ToString(),
                    BuhrName = row["ema_bhr_name"].ToString(),
                    JoiningDate = row["EMA_JOINING_DT"] != DBNull.Value ? ((DateTime)row["EMA_JOINING_DT"]).ToString("dd-MM-yyyy") : "",
                    DeptCode = row["EMA_DEPT_CODE"].ToString(),
                    SGrade = row["EMA_EMPL_SGRADE"].ToString(),
                    EmpClass = row["EMA_EMP_CLASS"].ToString(),
                    DottedPno = row["EMA_DOTTED_PNO"].ToString(),
                    PersExecPno = row["EMA_PERS_EXEC_PNO"].ToString(),
                    ExecHeadCode = row["EMA_EXEC_HEAD"].ToString(),
                    Step1Start = row["EMA_STEP1_STDT"] != DBNull.Value ? ((DateTime)row["EMA_STEP1_STDT"]).ToString("dd-MM-yyyy") : "",
                    Step1End = row["EMA_STEP1_ENDDT"] != DBNull.Value ? ((DateTime)row["EMA_STEP1_ENDDT"]).ToString("dd-MM-yyyy") : ""
                    // ... other steps
                };
            }
            return null;
        }

        public bool SaveEmployee(EmployeeDTO e)
        {
            string sql = @"INSERT INTO hrps.T_EMP_MASTER_FEEDBACK360 
                           (ema_year, ema_cycle, ema_perno, ema_ename, ema_desgn_code, ema_desgn_desc, ema_email_id, ema_eqv_level, 
                            ema_phone_no, ema_pers_subarea, ema_pers_subarea_desc, ema_comp_code, ema_reporting_to_pno, 
                            ema_bhr_pno, ema_bhr_name, ema_joining_dt, ema_dept_code, ema_dept_desc, ema_empl_sgrade, 
                            ema_emp_class, ema_dotted_pno, ema_pers_exec_pno, ema_exec_head, ema_exec_head_desc,
                            ema_step1_stdt, ema_step1_enddt, ema_step2_stdt, ema_step2_enddt, ema_step3_stdt, ema_step3_enddt) 
                           VALUES (:year, :cycle, :perno, :name, :dcode, :ddesc, :email, :eqv, :phone, :sacode, :sadesc, '1000', :rpt, :buhr, :buhrnm, 
                                   to_date(:jdt,'dd-mm-yyyy'), :dept, :deptdesc, :sgrade, :cls, :dotted, :perex, :exh, :exhdesc,
                                   to_date(:s1s,'dd-mm-yyyy'), to_date(:s1e,'dd-mm-yyyy'), 
                                   to_date(:s2s,'dd-mm-yyyy'), to_date(:s2e,'dd-mm-yyyy'), 
                                   to_date(:s3s,'dd-mm-yyyy'), to_date(:s3e,'dd-mm-yyyy'))";
            
            return ExecuteNonQuery(sql, GetEmployeeParameters(e)) > 0;
        }

        public bool UpdateEmployee(EmployeeDTO e)
        {
            string sql = @"UPDATE hrps.T_EMP_MASTER_FEEDBACK360 SET 
                            ema_ename=:name, ema_desgn_code=:dcode, ema_desgn_desc=:ddesc, ema_email_id=:email, 
                            ema_eqv_level=:eqv, ema_phone_no=:phone, ema_pers_subarea=:sacode, ema_pers_subarea_desc=:sadesc, 
                            ema_reporting_to_pno=:rpt, ema_bhr_pno=:buhr, ema_bhr_name=:buhrnm, 
                            ema_joining_dt=to_date(:jdt,'dd-mm-yyyy'), ema_dept_code=:dept, ema_dept_desc=:deptdesc, 
                            ema_empl_sgrade=:sgrade, ema_emp_class=:cls, ema_dotted_pno=:dotted, 
                            ema_pers_exec_pno=:perex, ema_exec_head=:exh, ema_exec_head_desc=:exhdesc,
                            ema_step1_stdt=to_date(:s1s,'dd-mm-yyyy'), ema_step1_enddt=to_date(:s1e,'dd-mm-yyyy'), 
                            ema_step2_stdt=to_date(:s2s,'dd-mm-yyyy'), ema_step2_enddt=to_date(:s1e,'dd-mm-yyyy'), 
                            ema_step3_stdt=to_date(:s3s,'dd-mm-yyyy'), ema_step3_enddt=to_date(:s3e,'dd-mm-yyyy')
                           WHERE ema_perno=:perno and ema_year=:year and ema_cycle=:cycle";

            return ExecuteNonQuery(sql, GetEmployeeParameters(e)) > 0;
        }

        private OracleParameter[] GetEmployeeParameters(EmployeeDTO e)
        {
            return new[] {
                new OracleParameter("year", e.Year),
                new OracleParameter("cycle", e.Cycle),
                new OracleParameter("perno", e.Perno),
                new OracleParameter("name", e.EmployeeName),
                new OracleParameter("dcode", e.DesignationCode),
                new OracleParameter("ddesc", e.DesignationDesc),
                new OracleParameter("email", e.Email),
                new OracleParameter("eqv", e.EqvLevel),
                new OracleParameter("phone", e.ContactNo),
                new OracleParameter("sacode", e.SubAreaCode),
                new OracleParameter("sadesc", e.SubAreaDesc),
                new OracleParameter("rpt", e.ReportingTo),
                new OracleParameter("buhr", e.BuhrNo),
                new OracleParameter("buhrnm", e.BuhrName),
                new OracleParameter("jdt", e.JoiningDate),
                new OracleParameter("dept", e.DeptCode),
                new OracleParameter("deptdesc", e.DeptDesc),
                new OracleParameter("sgrade", e.SGrade),
                new OracleParameter("cls", e.EmpClass),
                new OracleParameter("dotted", e.DottedPno),
                new OracleParameter("perex", e.PersExecPno),
                new OracleParameter("exh", e.ExecHeadCode),
                new OracleParameter("exhdesc", e.ExecHeadDesc),
                new OracleParameter("s1s", e.Step1Start),
                new OracleParameter("s1e", e.Step1End),
                new OracleParameter("s2s", e.Step2Start),
                new OracleParameter("s2e", e.Step2End),
                new OracleParameter("s3s", e.Step3Start),
                new OracleParameter("s3e", e.Step3End)
            };
        }
    }
}
