using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using challenge.Services;
using challenge.Models;

namespace challenge.Controllers
{
    [Route("api/reporting")]
    public class ReportingStructureController : Controller
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;
        public ReportingStructureController(ILogger<ReportingStructureController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        [HttpGet("{id}", Name = "getEmployeeReportingStructureById")]
        public IActionResult GetEmployeeReportingStructureById(String id)
        {
            _logger.LogDebug($"Reporting structure request received for employee ID:  '{id}'");
            var reportingStructure = new ReportingStructure();
            try
            {
                reportingStructure = _employeeService.GetEmployeeReportingStructureById(id);
                if (reportingStructure.Employee == null)
                    throw new Exception();
                else
                    return Ok(reportingStructure);
            }
            catch(Exception exception)
            {
                _logger.LogError(exception.Message, exception);
                reportingStructure.IsError = true;
                reportingStructure.ErrorMessage = "There was an error processing request for employeeID: " + id + " . Exception:  " + exception.Message;
                return NotFound(reportingStructure);
            }
        }
    }
}
