namespace feedback360.Backend.DTOs
{
    public class LookupDTO
    {
        public string Id { get; set; }
        public string Text { get; set; }
    }

    public class EmployeeDTO
    {
        public string Year { get; set; }
        public string Cycle { get; set; }
        public string Perno { get; set; }
        public string EmployeeName { get; set; }
        public string DesignationCode { get; set; }
        public string DesignationDesc { get; set; }
        public string Email { get; set; }
        public string EqvLevel { get; set; }
        public string ContactNo { get; set; }
        public string SubAreaCode { get; set; }
        public string SubAreaDesc { get; set; }
        public string ReportingTo { get; set; }
        public string BuhrNo { get; set; }
        public string BuhrName { get; set; }
        public string JoiningDate { get; set; }
        public string DeptCode { get; set; }
        public string DeptDesc { get; set; }
        public string SGrade { get; set; }
        public string EmpClass { get; set; }
        public string DottedPno { get; set; }
        public string PersExecPno { get; set; }
        public string ExecHeadCode { get; set; }
        public string ExecHeadDesc { get; set; }
        public string Step1Start { get; set; }
        public string Step1End { get; set; }
        public string Step2Start { get; set; }
        public string Step2End { get; set; }
        public string Step3Start { get; set; }
        public string Step3End { get; set; }
    }
}
