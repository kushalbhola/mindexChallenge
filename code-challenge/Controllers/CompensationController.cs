using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using challenge.Services;
using challenge.Models;

namespace code_challenge.Controllers
{
    [Route("api/compensation")]
    public class CompensationController : Controller
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;

        public CompensationController(ILogger<CompensationController> logger, ICompensationService compensationService)
        {
            _logger = logger;
            _compensationService = compensationService;
        }

        /// <summary>
        /// This method will create a new compensation record for en employee
        /// </summary>
        /// <param name="compensation">Compensation record</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateCompensation([FromBody] Compensation compensation)
        {
            _logger.LogDebug($"Compensation create request");
            try
            {
                _compensationService.CreateCompensation(compensation);
                return CreatedAtRoute("getCompensationById", new { id = compensation.employees.Select(e => e.EmployeeId.ToString()) }, compensation);
            }
            catch(Exception exception)
            {
                _logger.LogError(exception.Message, exception);
                compensation.IsError = true;
                compensation.ErrorMessage = "There was an error saving the compensation record:  " + exception.Message;
                return BadRequest(compensation);
            }
        }

        /// <summary>
        /// Get a single compensation record
        /// </summary>
        /// <param name="id">The id of the compensation being listed</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "getCompensationById")]
        public IActionResult GetCompensationById(String id)
        {
            _logger.LogDebug($"GetCompensationById received for id: '{id}'");
            var compensation = new Compensation();
            try
            {
                compensation = _compensationService.GetCompensationByEmployeeID(id);
                if (compensation.SalaryID == 0)
                    throw new Exception();
                return Ok(compensation);
            }
            catch(Exception exception)
            {
                _logger.LogError(exception.Message, exception);
                compensation.IsError = true;
                compensation.ErrorMessage = "There was an error processing request for employeeID: " + id + " . Exception:  " + exception.Message;
                return NotFound(compensation);
            }
            
            


        }
    }
}