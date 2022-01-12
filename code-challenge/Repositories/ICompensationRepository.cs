using challenge.Models;
using System;
using System.Threading.Tasks;

namespace challenge.Repositories
{
    public interface ICompensationRepository
    {
        Compensation GetCompensationByEmployeeID(String id);
        Compensation AddCompensation(Compensation compensation);
        Task SaveAsync();
    }
}