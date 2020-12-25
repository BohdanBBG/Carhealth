using CarHealth.Api;
using CarHealth.Api.Models;
using CarHealth.Api.Models.HttpModels;
using CarHealth.ApiTest.Exceptions;
using CarHealth.ApiTest.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CarHealth.ApiTest.Controllers
{
    public class CarsControllerTest : TestBase
    {

        public CarsControllerTest(CustomWebApplicationBuilder<Startup> factory) : base(factory)
        {

        }

        #region GET methods

        [Fact]
        public async Task GetAllUsersCars_ReturnItemsList_List()
        {
            // Arrange
            await PrepareTestUser();

            int count = 2;
            var carEntities = await _dataUtil.CreateCarEntityInTestRepo(_user.Id, count);

            // Act
            var response = await _apiUtil.GetAsync<List<CarEntitySendModel>>("/api/cars/allUsersCars", _accessToken);

            // Assert
            Assert.NotEmpty(response);
            Assert.Equal(carEntities.Count, response.Count);
            response.ForEach(responseItem =>
            {
                Assert.Contains(carEntities, y => y.Id == responseItem.Id);
            });

        }

        [Fact]
        public async Task GetUsersCurrentCar_ReturnCurrentCar_CarObject()
        {
            // Arrange
            await PrepareTestUser();

            // Act
            int count = 2;
            var carEntities = await _dataUtil.CreateCarEntityInTestRepo(_user.Id, count);

            // Act
            var response = await _apiUtil.GetAsync<CarEntity>("/api/Cars/car", _accessToken);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsCurrent);
        }

        [Fact]
        public async Task GetCarItems_ReturnCarItemsList_CarItems()
        {
            // Arrange
            await PrepareTestUser();

            int count = 10;
            int limit = count;
            int offset = 2;
            var carItems = await _dataUtil.CreateCarItemInTestRepo(_user.Id, count);

            // Act
            var response = await _apiUtil.GetAsync<CarItemsSendModel>($"/api/Cars/cardetails?offset={offset}&limit={limit}", _accessToken);

            // Assert
            Assert.NotEmpty(response.CarItems);
            Assert.Equal(carItems.Count, response.CountCarsItems);
            response.CarItems.ForEach(responseItem =>
            {
                Assert.Contains(carItems, y => y.Id == responseItem.Id);
            });
        }

        [Fact]
        public async Task GetTotalRide_ReturnTotalRide_CarTotalRideModelObject()
        {
            // Arrange
            await PrepareTestUser();

            int count = 1;
            var carEntities = await _dataUtil.CreateCarEntityInTestRepo(_user.Id, count);

            // Act
            var response = await _apiUtil.GetAsync<CarTotalRideModel>("/api/Cars/totalride", _accessToken);

            // Assert
            Assert.Equal(carEntities.FirstOrDefault().Mileage, response.Miliage);

        }

        [Fact]
        public async Task FindCarItemsTest_SearchCarItems_CarItems() 
        {
            // Arrange
            await PrepareTestUser();

            int count = 10;
            string search = "test_search";

            var listOfSearch = new List<string>
            {
                $"{search}",
                $"{search}xxx x xxx",
                $"xxxx xxx{search}",
                $"xxxx x {search} xxx xx x",
                $"xxxx x xxx xx x"
            };

            var carItems = await _dataUtil.CreateCarItemInTestRepo(_user.Id, count, listOfSearch);

            // Act
            var response = await _apiUtil.FindCarItem($"/api/Cars/find/caritem?name={search}", _accessToken);

            // Assert

            response.ForEach(responseItem =>
            {
                Assert.Contains(carItems, y => y.Id != responseItem.Id);
            });

            response.ForEach(responseItem =>
            {
                Assert.Contains(search, responseItem.Name);
            });

            Assert.Equal(listOfSearch.Count(), response.Count() + 1);
        }

        #endregion

        #region POST methods

        [Fact]
        public async Task AddUserNewCar_ReturnOK_OkStatus()
        {
            // Arrange
            await PrepareTestUser();

            var carEntity = _dataUtil.CreateTestCarEntity(_user.Id, true);

            // Act
            var response = await _apiUtil.CreateAsync("/api/Cars/add/car", new NewCarModel
            {
                CarName = carEntity.CarName,
                Mileage = carEntity.Mileage.ToString(),
                IsCurrent = carEntity.IsCurrent
            }, _accessToken);

            var carDbEntity = await _apiUtil.GetAsync<CarEntity>("/api/Cars/car", _accessToken);

            // Assert

            Assert.NotNull(carDbEntity);
            Assert.Equal(carEntity.CarName, carDbEntity.CarName);
            Assert.Equal(carEntity.UserId, carDbEntity.UserId);
        }

        [Fact]
        public async Task AddNewCarItem_ReturnOK_OkStatus()
        {
            // Arrange
            await PrepareTestUser();

            int countCarItem = 2;
            int limit = countCarItem;
            int offset = 0;

            var carItems =  _dataUtil.CreateTestCarItem(_user.Id);
            var carItemsInRepo = await _dataUtil.CreateCarItemInTestRepo(_user.Id, countCarItem);

            var newCarItem = new NewCarItemModel
            {
                CarEntityId = carItemsInRepo.FirstOrDefault().CarEntityId,
                Name = carItems.FirstOrDefault().Name,
                ChangeRide = carItems.FirstOrDefault().ChangeRide.ToString(),
                PriceOfDetail = carItems.FirstOrDefault().PriceOfDetail.ToString(),
                Replaced = carItems.FirstOrDefault().Replaced.ToString(),
                ReplaceAt = carItems.FirstOrDefault().ReplaceAt.ToString(),
                RecomendedReplace = carItems.FirstOrDefault().RecomendedReplace.ToString()
            };

            // Act
            var response = await _apiUtil.CreateAsync("/api/Cars/add/caritem", newCarItem, _accessToken);

            var carItemsDb = await _apiUtil.GetAsync<CarItemsSendModel>($"/api/Cars/cardetails?offset={offset}&limit={limit + 1}", _accessToken);

            // Assert

            Assert.True(carItemsInRepo.Count() + 1 == carItemsDb.CountCarsItems);
        }

        [Fact]
        public async Task SetTotalRide_ReturnOK_OkStatus()
        {
            // Arrange
            await PrepareTestUser();

            var carEntity = await _dataUtil.CreateCarEntityInTestRepo(_user.Id);
            var updatedTotalRide = new UpdateCarMiliageModel
            {
                Id = carEntity.FirstOrDefault().Id,
                Miliage = carEntity.FirstOrDefault().Mileage + 2
            };

            // Act

            var totalRideResponse = await _apiUtil.CreateAsync("/api/Cars/totalride/set", updatedTotalRide, _accessToken);

            var carDbEntity = await _apiUtil.GetAsync<UpdateCarMiliageModel>("/api/Cars/car", _accessToken);

            // Assert

            Assert.NotNull(carDbEntity);
            Assert.NotEqual(updatedTotalRide.Miliage, carDbEntity.Miliage);

        }

        [Fact]
        public async Task SetTotalRide_ReturnNotFound_IsReturnedNotFound()
        {
            // Arrange
            await PrepareTestUser();

            var carEntity = await _dataUtil.CreateCarEntityInTestRepo(_user.Id);
            var updatedTotalRide = new UpdateCarMiliageModel
            {
                Id = "not_valid_id",
                Miliage = -1
            };

            // Act

            var ex = await Assert.ThrowsAsync<HttpStatusException>(() => _apiUtil.CreateAsync("/api/Cars/totalride/set", updatedTotalRide, _accessToken));

            // Assert

            Assert.Equal(HttpStatusCode.NotFound, ex.HttpStatusCode);
        }

        #endregion


        #region PUT methods

        [Fact]
        public async Task UpdateUserCar_ReturnOK_OkStatus()
        {
            // Arrange
            await PrepareTestUser();

            int count = 3;

            var carEntities = await _dataUtil.CreateCarEntityInTestRepo(_user.Id, count);

            var carDbEntity = carEntities.FirstOrDefault(x => x.IsCurrent);


            var updatedCar = new EditCarModel
            {
                Id = carDbEntity.Id,
                CarName = "New test name",
                IsCurrent = true,
                Mileage = carDbEntity.Mileage + count
            };

            // Act

            var responseUpdateResult = await _apiUtil.PutAsync("/api/Cars/put/car", updatedCar, _accessToken);

            var allCarEntitiesDb = await _apiUtil.GetAsync<List<CarEntitySendModel>>("/api/cars/allUsersCars", _accessToken);

            // Assert

            Assert.Contains(updatedCar.Id, allCarEntitiesDb.Select(x => x.Id));
            Assert.DoesNotContain(carDbEntity.CarName, allCarEntitiesDb.Select(x => x.CarEntityName));
            Assert.Equal(allCarEntitiesDb.Count(), carEntities.Count());
        }

        [Fact]
        public async Task UpdateCarItem_ReturnOK_OkStatus()
        {
            // Arrange
            await PrepareTestUser();

            int count = 4;
            int limit = count;
            int offset = 0;

            var carItems = await _dataUtil.CreateCarItemInTestRepo(_user.Id, count);

            var updatedCarItem = new UpdateCarItemModel
            {
                Id = carItems.FirstOrDefault().Id,
                Name = "New test car item",
                IsTotalRideChanged = false,
                ChangeRide = new Random().Next().ToString(),
                PriceOfDetail = new Random().Next().ToString(),
                Replaced = DateTime.Now.ToString(),
                ReplaceAt = DateTime.Now.ToString(),
                RecomendedReplace = new Random().Next().ToString(),
            };

            // Act

            var responseUpdateResult = await _apiUtil.PutAsync("/api/Cars/put/caritem", updatedCarItem, _accessToken);

            var allCarItemsDb = await _apiUtil.GetAsync<CarItemsSendModel>($"/api/Cars/cardetails?offset={offset}&limit={limit}", _accessToken);

            // Assert

            Assert.Contains(updatedCarItem.Id, allCarItemsDb.CarItems.Select(x => x.Id));

            Assert.DoesNotContain(updatedCarItem.Name, allCarItemsDb.CarItems.Select(x => x.Name));

            Assert.Equal(allCarItemsDb.CarItems.FirstOrDefault(x => x.Id == updatedCarItem.Id).DetailMileage,
                    carItems.FirstOrDefault(x => x.Id == updatedCarItem.Id).DetailMileage); // if IsTotalRideChanged = false, then TotalRide must be the same

            Assert.Equal(carItems.FirstOrDefault().CarEntityId, allCarItemsDb.CarEntityId);

            Assert.Equal(allCarItemsDb.CountCarsItems, carItems.Count());

            // TODO Assert for IsTotalRideChanged = true
        }
        #endregion


        #region DELETE methods

        [Fact]
        public async Task DeleteUserCar_ReturnOK_OkStatus()
        {
            // Arrange
            await PrepareTestUser();

            int count = 3;

            var carEntities = await _dataUtil.CreateCarEntityInTestRepo(_user.Id, count);

            var carDbEntity = carEntities.FirstOrDefault(x => x.IsCurrent);

            // Act

            var responseDeleteResult = await _apiUtil.DeleteAsync<CarEntity>($"/api/Cars/delete/car?carEntityId={carDbEntity.Id}", _accessToken);

            var allCarEntitiesDb = await _apiUtil.GetAsync<List<CarEntitySendModel>>("/api/cars/allUsersCars", _accessToken);

            // Assert

            Assert.Contains(true, allCarEntitiesDb.Select(x => x.IsDefault));
            Assert.DoesNotContain(carDbEntity.Id, allCarEntitiesDb.Select(x => x.Id));
            Assert.True(allCarEntitiesDb.Count() + 1 == carEntities.Count());

        }

        [Fact]
        public async Task DeleteCarItem_ReturnOK_OkStatus()
        {
            // Arrange
            await PrepareTestUser();

            int count = 6;
            int limit = count;
            int offset = 2;

            var carItems = await _dataUtil.CreateCarItemInTestRepo(_user.Id, count);

            var carItemDb = carItems.FirstOrDefault();

            // Act

            var responseDeleteResult = await _apiUtil.DeleteAsync<CarItem>($"/api/Cars/delete/caritem?detailId={carItemDb.Id}", _accessToken);

            var allCarItemsDb = await _apiUtil.GetAsync<CarItemsSendModel>($"/api/Cars/cardetails?offset={offset}&limit={limit}", _accessToken);

            // Assert
            Assert.Equal(carItems.Count() - 1, allCarItemsDb.CountCarsItems);
            Assert.DoesNotContain(carItemDb.Id, allCarItemsDb.CarItems.Select(x => x.Id));
        }

        #endregion
    }
}
