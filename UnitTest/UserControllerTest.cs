using Domain.Enums;
using Domain.Users;
using Infrastructure.Repositories.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Service.Users;
using System;
using System.Collections.Generic;
using User_Api.Controllers;
using Xunit;

namespace UnitTest
{
    public class UserControllerTest
    {
        private readonly Mock<IUserService> _userServiceMock = new Mock<IUserService>();
        private readonly Mock<ILogger<UserController>> _loggerMock = new Mock<ILogger<UserController>>();

        [Fact]
        public void GetUsers_ShouldReturnUsers_WhenUsersExistWithEmailPartial()
        {
            // Arrange
            var usersDTO = new List<User> { new User { Email = "nathan@test.com", FullName = "Nathan Romero", Phone = "513-771-4079", Age = 20 } };
            _userServiceMock.Setup(x => x.GetUsers("nathan", "", "", SortOrder.Ascending)).Returns(usersDTO);
            UserController userController = new UserController(_userServiceMock.Object, _loggerMock.Object);

            // Act
            var userResult = userController.GetUsers("nathan", "", "", (int)SortOrder.Ascending) as ObjectResult;

            // Assert
            Assert.Equal(StatusCodes.Status200OK, userResult.StatusCode);
        }

        [Fact]
        public void RegisterUser_ShouldReturnError_WhenNameNotGiven()
        {
            // Arrange
            var userDTO = new User { Email = "nicholas@test.com", FullName = "", Phone = "425-663-7202", Age = 45 };
            UserController userController = new UserController(_userServiceMock.Object, _loggerMock.Object);

            // Act
            var userResult = userController.Register(userDTO) as ObjectResult;

            // Assert
            Assert.Equal(StatusCodes.Status422UnprocessableEntity, userResult.StatusCode);
        }

        [Fact]
        public void UpdateUser_ShouldReturnError_WhenEmailNotValid()
        {
            // Arrange
            var userDTO = new User { Email = "nicholas.com", FullName = "Nicholas Jefferson", Phone = "425-663-7202", Age = 45 };
            UserController userController = new UserController(_userServiceMock.Object, _loggerMock.Object);

            // Act
            var userResult = userController.Update(userDTO) as ObjectResult;

            // Assert
            Assert.Equal(StatusCodes.Status422UnprocessableEntity, userResult.StatusCode);
        }

        [Fact]
        public void DeleteUser_ShouldReturnOk_WhenEmailIsValid()
        {
            // Arrange
            string email = "marilyn@test.com";
            _userServiceMock.Setup(x => x.DeleteByEmail(email)).Returns(true);
            UserController userController = new UserController(_userServiceMock.Object, _loggerMock.Object);

            // Act
            var userResult = userController.Delete(email) as ObjectResult;

            // Assert
            Assert.Equal(StatusCodes.Status200OK, userResult.StatusCode);
        }
    }
}
