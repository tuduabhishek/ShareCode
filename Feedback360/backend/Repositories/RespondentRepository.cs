using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Text;
using feedback360.Backend.DTOs;

namespace feedback360.Backend.Repositories
{
    public interface IRespondentRepository
    {
        IEnumerable<AssesseeDTO> GetAssesseesForAdmin(string adminId, bool isSuperAdmin, string fy, string cycle, string execHead = null, string dept = null, string perno = null);
        IEnumerable<RespondentRecordDTO> GetRespondents(string assesPno, string fy, string cycle);
        IEnumerable<ValidationRuleDTO> GetValidationRules(string level);
        string AddRespondent(RespondentSaveRequestDTO request);
        bool RemoveRespondent(string assesPno, string respondentPno, string category, string fy, string cycle);
        bool SubmitRespondentList(string assesPno, string fy, string cycle, string submittedBy);
        bool ApproveRespondentList(string assesPno, string fy, string cycle, string approvedBy);
        bool RejectRespondentList(string assesPno, string fy, string cycle, string rejectedBy);
    }

    public class RespondentRepository : BaseRepository, IRespondentRepository
    {
        public IEnumerable<AssesseeDTO> GetAssesseesForAdmin(string adminId, bool isSuperAdmin, string fy, string cycle, string execHead = null, string dept = null, string perno = null)
        {
            var list = new List<AssesseeDTO>();
            StringBuilder sb = new StringBuilder();
            sb.Append(@"select distinct SS_ASSES_PNO, a.ema_ename, a.ema_desgn_desc, a.EMA_EQV_LEVEL,
                        (select distinct ss_app_tag from hrps.t_survey_status where ss_asses_pno=SS_ASSES_PNO and ss_year=:fy and ss_srlno=:cycle and rownum=1) APP_STATUS,
                        (select distinct ss_tag from hrps.t_survey_status where ss_asses_pno=SS_ASSES_PNO and ss_year=:fy and ss_srlno=:cycle and rownum=1) SUBMIT_STATUS
                        from hrps.t_survey_status s
                        join hrps.t_emp_master_feedback360 a on a.ema_perno = s.SS_ASSES_PNO and a.ema_year = s.ss_year and a.ema_cycle = s.ss_srlno
                        where s.ss_year = :fy and s.SS_SRLNO = :cycle and s.ss_status = 'SE'");

            if (!isSuperAdmin) sb.Append(" and a.EMA_BHR_PNO = :adminId");
            if (!string.IsNullOrEmpty(execHead)) sb.Append(" and a.ema_exec_head = :execHead");
            if (!string.IsNullOrEmpty(dept)) sb.Append(" and a.ema_dept_code = :dept");
            if (!string.IsNullOrEmpty(perno)) sb.Append(" and s.SS_ASSES_PNO = :perno");

            var parameters = new List<OracleParameter> {
                new OracleParameter("fy", fy),
                new OracleParameter("cycle", cycle)
            };
            if (!isSuperAdmin) parameters.Add(new OracleParameter("adminId", adminId));
            if (!string.IsNullOrEmpty(execHead)) parameters.Add(new OracleParameter("execHead", execHead));
            if (!string.IsNullOrEmpty(dept)) parameters.Add(new OracleParameter("dept", dept));
            if (!string.IsNullOrEmpty(perno)) parameters.Add(new OracleParameter("perno", perno));

            DataTable dt = ExecuteQuery(sb.ToString(), parameters.ToArray());
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new AssesseeDTO {
                    Perno = row["SS_ASSES_PNO"].ToString(),
                    Name = row["ema_ename"].ToString(),
                    Designation = row["ema_desgn_desc"].ToString(),
                    Level = row["EMA_EQV_LEVEL"].ToString(),
                    AppStatus = row["APP_STATUS"].ToString(),
                    SubmitStatus = row["SUBMIT_STATUS"].ToString()
                });
            }
            return list;
        }

