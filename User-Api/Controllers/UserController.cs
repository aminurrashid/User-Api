using Domain.Enums;
using Domain.Users;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Users;
using System;
using System.Linq;
using User_Api.Models;
using User_Api.Models.Filters;
using User_Api.Validators.User;

namespace User_Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Get users
        /// </summary>
        /// <param name="email">Email</param>
        /// <param name="phone">Phone number</param>
        /// <param name="sortingField">Sort by field name (e.g. Email, FullName etc.)</param>
        /// <param name="sortOrder">Sorting order</param>
        /// <returns>List of users</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /GetUsers?email=nat&#38;phone=513&#38;sortingField=Age&#38;sortOrder=1
        ///
        /// email and phone parameters supports partial match
        /// 
        /// Sorting field names (case-sensitive): FullName, Email, Phone, Age
        /// 
        /// For sort order, use 0 for ascending order (default order) and 1 for descending order
        /// </remarks>
        [HttpGet]
        public IActionResult GetUsers(string email, string phone, string sortingField, int sortOrder)
        {
            try
            {
                UserFilter userFilter = new UserFilter
                {
                    Email = email,
                    Phone = phone,
                    SortingField = sortingField,
                    SortOrder = sortOrder
                };
                FilterValidator validator = new FilterValidator();
                var validationResult = validator.Validate(userFilter);
                if (validationResult.IsValid)
                {
                    var users = _userService.GetUsers(email, phone, sortingField, (SortOrder)sortOrder);

                    if (users != null && users.Count > 0)
                        return Ok(users);
                    else
                    {
                        return NotFound(new ApiErrorResponse { Error = "No user found for the given parameters" });
                    }
                }
                else
                {
                    string errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return UnprocessableEntity(new ApiErrorResponse { Error = errors });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Newly registered user</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Register
        ///     {
        ///        "fullname": "Nathan Romero",
        ///        "email": "nathan@test.com",
        ///        "phone": "513-771-4079",
        ///        "age": 30
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        public IActionResult Register([FromBody] User user)
        {
            try
            {
                UserValidator validator = new UserValidator();
                var validationResult = validator.Validate(user);
                if (validationResult.IsValid)
                {
                    var createdUser = _userService.Register(user);
                    if (createdUser != null)
                        return Created(string.Empty, createdUser);
                    else
                    {
                        var existingUsers = _userService.GetUsers(user.Email);
                        if (existingUsers != null && existingUsers.Count > 0)
                            return UnprocessableEntity(new ApiErrorResponse { Error = "User already exists with this email" });
                        else
                            return BadRequest(new ApiErrorResponse { Error = "Error" });
                    }
                }
                else
                {
                    string errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return UnprocessableEntity(new ApiErrorResponse { Error = errors });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Update user by email
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Updated user</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Update
        ///     {
        ///        "fullname": "Nathan Romero",
        ///        "email": "nathan@test.com",
        ///        "phone": "513-771-4040",
        ///        "age": 40
        ///     }
        ///
        /// </remarks>
        [HttpPut]
        public IActionResult Update([FromBody] User user)
        {
            try
            {
                UserValidator validator = new UserValidator();
                var validationResult = validator.Validate(user);
                if (validationResult.IsValid)
                {
                    var updatedUser = _userService.Update(user);
                    if (updatedUser != null)
                        return Ok(updatedUser);
                    else
                    {
                        var existingUsers = _userService.GetUsers(user.Email);
                        if (existingUsers == null || existingUsers.Count == 0)
                            return UnprocessableEntity(new ApiErrorResponse { Error = "No user exists with this email" });
                        else
                            return BadRequest(new ApiErrorResponse { Error = "Error" });
                    }
                }
                else
                {
                    string errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return UnprocessableEntity(new ApiErrorResponse { Error = errors });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Delete user by email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>Success/Error confirmation</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /Delete?email=nathan@test.com
        ///
        /// </remarks>
        [HttpDelete]
        public IActionResult Delete(string email)
        {
            try
            {
                EmailValidator validator = new EmailValidator();
                var validationResult = validator.Validate(email);
                if (validationResult.IsValid)
                {
                    bool success = _userService.DeleteByEmail(email);
                    if (success)
                        return Ok("User deleted successfully");
                    else
                    {
                        var existingUsers = _userService.GetUsers(email);
                        if (existingUsers == null || existingUsers.Count == 0)
                            return UnprocessableEntity(new ApiErrorResponse { Error = "No user exists with this email" });
                        else
                            return BadRequest(new ApiErrorResponse { Error = "Error" });
                    }
                }
                else
                {
                    string errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
                    return UnprocessableEntity(new ApiErrorResponse { Error = errors });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
