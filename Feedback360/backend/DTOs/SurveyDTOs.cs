using System;
using System.Collections.Generic;

namespace feedback360.Api.DTOs
{
    public class SurveyPendingRecordDTO
    {
        public string AssesPno { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string Status { get; set; } // Pending, Completed, Rejected
    }

    public class SurveyResponseDTO
    {
        public string AssesPno { get; set; }
        public string RespondentPno { get; set; }
        public string FiscalYear { get; set; }
        public string Cycle { get; set; }

        public string Q1A { get; set; }
        public string Q1B { get; set; }
        public string Q1C { get; set; }
        public string Q1D { get; set; }
        public string Q2A { get; set; }
        public string Q2B { get; set; }
        
        public bool IsInsufficientExposure { get; set; }
    }

    public class SurveyDetailsDTO
    {
        public string AssesPno { get; set; }
        public string AssesName { get; set; }
        public string Q1A { get; set; }
        public string Q1B { get; set; }
        public string Q1C { get; set; }
        public string Q1D { get; set; }
        public string Q2A { get; set; }
        public string Q2B { get; set; }
        public string Status { get; set; }
    }
}
