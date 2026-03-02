using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using feedback360.Backend.DTOs;

namespace feedback360.Backend.Repositories
{
    public interface IReportRepository
    {
        DataTable GetDetailedStatusReport(string year, string cycle, string pno = null);
        DataTable GetSummaryCompletionReport(string year, string cycle);
        DataTable GetRawSurveyData(string assesPno, string year);
    }

    public class ReportRepository : BaseRepository, IReportRepository
    {
        public DataTable GetDetailedStatusReport(string year, string cycle, string pno = null)
        {
            string sql = @"SELECT ss_pno feedback_giver_pno, ss_name feedback_giver_name, ss_email feedback_giver_email,  ss_level feedback_giver_level, ss_desg
                           feedback_giver_designation, DECODE(ss_wfl_status,'','Pending With Assesor','1', 'Pending With Manager','2','Pending With Respondent'
                           ,'3','Completed','9','Insufficient Exposure' ) status, irc_desc category, ss_asses_pno assesor_pno, a.ema_ename assesor_name,
                           a.ema_email_id assesor_email, a.ema_desgn_desc assesor_designation, a.ema_exec_head_desc assesor_executive_head, 
                           DECODE(ss_del_tag,'Y','DELETED','N','SELECTED',ss_del_tag) adm_del_tag, DECODE(ss_app_tag,'AP','Approved','RJ','Returned to officer',ss_app_tag)
                           approval_status FROM hrps.t_emp_master_feedback360 a, hrps.t_survey_status, hrps.t_ir_codes WHERE ss_asses_pno = a.ema_perno AND irc_type = '360RL' 
                           AND upper(irc_code) = upper(ss_categ) AND ss_year = :yr AND ss_srlno = :cyc AND ss_status = 'SE' AND ss_del_tag = 'N' AND a.ema_comp_code='1000'";

            var paras = new List<OracleParameter> {
                new OracleParameter("yr", year),
                new OracleParameter("cyc", cycle)
            };

            if (!string.IsNullOrEmpty(pno))
            {
                sql += " AND ss_asses_pno = :pno";
                paras.Add(new OracleParameter("pno", pno));
            }

            sql += " ORDER BY ss_asses_pno, ss_categ";
            return ExecuteQuery(sql, paras.ToArray());
        }

        public DataTable GetSummaryCompletionReport(string year, string cycle)
        {
            string sql = @"select A.ss_year, A.ss_asses_pno, decode(A.ss_categ,'INTSH','Internal Stakeholder','MANGR','Manager/Superior',
                           'PEER','Peer','ROPT','Subordinates','Self','Self') CATEGORY,
                           decode(a.ss_categ,'INTSH',3,'MANGR',1,'PEER',3,'ROPT',3,'Self',1) MIN_REQD, a.approved APPROVED_COUNT, 
                           nvl(c.completed,0) COMPLETED_COUNT, NVL(b.rejected,0) INSUFFICIENT_EXPOSURE,
                           decode(sign(a.approved-b.rejected-decode(a.ss_categ,'INTSH',3,'MANGR',1,'PEER',3,'ROPT',3)),'-1','LESS','OK') CRITERIA 
                           from (select ss_year, ss_asses_pno, ss_categ, count(*) approved from hrps.t_survey_status 
                                 where ss_year=:yr and ss_srlno=:cyc and ss_status='SE' and ss_del_tag='N' and ss_app_tag='AP' group by ss_year, ss_asses_pno, ss_categ) a, 
                                (select ss_year, ss_asses_pno, ss_categ, count(*) rejected from hrps.t_survey_status 
                                 where ss_year=:yr and ss_srlno=:cyc and ss_status='SE' and ss_del_tag='N' and ss_app_tag='AP' and nvl(ss_wfl_status,'0') ='9' group by ss_year, ss_asses_pno, ss_categ) b,
                                (select ss_year, ss_asses_pno, ss_categ, count(*) completed from hrps.t_survey_status 
                                 where ss_year=:yr and ss_srlno=:cyc and ss_status='SE' and ss_del_tag='N' and ss_app_tag='AP' and nvl(ss_wfl_status,'0')='3' group by ss_year, ss_asses_pno, ss_categ) c 
                           where a.ss_year=b.ss_year(+) and a.ss_asses_pno=b.ss_asses_pno(+) and a.ss_categ=b.ss_categ(+) 
                           and a.ss_year=c.ss_year(+) and a.ss_asses_pno=c.ss_asses_pno(+) and a.ss_categ=c.ss_categ(+) order by 1,2,3";

            return ExecuteQuery(sql, new[] { 
                new OracleParameter("yr", year),
                new OracleParameter("cyc", cycle)
            });
        }

        public DataTable GetRawSurveyData(string assesPno, string year)
        {
            string sql = "select * from hrps.t_survey_status where SS_ASSES_PNO=:pno and SS_YEAR=:yr";
            return ExecuteQuery(sql, new[] { 
                new OracleParameter("pno", assesPno),
                new OracleParameter("yr", year)
            });
        }
        public IEnumerable<IndividualScoreDTO> GetIndividualScores(string perno, string year)
        {
            string sql = @"
                select 
                    decode(upper(ss_categ), 'SELF', 'Self', 'MANGR', 'Manager', 'PEER', 'Peers', 'INTSH', 'Internal Stakeholders', 'ROPT', 'People you lead') Category,
                    round(avg(ss_q1_a), 2) Accountability,
                    round(avg(ss_q1_b), 2) Collaboration,
                    round(avg(ss_q1_c), 2) Responsiveness,
                    round(avg(ss_q1_d), 2) PeopleDevelopment,
                    count(*) Count,
                    upper(ss_categ) CategKey
                from hrps.t_survey_status 
                where ss_asses_pno = :perno and ss_year = :year and ss_wfl_status = '3' and ss_del_tag = 'N' and ss_app_tag = 'AP'
                group by ss_categ";

            return Query<IndividualScoreDTO>(sql, new { perno, year });
        }

        public dynamic GetIndividualComments(string perno, string year)
        {
            string sqlSelf = "select ss_q2_a from hrps.t_survey_status where ss_asses_pno=:perno and ss_year=:year and upper(ss_categ)='SELF' and ss_wfl_status='3'";
            string sqlOthers = "select ss_q2_a from hrps.t_survey_status where ss_asses_pno=:perno and ss_year=:year and upper(ss_categ)<>'SELF' and ss_wfl_status='3'";
            string sqlSelfB = "select ss_q2_b from hrps.t_survey_status where ss_asses_pno=:perno and ss_year=:year and upper(ss_categ)='SELF' and ss_wfl_status='3'";
            string sqlOthersB = "select ss_q2_b from hrps.t_survey_status where ss_asses_pno=:perno and ss_year=:year and upper(ss_categ)<>'SELF' and ss_wfl_status='3'";

            return new {
                SelfStrengths = Query<string>(sqlSelf, new { perno, year }),
                OtherStrengths = Query<string>(sqlOthers, new { perno, year }),
                SelfDevelop = Query<string>(sqlSelfB, new { perno, year }),
                OtherDevelop = Query<string>(sqlOthersB, new { perno, year })
            };
        }
    }

    public class IndividualScoreDTO
    {
        public string Category { get; set; }
        public double Accountability { get; set; }
        public double Collaboration { get; set; }
        public double Responsiveness { get; set; }
        public double PeopleDevelopment { get; set; }
        public int Count { get; set; }
        public string CategKey { get; set; }
    }
}
