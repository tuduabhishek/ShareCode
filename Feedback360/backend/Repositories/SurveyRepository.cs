using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using feedback360.Api.DTOs;

namespace feedback360.Api.Repositories
{
    public class SurveyRepository : BaseRepository
    {
        public IEnumerable<SurveyPendingRecordDTO> GetPendingSurveys(string respondentId, string fy, string cycle)
        {
            var list = new List<SurveyPendingRecordDTO>();
            // Logic based on Feedback.aspx.vb bindPendingRecord
            // It checks T_SURVEY_STATUS where SS_PNO = respondentId AND SS_YEAR = fy AND SS_APP_TAG = 'AP'
            // and joins with tips.t_empl_all to get assessee details.

            string sql = @"select ss_asses_pno, ema_ename, ema_desgn_desc, ema_dept_desc, 
                           decode(ss_wfl_status,'2','Pending','3','Completed','9','Insufficient exposure','Pending') status 
                           from hrps.t_survey_status, tips.t_empl_all 
                           where ss_asses_pno=ema_perno and ss_app_tag='AP' and ss_year=:fy and ss_srlno=:cycle 
                           and (trim(upper(ss_pno))=:me or trim(upper(ss_email || ss_intsh_otp))=:me)
                           and nvl(ss_del_tag, 'N') = 'N'";

            var parameters = new[] {
                new OracleParameter("fy", fy),
                new OracleParameter("cycle", cycle),
                new OracleParameter("me", respondentId.ToUpper())
            };

            using (var dr = ExecuteReader(sql, parameters))
            {
                while (dr.Read())
                {
                    list.Add(new SurveyPendingRecordDTO
                    {
                        AssesPno = dr["ss_asses_pno"].ToString(),
                        Name = dr["ema_ename"].ToString(),
                        Designation = dr["ema_desgn_desc"].ToString(),
                        Department = dr["ema_dept_desc"].ToString(),
                        Status = dr["status"].ToString()
                    });
                }
            }
            return list;
        }

        public SurveyDetailsDTO GetSurveyDetails(string respondentId, string assesPno, string fy, string cycle)
        {
            string sql = @"select SS_Q1_A, SS_Q1_B, SS_Q1_C, SS_Q1_D, SS_Q2_A, SS_Q2_B, SS_WFL_STATUS, ema_ename
                           from hrps.t_survey_status, tips.t_empl_all
                           where ss_asses_pno=ema_perno and ss_asses_pno=:asses and ss_year=:fy and ss_srlno=:cycle 
                           and (trim(upper(ss_pno))=:me or trim(upper(ss_email || ss_intsh_otp))=:me)";

            var parameters = new[] {
                new OracleParameter("me", respondentId.ToUpper()),
                new OracleParameter("asses", assesPno),
                new OracleParameter("fy", fy),
                new OracleParameter("cycle", cycle)
            };

            var dt = GetDataTable(sql, parameters);
            if (dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];
                return new SurveyDetailsDTO
                {
                    AssesPno = assesPno,
                    AssesName = row["ema_ename"].ToString(),
                    Q1A = row["SS_Q1_A"].ToString(),
                    Q1B = row["SS_Q1_B"].ToString(),
                    Q1C = row["SS_Q1_C"].ToString(),
                    Q1D = row["SS_Q1_D"].ToString(),
                    Q2A = row["SS_Q2_A"].ToString(),
                    Q2B = row["SS_Q2_B"].ToString(),
                    Status = row["SS_WFL_STATUS"].ToString()
                };
            }
            return null;
        }

        public bool SubmitFeedback(SurveyResponseDTO res)
        {
            string sql = @"update hrps.t_survey_status set 
                           SS_Q1_A=:q1a, SS_Q1_B=:q1b, SS_Q1_C=:q1c, SS_Q1_D=:q1d, 
                           SS_Q2_A=:q2a, SS_Q2_B=:q2b, SS_WFL_STATUS='3', 
                           SS_FEEDBACK_DT=sysdate, SS_UPDATED_DT=sysdate, SS_UPDATED_BY=:me 
                           where ss_asses_pno=:asses and ss_year=:fy and ss_srlno=:cycle 
                           and (trim(upper(ss_pno))=:me or trim(upper(ss_email || ss_intsh_otp))=:me)";

            var parameters = new[] {
                new OracleParameter("q1a", res.Q1A),
                new OracleParameter("q1b", res.Q1B),
                new OracleParameter("q1c", res.Q1C),
                new OracleParameter("q1d", res.Q1D),
                new OracleParameter("q2a", res.Q2A),
                new OracleParameter("q2b", res.Q2B),
                new OracleParameter("me", res.RespondentPno.ToUpper()),
                new OracleParameter("asses", res.AssesPno),
                new OracleParameter("fy", res.FiscalYear),
                new OracleParameter("cycle", res.Cycle)
            };

            return ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool RejectFeedback(string respondentId, string assesPno, string fy, string cycle)
        {
            string sql = @"update hrps.t_survey_status set SS_WFL_STATUS='9', 
                           SS_FEEDBACK_DT=sysdate, SS_UPDATED_BY=:me, SS_UPDATED_DT=sysdate
                           where ss_asses_pno=:asses and ss_year=:fy and ss_srlno=:cycle 
                           and (trim(upper(ss_pno))=:me or trim(upper(ss_email || ss_intsh_otp))=:me)";

            var parameters = new[] {
                new OracleParameter("me", respondentId.ToUpper()),
                new OracleParameter("asses", assesPno),
                new OracleParameter("fy", fy),
                new OracleParameter("cycle", cycle)
            };

            return ExecuteNonQuery(sql, parameters) > 0;
        }
        public bool VerifyOTP(string id, string otp, out string assesPno, out string fy, out string cycle)
        {
            assesPno = fy = cycle = "";
            string sql = @"select ss_asses_pno, ss_year, ss_srlno from hrps.t_survey_status 
                           where ss_id=:id and ss_intsh_otp=:otp and nvl(ss_del_tag,'N')='N'";
            
            var dt = GetDataTable(sql, new[] {
                new OracleParameter("id", id),
                new OracleParameter("otp", otp)
            });

            if (dt.Rows.Count > 0)
            {
                assesPno = dt.Rows[0]["ss_asses_pno"].ToString();
                fy = dt.Rows[0]["ss_year"].ToString();
                cycle = dt.Rows[0]["ss_srlno"].ToString();
                return true;
            }
            return false;
        }
    }
}
