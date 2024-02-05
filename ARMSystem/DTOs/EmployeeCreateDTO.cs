namespace ARMSystem.DTOs
{
    public class EmployeeCreateDTO
    {
        public string EmpAdId { get; set; }
        public string EmployeeName { get; set; }
        public string BusinessUnit { get; set; }
        public string CorporateDesignation { get; set; }
        public string FunctionalDesignation { get; set; }
        public bool IsActive { get; set; }
        public string Type { get; set; }
    }
}
