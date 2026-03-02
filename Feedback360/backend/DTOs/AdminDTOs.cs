using System.Collections.Generic;

namespace feedback360.Backend.DTOs
{
    public class ExcludedEmployeeDTO
    {
        public string Year { get; set; }
        public string Cycle { get; set; }
        public string Perno { get; set; }
        public string EmployeeName { get; set; }
        public string Email { get; set; }
        public string DeptDesc { get; set; }
        public string SGrade { get; set; }
        public string EqvLevel { get; set; }
        public string CompCode { get; set; }
    }

    public class SuperAdminDTO
    {
        public string Code { get; set; }
        public string UserId { get; set; }
        public string GroupId { get; set; }
    }
    
    public class BulkUpdateDTO
    {
        public string Year { get; set; }
        public string Cycle { get; set; }
        public string Perno { get; set; }
        public string NewValue { get; set; }
        public string FieldType { get; set; } // e.g., "EQV", "DEPT", etc.
    }
}
