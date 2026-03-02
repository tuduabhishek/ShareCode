namespace feedback360.Backend.DTOs
{
    public class FeedbackRecordDTO
    {
        public string AssesPno { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string Status { get; set; }
    }

    public class FeedbackSubmitRequestDTO
    {
        public string UserId { get; set; }
        public string AssesPno { get; set; }
        public string FiscalYear { get; set; }
        public string Q1A { get; set; }
        public string Q1B { get; set; }
        public string Q1C { get; set; }
        public string Q1D { get; set; }
        public string Q2A { get; set; }
        public string Q2B { get; set; }
    }

    public class FeedbackRejectRequestDTO
    {
        public string UserId { get; set; }
        public string AssesPno { get; set; }
        public string FiscalYear { get; set; }
    }
}
