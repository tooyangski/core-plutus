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
    [Route("api/v1/users")]
    [UserResultFilterAttribute]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public UsersController(
           IUnitOfWork unitOfWork,
           IMapper mapper,
           ILogger<UsersController> logger)
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
            string createdAtEnd)
        {
            if (firstName != null || lastName != null)
            {
                _logger.LogInformation(
                    LoggingEvents.GetItem,
                    "Getting all with firstname: {FIRSTNAME} OR lastname: {LASTNAME}", firstName, lastName);

                var userWithNames =
                    await _unitOfWork
                        .UserRepository
                        .FindAsync(e => e.FirstName == firstName || e.LastName == lastName);

                return Ok(userWithNames);
            }
            else if (createdAtStart != null && createdAtEnd != null)
            {
                _logger.LogInformation(
                    LoggingEvents.GetItem,
                    "Getting all with created range of: {START} AND createdAtEnd: {END}", createdAtStart, createdAtEnd);

                var start = DateTimeOffset.Parse(createdAtStart);
                var end = DateTimeOffset.Parse(createdAtEnd);

                var userCreatedRange =
                    await _unitOfWork
                        .UserRepository
                        .FindAsync(e => e.CreatedAt >= start && e.CreatedAt <= end);

                return Ok(userCreatedRange);
            }

            _logger.LogInformation(
                    LoggingEvents.GetItem,
                    "Getting all users");

            var users = await _unitOfWork.UserRepository.GetAllAsync();

            return Ok(users);

        }



        [HttpGet]
        [Route("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUserAsync(int id)
        {
            _logger.LogInformation(
               LoggingEvents.GetItem,
               "Getting the User with ID: {ID}", id);

            var user = await _unitOfWork.UserRepository.GetAsync(id);

            if (user == null)
                return NotFound();
            else
                return Ok(user);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateUserAsync(
            UserModificationViewModel userModificationViewModel)
        {
            var user = _mapper.Map<User>(userModificationViewModel);

            if (user.Id == 0)
                return NotFound();
            else
            {
                _logger.LogInformation(
                    LoggingEvents.PatchItem,
                    "Update record of a user with ID of: {ID}",
                    user.Id);

                _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.SaveChangesAsync();
                return NoContent();
            }
        }

        [HttpDelete]
        [Route("{id}", Name = "DeleteUser")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            _logger.LogInformation(
              LoggingEvents.DeleteItem,
              "Deleting the User with ID: {ID}", id);

            var deleted = await _unitOfWork.UserRepository.DeleteAsync(id);

            if (deleted)
                return Ok();
            else
                return NotFound();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserLoginViewModel userLoginViewModel)
        {
            if (userLoginViewModel != null)
            {
                _logger.LogInformation(
                    LoggingEvents.GetItem,
                    "Getting user login of: {EMAIL}", userLoginViewModel.Email);

                var userWithNames =
                    await _unitOfWork
                        .UserRepository
                        .FindAsync(e => e.Email == userLoginViewModel.Email && e.Password == userLoginViewModel.Password);

                if (userWithNames != null)
                    return Ok(true);
            }
            return Ok(false);
        }

        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> CreateUserAsync(
            UserCreationViewModel userCreationViewModel)
        {
            var user = _mapper.Map<User>(userCreationViewModel);

            _unitOfWork.UserRepository.Add(user);

            _logger.LogInformation(
                LoggingEvents.PostItem,
                "Update record of a user with ID of: {ID}",
                user.Id);

            await _unitOfWork.SaveChangesAsync();

            //query again in case you want to include other details from the context,
            //this is just for future use.
            await _unitOfWork.UserRepository.GetAsync(user.Id);

            return CreatedAtRoute("GetUser", new { id = user.Id }, user);
        }
    }
}
