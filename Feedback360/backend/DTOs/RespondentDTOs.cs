namespace feedback360.Backend.DTOs
{
    public class AssesseeDTO
    {
        public string Perno { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Level { get; set; }
        public string AppStatus { get; set; }
        public string SubmitStatus { get; set; }
    }

    public class RespondentRecordDTO
    {
        public string Perno { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public string AppStatus { get; set; }
        public string Type { get; set; } // ORG or NORG
        public bool IsAutoPopulated { get; set; }
    }

    public class ValidationRuleDTO
    {
        public string Category { get; set; }
        public string CategoryName { get; set; }
        public int MinCount { get; set; }
        public int MaxCount { get; set; }
    }

    public class RespondentSaveRequestDTO
    {
        public string AssesPno { get; set; }
        public string AssesLevel { get; set; }
        public string Perno { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string AdminId { get; set; }
        public string FiscalYear { get; set; }
        public string Cycle { get; set; }
    }
}
