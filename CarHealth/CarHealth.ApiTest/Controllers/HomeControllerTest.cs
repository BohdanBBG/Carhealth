using CarHealth.Api.Controllers;
using CarHealth.Api.Models;
using CarHealth.Api.Models.HttpModels;
using CarHealth.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CarHealth.ApiTest.Controllers
{
    public class HomeControllerTest
    {
       

            [Fact(DisplayName = "Should return all user`s cars")]
        public async Task GetUsersCarsTestAsync()
        {
            // Arrange
            var mock = new Mock<ICarRepository>();
           // mock.Setup(repo =>  repo.GetAllUsersCarsAsync(It.IsAny<string>())).Returns(GetCarEntities());

            //var userIdMock = new Mock<HomeController>();
            //userIdMock.Setup(repo => repo.GetUserId()).Returns("id");

            var controller = new HomeController(mock.Object);

            // Act
            var result = await controller.GetUsersCarsAsync();

            // Assert

            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(objectResult.Value);

        }

        //[Fact(DisplayName = "Should return null ")]
        //public void GetNullUsersCarsTest()
        //{
        //    // Arrange
        //    var mock = new Mock<ICarRepository>();
        //    mock.Setup(repo => repo.GetAllUsersCarsAsync(GetUserId())).Returns(null as Task<List<CarEntity>>);

        //    var controller = new HomeController(mock.Object);

        //    // Act
        //    var result = controller.GetUsersCarsAsync().Result;

        //    // Assert
        //    Assert.IsType<NotFoundResult>(result);
        //    //Assert.IsType<OkObjectResult>(result);

        //}

        //[Fact(DisplayName = "")]
        //public void SetUserCurCarTest()
        //{
        //    // Arrange

        //    // Act

        //    // Assert
        //}

        //[Fact(DisplayName = "")]
        //public void GetCurrentCarTest()
        //{
        //    // Arrange

        //    // Act

        //    // Assert
        //}

        //[Fact(DisplayName = "")]
        //public void AddUserNewCarTest()
        //{
        //    // Arrange

        //    // Act

        //    // Assert
        //}

        //[Fact(DisplayName = "")]
        //public void UpdateUserCarTest()
        //{
        //    // Arrange

        //    // Act

        //    // Assert
        //}

        //[Fact(DisplayName = "")]
        //public void DeleteUserCarTest()
        //{
        //    // Arrange

        //    // Act

        //    // Assert
        //}

        //[Fact(DisplayName = "")]
        //public void FindCarItemTest()
        //{
        //    // Arrange

        //    // Act

        //    // Assert
        //}

        //[Fact(DisplayName = "")]
        //public void GetCarItemsAsyncTest()
        //{
        //    // Arrange

        //    // Act

        //    // Assert
        //}

        //[Fact(DisplayName = "")]
        //public void GetTotalRideAsyncTest()
        //{
        //    // Arrange

        //    // Act

        //    // Assert
        //}

        //[Fact(DisplayName = "")]
        //public void SetTotalRideAsyncTest()
        //{
        //    // Arrange

        //    // Act

        //    // Assert
        //}

        //[Fact(DisplayName = "")]
        //public void AddNewCarItemAsyncTest()
        //{
        //    // Arrange

        //    // Act

        //    // Assert
        //}

        //[Fact(DisplayName = "")]
        //public void UpdateCarItemAsyncTest()
        //{
        //    // Arrange

        //    // Act

        //    // Assert
        //}

        //[Fact(DisplayName = "")]
        //public void DeleteCarItemAsyncTest()
        //{
        //    // Arrange

        //    // Act

        //    // Assert
        //}

        private TController SetupController<TController, U>(Mock<U> mock) where TController : class, new()
                                                                           where U : class
        {
            var x = new TController();
            return null;
        }

        private string GetUserId() 
        {  
            return "id"; 
        }

        private Task<List<CarEntity>> GetCarEntities()
        {
            return Task.FromResult(new List<CarEntity>()
            {
                new CarEntity()
                {
                  Id = "1",
                  CarEntityName = "Test current car",
                  CarsTotalRide = 1000,
                  IsCurrent  = true,
                  UserId = GetUserId(),
                },
                 new CarEntity()
                {
                  Id = "2",
                  CarEntityName = "Test car 2",
                  CarsTotalRide = 2000,
                  IsCurrent  = false,
                  UserId = GetUserId(),
                },
            });
        }
    }
}
