using challenge.Models;
using System;

namespace challenge.Services
{
    public interface ICompensationService
    {
        Compensation GetCompensationByEmployeeID(String id);
        Compensation CreateCompensation(Compensation employee);
    }
}
