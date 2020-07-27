using CarHealth.Api;
using CarHealth.Api.Controllers;
using CarHealth.Api.Models;
using CarHealth.Api.Models.HttpModels;
using CarHealth.Api.Repositories;
using CarHealth.ApiTest.Utils;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CarHealth.ApiTest.Controllers
{
    public class HomeControllerTest: TestBase
    {

        public HomeControllerTest(CustomWebApplicationBuilder<Startup> factory) : base(factory)
        {

        }

        [Fact]
        public async Task GetAll_returnItemsList_list()
        {
            // Arrange
            await PrepareTestUser();

            int count = 2;
            var carEntities = _dataUtil.CreateCarEntityInTestRepo(_user.Id, count);

            // Act
            var response = await _apiUtil.GetUsersCarsAsync(_accessToken);

            // Assert
            Assert.NotEmpty(response);

        }
        //public async Task GetUsersCarsTest_IsUserFound_false()
        //{
        //    // Arrange
        //    var mock = new Mock<ICarRepository>();
        //    mock.Setup(repo =>  repo.GetAllUsersCarsAsync()).Returns(GetCarEntities());


        //    var controller = new HomeController(mock.Object);

        //    // Act
        //    var result = await controller.GetUsersCarsAsync();

        //    // Assert

        //    // var objectResult = Assert.IsType<OkObjectResult>(result);
        //    // Assert.NotNull(objectResult.Value);
        //    Assert.Throws<NullReferenceException>(() => controller.UserId);
        //}

        //[Fact(DisplayName = "Should return null ")]
        //public void GetNullUsersCarsTest_ReturnNoItems_null()
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
                  UserId = _user.Id,
                },
                 new CarEntity()
                {
                  Id = "2",
                  CarEntityName = "Test car 2",
                  CarsTotalRide = 2000,
                  IsCurrent  = false,
                  UserId = _user.Id,
                },
            });
        }
    }
}
