using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using challenge.Data;

namespace challenge.Repositories
{
    public class CompensationRepository : ICompensationRepository
    {
        private readonly CompensationContext _compensationContext;
        private readonly ILogger<ICompensationRepository> _logger;

        public CompensationRepository(ILogger<ICompensationRepository> logger, CompensationContext compensationContext)
        {
            _compensationContext = compensationContext;
            _logger = logger;
        }
        public Compensation GetCompensationByEmployeeID(string id)
        {
            var compensation = _compensationContext.Compensations.Include(u => u.employees).AsEnumerable()
                .Where(c => c.employees
                .Where(emp => emp.EmployeeId == id).Any()
                ).SingleOrDefault();
            return compensation;
        }
        public Compensation AddCompensation(Compensation compensation)
        {
            _compensationContext.Compensations.Add(compensation);
            return compensation;
        }
        public Task SaveAsync()
        {
            return _compensationContext.SaveChangesAsync();
        }
    }
}
