using System;
using challenge.Repositories;
using challenge.Models;
using Microsoft.Extensions.Logging;

namespace challenge.Services
{
    public class CompensationService : ICompensationService
    {
        private readonly ICompensationRepository _compensationRepository;
        private readonly ILogger<CompensationService> _logger;

        public CompensationService(ILogger<CompensationService> logger, ICompensationRepository compensationRepository)
        {
            _compensationRepository = compensationRepository;
            _logger = logger;
        }
        public Compensation CreateCompensation(Compensation compensation)
        {
            if (compensation.SalaryID != 0)
            {
                _compensationRepository.AddCompensation(compensation);
                _compensationRepository.SaveAsync().Wait();
                return compensation;
            }
            throw new Exception();
        }
        public Compensation GetCompensationByEmployeeID(String ID)
        {
            var compensation = _compensationRepository.GetCompensationByEmployeeID(ID);
            return compensation == null ? new Compensation() : compensation;
        }

    }
}
