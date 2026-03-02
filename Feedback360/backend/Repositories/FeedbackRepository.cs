using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using feedback360.Backend.DTOs;

namespace feedback360.Backend.Repositories
{
    public interface IFeedbackRepository
    {
        IEnumerable<FeedbackRecordDTO> GetPendingRecords(string userId, string fy);
        bool SubmitFeedback(FeedbackSubmitRequestDTO request);
        bool RejectFeedback(FeedbackRejectRequestDTO request);
    }

    public class FeedbackRepository : BaseRepository, IFeedbackRepository
    {
        public IEnumerable<FeedbackRecordDTO> GetPendingRecords(string userId, string fy)
        {
            var records = new List<FeedbackRecordDTO>();
            string sql = @"select ss_asses_pno, ema_ename, ema_desgn_desc, ema_dept_desc, 
                           decode(ss_wfl_status,'2','Pending','3', 'Completed', '9', 'Insufficient exposure to provide feedback') as status 
                           from tips.t_empl_all, t_survey_status 
                           where ss_asses_pno=ema_perno and ss_app_tag='AP' and SS_YEAR = :fy 
                           and EMA_EQV_LEVEL='I1' and SS_PNO = :userId";
            
            var parameters = new[] { 
                new OracleParameter("fy", fy),
                new OracleParameter("userId", userId)
            };

            DataTable dt = ExecuteQuery(sql, parameters);
            foreach (DataRow row in dt.Rows)
            {
                records.Add(new FeedbackRecordDTO {
                    AssesPno = row["ss_asses_pno"].ToString(),
                    EmployeeName = row["ema_ename"].ToString(),
                    Designation = row["ema_desgn_desc"].ToString(),
                    Department = row["ema_dept_desc"].ToString(),
                    Status = row["status"].ToString()
                });
            }
            return records;
        }

        public bool SubmitFeedback(FeedbackSubmitRequestDTO request)
        {
            string sql = @"update T_SURVEY_STATUS set SS_Q1_A=:Q1A, SS_Q1_B=:Q1B, SS_Q1_C=:Q1C, SS_Q1_D=:Q1D, 
                           SS_Q2_A=:Q2A, SS_Q2_B=:Q2B, SS_WFL_STATUS='3', SS_FEEDBACK_DT = sysdate, 
                           SS_UPDATED_DT=sysdate, SS_UPDATED_BY=:updatedBy 
                           WHERE SS_PNO=:userId AND SS_ASSES_PNO=:assesPno AND SS_YEAR=:fy";
            
            var parameters = new[] {
                new OracleParameter("Q1A", request.Q1A),
                new OracleParameter("Q1B", request.Q1B),
                new OracleParameter("Q1C", request.Q1C),
                new OracleParameter("Q1D", request.Q1D),
                new OracleParameter("Q2A", request.Q2A),
                new OracleParameter("Q2B", request.Q2B),
                new OracleParameter("updatedBy", request.UserId),
                new OracleParameter("userId", request.UserId),
                new OracleParameter("assesPno", request.AssesPno),
                new OracleParameter("fy", request.FiscalYear)
            };

            return ExecuteNonQuery(sql, parameters) > 0;
        }

        public bool RejectFeedback(FeedbackRejectRequestDTO request)
        {
            string sql = @"update T_SURVEY_STATUS set SS_WFL_STATUS='9', SS_FEEDBACK_DT = sysdate, 
                           SS_UPDATED_BY=:updatedBy, SS_UPDATED_DT=sysdate 
                           WHERE SS_PNO=:userId AND SS_ASSES_PNO=:assesPno AND SS_YEAR=:fy";
            
            var parameters = new[] {
                new OracleParameter("updatedBy", request.UserId),
                new OracleParameter("userId", request.UserId),
                new OracleParameter("assesPno", request.AssesPno),
                new OracleParameter("fy", request.FiscalYear)
            };

            return ExecuteNonQuery(sql, parameters) > 0;
        }
    }
}
