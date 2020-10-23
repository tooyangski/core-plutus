using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectPlutus.API.Filters;
using ProjectPlutus.API.ViewModels;
using ProjectPlutus.Domain.Models;
using ProjectPlutus.Extensions.Models;
using ProjectPlutus.Infra;
using System;
using System.Threading.Tasks;

namespace ProjectPlutus.API.Controllers
{
    [ApiController]
    [Route("api/v1/employees")]
    [EmployeeResultFilterAttribute]
    public class EmployeesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public EmployeesController(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<EmployeesController> logger)
        {
            _unitOfWork = unitOfWork ??
                throw new ArgumentNullException(nameof(unitOfWork));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployeesAsync(
            string firstName,
            string lastName,
            string createdAtStart,
            string createdAtEnd,
            string temperatureStart,
            string temperatureEnd)
        {
            if (firstName != null || lastName != null)
            {
                _logger.LogInformation(
                    LoggingEvents.GetItem,
                    "Getting all with firstname: {FIRSTNAME} OR lastname: {LASTNAME}", firstName, lastName);

                var empWithNames =
                    await _unitOfWork
                        .EmployeeRepository
                        .FindAsync(e => e.FirstName == firstName || e.LastName == lastName);

                return Ok(empWithNames);
            }
            else if (createdAtStart != null && createdAtEnd != null)
            {
                _logger.LogInformation(
                    LoggingEvents.GetItem,
                    "Getting all with crated range of: {START} AND createdAtEnd: {END}", createdAtStart, createdAtEnd);

                var start = DateTimeOffset.Parse(createdAtStart);
                var end = DateTimeOffset.Parse(createdAtEnd);

                var empCreatedRange =
                    await _unitOfWork
                        .EmployeeRepository
                        .FindAsync(e => e.CreatedAt >= start && e.CreatedAt <= end);

                return Ok(empCreatedRange);
            }
            else if (temperatureStart != null && temperatureEnd != null)
            {
                _logger.LogInformation(
                    LoggingEvents.GetItem,
                    "Getting all with temparature range of: {START} AND createdAtEnd: {END}", temperatureStart, temperatureEnd);

                var start = Convert.ToDouble(temperatureStart);
                var end = Convert.ToDouble(temperatureEnd);

                var empTempRange =
                    await _unitOfWork
                        .EmployeeRepository
                        .FindAsync(e => e.Temperature >= start && e.Temperature <= end);

                return Ok(empTempRange);
            }

            _logger.LogInformation(
                    LoggingEvents.GetItem,
                    "Getting all employees");

            var employees = await _unitOfWork.EmployeeRepository.GetAllAsync();

            return Ok(employees);

        }

        [HttpGet]
        [Route("{id}", Name = "GetEmployee")]
        public async Task<IActionResult> GetEmployeeAsync(int id)
        {
            _logger.LogInformation(
               LoggingEvents.GetItem,
               "Getting the employee with ID: {ID}", id);

            var employee = await _unitOfWork.EmployeeRepository.GetAsync(id);

            if (employee == null)
                return NotFound();
            else
                return Ok(employee);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateEmployeeAsync(
            EmployeeModificationViewModel employeeModificationViewModel)
        {
            var employee = _mapper.Map<Employee>(employeeModificationViewModel);

            _logger.LogInformation(
                LoggingEvents.PatchItem,
                "Create record of a employee with ID of: {ID}",
                employee.Id);

            _unitOfWork.EmployeeRepository.Update(employee);

            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }


        [HttpPost]
        public async Task<IActionResult> CreateEmployeeAsync(
            EmployeeCreationViewModel employeeCreationViewModel)
        {
            var employee = _mapper.Map<Employee>(employeeCreationViewModel);

            _unitOfWork.EmployeeRepository.Add(employee);

            _logger.LogInformation(
                LoggingEvents.PostItem,
                "Update record of a employee with ID of: {ID}",
                employee.Id);

            await _unitOfWork.SaveChangesAsync();

            //query again in case you want to include other details from the context,
            //this is just for future use.
            await _unitOfWork.EmployeeRepository.GetAsync(employee.Id);

            return CreatedAtRoute("GetEmployee", new { id = employee.Id }, employee);
        }
    }
}
