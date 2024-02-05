using System.ComponentModel.DataAnnotations;

namespace ARMSystem.Model
{
    public class Employee
    {

        [Key]
        public int SrNo { get; set; }
        public string EmpAdId { get; set; }   //  EmpAdId Unique 

        public string EmployeeName { get; set; }

        public string BusinessUnit { get; set; }
        public string CorporateDesignation { get; set; }
        public string FunctionalDesignation { get; set; }

        public bool IsActive { get; set; }

        public string Type { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } = "System";
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; } = "";





        public Employee DeepCopy()
        {
            return new Employee
            {
                // Copy all the relevant fields
                EmpAdId = this.EmpAdId,
                EmployeeName = this.EmployeeName,
                BusinessUnit = this.BusinessUnit,
                CorporateDesignation = this.CorporateDesignation,
                FunctionalDesignation = this.FunctionalDesignation,
                IsActive = this.IsActive,
                Type = this.Type,
                CreatedOn = this.CreatedOn,
                CreatedBy = this.CreatedBy,
                // Do not copy SrNo as it should be unique for each record
            };
        }



    }
}
