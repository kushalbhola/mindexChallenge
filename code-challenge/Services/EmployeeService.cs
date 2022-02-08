using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using challenge.Repositories;

namespace challenge.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public Employee Create(Employee employee)
        {
            if(employee != null)
            {
                _employeeRepository.Add(employee);
                _employeeRepository.SaveAsync().Wait();
            }

            return employee;
        }

        public Employee GetById(string id)
        {
            if(!String.IsNullOrEmpty(id))
            {
                return _employeeRepository.GetById(id);
            }

            return null;
        }

        public Employee Replace(Employee originalEmployee, Employee newEmployee)
        {
            if(originalEmployee != null)
            {
                _employeeRepository.Remove(originalEmployee);
                if (newEmployee != null)
                {
                    // ensure the original has been removed, otherwise EF will complain another entity w/ same id already exists
                    _employeeRepository.SaveAsync().Wait();

                    _employeeRepository.Add(newEmployee);
                    // overwrite the new id with previous employee id
                    newEmployee.EmployeeId = originalEmployee.EmployeeId;
                }
                _employeeRepository.SaveAsync().Wait();
            }

            return newEmployee;
        }

        /// <summary>
        /// this method return all the direct and indirect reportee under an empoyee. 
        /// </summary>
        /// <returns>total number of reports</returns>
        public ReportingStructure GetEmployeeReportingStructureById(string id)
        {
            var employee = GetById(id);
            ReportingStructure reportingStructure = new ReportingStructure();
            if (employee != null)
            {
                reportingStructure.Employee = employee;
                reportingStructure.NumberOfReports = GetEmployeeReportingStructureByIdHelper(employee);
            }
            return reportingStructure;
        }
        /// <summary>
        /// Helper method uses depth first search to recursively find all the employees who directly or indirectly report to the employee passed in 
        /// </summary>
        /// <returns>total number of reports</returns>
        int GetEmployeeReportingStructureByIdHelper(Employee employee)
        {
            Stack<Employee> employeeStack = new Stack<Employee>();
            employeeStack.Push(employee);
            var count = 0;
            while (employeeStack.Count != 0)
            {
                Employee currentEmp = employeeStack.Pop();
                if (currentEmp != null && (currentEmp.DirectReports != null))
                {
                    foreach (Employee directReport in currentEmp.DirectReports)
                    {
                        if(directReport.EmployeeId != employee.EmployeeId)
                        {
                            count++;
                            employeeStack.Push(directReport);
                        }
                    }
                }
            }
            return count;
        }
    }
}
