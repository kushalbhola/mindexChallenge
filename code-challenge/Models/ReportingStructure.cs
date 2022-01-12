
namespace challenge.Models
{
    public class ReportingStructure : BaseModel
    {
        public Employee Employee { get; set; }
        public int NumberOfReports { get; set; }
    }
}
