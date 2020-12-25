using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CarHealth.Api.Models;
using Microsoft.AspNetCore.Authorization;
using CarHealth.Api.Repositories;
using CarHealth.Api.Models.HttpModels;
using MongoDB.Bson;
using System.Security.Claims;
using IdentityModel;

namespace CarHealth.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")] 
    public class CarsController : ControllerBase
    {

        private readonly ICarRepository _repository;


        public CarsController(ICarRepository repository)
        {
            _repository = repository;
        }

        protected string UserId
        {
            get
            {
                string currentUserId = null;

                if (User.HasClaim(x => x.Type == ClaimTypes.NameIdentifier))
                {
                    currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                }
                else if (User.HasClaim(x => x.Type == JwtClaimTypes.Subject))
                {
                    currentUserId = User.FindFirst(JwtClaimTypes.Subject).Value;
                }


                return currentUserId;
            }

        }

        [HttpGet("allUsersCars")]
        public async Task<IActionResult> GetUsersCarsAsync()
        {
            var cars = await _repository.GetAllUsersCarsAsync(UserId);

            if (!cars.Any())
            {
                return NotFound();
            }

            return Ok(cars.Select(x => new CarEntitySendModel
            {
                CarEntityName = x.CarName,
                Id = x.Id,
                TotalRide = x.Mileage,
                IsDefault = x.IsCurrent

            }));

        }

        [HttpGet("car")]
        public async Task<IActionResult> GetAsync()
        {
            var car = await _repository.GetCurrentCarAsync(UserId);

            if (car != null)
            {
                return Ok(car);
            }

            return NotFound();
        }

        [HttpPost("add/car")]
        public async Task<IActionResult> AddUserNewCar([FromBody] NewCarModel carEntity)
        {

            await _repository.AddUserNewCarAsync(new CarEntity
            {
                CarName = carEntity.CarName,
                Mileage = int.Parse(carEntity.Mileage),
                IsCurrent = carEntity.IsCurrent,
                UserId = UserId,
            });

            return Ok();

        }

        [HttpPut("put/car")]
        public async Task<IActionResult> UpdateUserCar([FromBody] EditCarModel carEntity)
        {
            if (await _repository.UpdateUserCarAsync(carEntity, UserId))
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpDelete("delete/car")]
        public async Task<IActionResult> DeleteUserCar([FromQuery] string carEntityId)
        {
            if (await _repository.DeleteUserCarAsync(carEntityId, UserId))
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpGet("find/caritem")]
        public async Task<IActionResult> FindCarItem([FromQuery] string name)
        {
            var carItems = await _repository.FindCarItem(name, UserId);

            if (carItems != null)
            {
                return Ok(carItems);
            }

            return NotFound();
        }

        [HttpGet("cardetails")]
        public async Task<IActionResult> GetCarItemsAsync([FromQuery] int offset, [FromQuery] int limit)
        {
            var carItems = await _repository.GetCarItemsAsync(offset, limit, UserId);

            if (carItems != null)
            {
                return Ok(carItems);
            }

            return NotFound();

        }

        [HttpGet("totalride")]
        public async Task<IActionResult> GetTotalRideAsync()
        {
            var result = await _repository.GetTotalRideAsync(UserId);

            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }

        [HttpPost("totalride/set")]
        public async Task<IActionResult> SetTotalRideAsync([FromBody] UpdateCarMiliageModel value)
        {
            if (await _repository.SetTotalRideAsync(value, UserId))
            {
                return Ok();
            }

            return NotFound();
        }

        [HttpPost("add/caritem")]
        public async Task<IActionResult> AddNewCarItemAsync([FromBody] NewCarItemModel data)
        {

            if (await _repository.AddNewCarItemAsync(new CarItem
            {
                Id = ObjectId.GenerateNewId().ToString(),
                CarEntityId = data.CarEntityId,
                Name = data.Name,
                DetailMileage = 0,
                ChangeRide = int.Parse(data.ChangeRide),
                PriceOfDetail = int.Parse(data.PriceOfDetail),
                RecomendedReplace = int.Parse(data.RecomendedReplace),
                Replaced = DateTime.Parse(data.Replaced),
                ReplaceAt = DateTime.Parse(data.ReplaceAt)

            }, UserId))
            {
                return Ok();
            }

            return NotFound();
        }

        [HttpPut("put/caritem")]
        public async Task<IActionResult> UpdateCarItemAsync([FromBody] UpdateCarItemModel value)
        {
            if (await _repository.UpdateCarItemAsync(value, UserId))
            {
                return Ok();
            }

            return NotFound();
        }

        [HttpDelete("delete/caritem")]
        public async Task<IActionResult> DeleteAsync([FromQuery] string detailId)
        {
            if (await _repository.DeleteCarItemAsync(detailId, UserId))
            {
                return Ok();
            }

            return NotFound();
        }
    }
}