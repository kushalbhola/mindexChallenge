using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace challenge.Models
{
    public class Compensation: BaseModel
    {
        [Key]
        public int SalaryID { get; set; }
        //Multiple employees can have the same salary
        public List<Employee> employees { get; set; }
        public Decimal Salary { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