        public IEnumerable<RespondentRecordDTO> GetRespondents(string assesPno, string fy, string cycle)
        {
            var list = new List<RespondentRecordDTO>();
            string sql = @"select SS_PNO, SS_NAME, SS_DESG, SS_DEPT, SS_EMAIL, SS_CATEG, SS_TAG, SS_APP_TAG, SS_TYPE 
                           from hrps.t_survey_status 
                           where SS_ASSES_PNO = :assesPno and ss_year = :fy and SS_SRLNO = :cycle and nvl(SS_DEL_TAG, 'N') = 'N' 
                           order by SS_CATEG";

            var parameters = new[] {
                new OracleParameter("assesPno", assesPno),
                new OracleParameter("fy", fy),
                new OracleParameter("cycle", cycle)
            };

            DataTable dt = ExecuteQuery(sql, parameters);
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new RespondentRecordDTO {
                    Perno = row["SS_PNO"].ToString(),
                    Name = row["SS_NAME"].ToString(),
                    Designation = row["SS_DESG"].ToString(),
                    Department = row["SS_DEPT"].ToString(),
                    Email = row["SS_EMAIL"].ToString(),
                    Category = row["SS_CATEG"].ToString(),
                    Status = row["SS_TAG"].ToString(),
                    AppStatus = row["SS_APP_TAG"].ToString(),
                    Type = row["SS_TYPE"].ToString()
                });
            }
            return list;
        }

        public IEnumerable<ValidationRuleDTO> GetValidationRules(string level)
        {
            string type = "";
            switch (level) {
                case "I6": type = "360V6"; break;
                case "I5": type = "360V5"; break;
                case "I4": type = "360V4"; break;
                case "I3": type = "360V3"; break;
                case "I2": type = "360V2"; break;
                default: type = "360V1"; break;
            }

            string sql = @"select a.IRC_CODE, REGEXP_SUBSTR(a.IRC_DESC, '[^-]+', 1, 1) MIN_VAL, 
                           REGEXP_SUBSTR(a.IRC_DESC, '[^-]+', 1, 2) MAX_VAL, b.IRC_DESC CATEG_NAME
                           from t_ir_codes a
                           join t_ir_codes b on a.IRC_CODE = b.IRC_CODE and b.IRC_TYPE = '360RL'
                           where a.IRC_TYPE = :type and a.IRC_VALID_TAG = 'A'";

            var list = new List<ValidationRuleDTO>();
            DataTable dt = ExecuteQuery(sql, new[] { new OracleParameter("type", type) });
            foreach (DataRow row in dt.Rows)
            {
                list.Add(new ValidationRuleDTO {
                    Category = row["IRC_CODE"].ToString(),
                    CategoryName = row["CATEG_NAME"].ToString(),
                    MinCount = int.TryParse(row["MIN_VAL"].ToString(), out int min) ? min : 0,
                    MaxCount = row["MAX_VAL"].ToString() == "NA" ? 999 : (int.TryParse(row["MAX_VAL"].ToString(), out int max) ? max : 999)
                });
            }
            return list;
        }

        public string AddRespondent(RespondentSaveRequestDTO r)
        {
            string id = r.Type == "NORG" ? GenerateRefNo(r.FiscalYear) : r.Perno;
            
            string sql = @"insert into hrps.T_SURVEY_STATUS (SS_CATEG, SS_ID, SS_PNO, SS_NAME, SS_DESG, SS_DEPT, SS_EMAIL, 
                            SS_STATUS, SS_CRT_BY, SS_CRT_DT, SS_DEL_TAG, SS_TYPE, ss_year, SS_ASSES_PNO, SS_LEVEL, SS_SRLNO) 
                            values (:cat, :id, :pno, :name, :desg, :dept, :email, 'SE', :me, sysdate, 'N', :type, :fy, :asses, :lvl, :cycle)";

            var parameters = new[] {
                new OracleParameter("cat", r.Category),
                new OracleParameter("id", id),
                new OracleParameter("pno", r.Perno ?? id),
                new OracleParameter("name", r.Name),
                new OracleParameter("desg", r.Designation),
                new OracleParameter("dept", r.Department),
                new OracleParameter("email", r.Email),
                new OracleParameter("me", r.AdminId),
                new OracleParameter("type", r.Type),
                new OracleParameter("fy", r.FiscalYear),
                new OracleParameter("asses", r.AssesPno),
                new OracleParameter("lvl", r.AssesLevel),
                new OracleParameter("cycle", r.Cycle)
            };

            ExecuteNonQuery(sql, parameters);
            return id;
        }

        private string GenerateRefNo(string fy)
        {
            string sql = "select MAX(to_number(substr(SS_ID, 3, 10))) from hrps.T_SURVEY_STATUS where SS_ID like 'SR%' and SS_YEAR = :fy";
            var result = ExecuteScalar(sql, new[] { new OracleParameter("fy", fy) });
            int next = (result == DBNull.Value || result == null) ? 1 : Convert.ToInt32(result) + 1;
            return "SR" + next.ToString().PadLeft(10, '0');
        }

        public bool RemoveRespondent(string assesPno, string respondentPno, string category, string fy, string cycle)
        {
            string sql = "delete from hrps.t_survey_status where SS_ASSES_PNO = :asses and SS_PNO = :resp and SS_CATEG = :cat and ss_year = :fy and SS_SRLNO = :cycle and nvl(SS_APP_TAG, 'N') <> 'AP'";
            return ExecuteNonQuery(sql, new[] {
                new OracleParameter("asses", assesPno),
                new OracleParameter("resp", respondentPno),
                new OracleParameter("cat", category),
                new OracleParameter("fy", fy),
                new OracleParameter("cycle", cycle)
            }) > 0;
        }

        public bool SubmitRespondentList(string assesPno, string fy, string cycle, string submittedBy)
        {
            string sql = "update hrps.t_survey_status set SS_TAG='SU', SS_WFL_STATUS='1', SS_UPDATED_DT=sysdate, SS_UPDATED_BY=:me where SS_ASSES_PNO=:asses and ss_year=:fy and SS_SRLNO=:cycle and nvl(SS_DEL_TAG,'N')='N'";
            return ExecuteNonQuery(sql, new[] {
                new OracleParameter("asses", assesPno),
                new OracleParameter("fy", fy),
                new OracleParameter("cycle", cycle),
                new OracleParameter("me", submittedBy)
            }) > 0;
        }

        public bool ApproveRespondentList(string assesPno, string fy, string cycle, string approvedBy)
        {
            string sql = "update hrps.t_survey_status set SS_APP_TAG='AP', SS_WFL_STATUS='2', SS_UPDATED_DT=sysdate, SS_UPDATED_BY=:me where SS_ASSES_PNO=:asses and ss_year=:fy and SS_SRLNO=:cycle and nvl(SS_DEL_TAG,'N')='N'";
            return ExecuteNonQuery(sql, new[] {
                new OracleParameter("asses", assesPno),
                new OracleParameter("fy", fy),
                new OracleParameter("cycle", cycle),
                new OracleParameter("me", approvedBy)
            }) > 0;
        }

        public bool RejectRespondentList(string assesPno, string fy, string cycle, string rejectedBy)
        {
            string sql = "update hrps.t_survey_status set SS_APP_TAG='RJ', SS_WFL_STATUS='0', SS_UPDATED_DT=sysdate, SS_UPDATED_BY=:me where SS_ASSES_PNO=:asses and ss_year=:fy and SS_SRLNO=:cycle and nvl(SS_DEL_TAG,'N')='N'";
            return ExecuteNonQuery(sql, new[] {
                new OracleParameter("asses", assesPno),
                new OracleParameter("fy", fy),
                new OracleParameter("cycle", cycle),
                new OracleParameter("me", rejectedBy)
            }) > 0;
        }
        public IEnumerable<RespondentRecordDTO> GetAutoPopulatedRespondents(string assesPno, string fy, string cycle, string level)
        {
            var list = new List<RespondentRecordDTO>();
            // 1. Managers
            string sqlManager = @"select b.ema_perno, b.ema_ename, b.ema_desgn_desc, b.ema_dept_desc, b.ema_email_id, 'MANGR' categ 
                                  from hrps.t_emp_master_feedback360 a, hrps.t_emp_master_feedback360 b 
                                  where b.ema_perno in (a.EMA_REPORTING_TO_PNO, a.ema_dotted_pno, a.EMA_PERS_EXEC_PNO) 
                                  and a.ema_perno=:pno and a.ema_year=:fy and a.ema_cycle=:cycle";
            
            // 2. Peers
            string sqlPeers = @"select ema_perno, ema_ename, ema_desgn_desc, ema_dept_desc, ema_email_id, 'PEER' categ 
                                from hrps.t_emp_master_feedback360 
                                where ema_reporting_to_pno = (select ema_reporting_to_pno from hrps.t_emp_master_feedback360 where ema_perno=:pno and ema_year=:fy and ema_cycle=:cycle)
                                and ema_perno <> :pno and ema_year=:fy and ema_cycle=:cycle";

            // 3. Subordinates
            string sqlSub = @"select ema_perno, ema_ename, ema_desgn_desc, ema_dept_desc, ema_email_id, 'SUBOR' categ 
                              from hrps.t_emp_master_feedback360 
                              where ema_reporting_to_pno = :pno and ema_year=:fy and ema_cycle=:cycle";

            var parameters = new[] {
                new OracleParameter("pno", assesPno),
                new OracleParameter("fy", fy),
                new OracleParameter("cycle", cycle)
            };

            foreach (var sql in new[] { sqlManager, sqlPeers, sqlSub })
            {
                using (var dr = ExecuteReader(sql, parameters))
                {
                    while (dr.Read())
                    {
                        list.Add(new RespondentRecordDTO
                        {
                            Perno = dr["ema_perno"].ToString(),
                            Name = dr["ema_ename"].ToString(),
                            Designation = dr["ema_desgn_desc"].ToString(),
                            Department = dr["ema_dept_desc"].ToString(),
                            Email = dr["ema_email_id"].ToString(),
                            Category = dr["categ"].ToString(),
                            IsAutoPopulated = true
                        });
                    }
                }
            }
            return list;
        }
    }
}
