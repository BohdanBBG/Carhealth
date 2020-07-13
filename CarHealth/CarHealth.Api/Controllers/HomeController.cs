using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CarHealth.Api.Models;
using Microsoft.AspNetCore.Authorization;
using CarHealth.Api.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using CarHealth.Api.Models.HttpModels;
using MongoDB.Bson;
using System.Security.Claims;
using IdentityModel;

namespace CarHealth.Api.Controllers
{
    public class HomeController : ControllerBase
    {


        private readonly ICarRepository _repository;


        public HomeController(ICarRepository repository)
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

       // [Authorize]
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }

        [Authorize]
        [HttpGet("allUsersCars")]
        public async Task<IActionResult> GetUsersCarsAsync()
        {
            var cars = await _repository.GetAllUsersCarsAsync(UserId);

            if (cars != null)
            {
                return Ok(cars.Select(x => new CarEntitySendModel
                {
                    CarEntityName = x.CarEntityName,
                    Id = x.Id,
                    TotalRide = x.CarsTotalRide,
                    IsDefault = x.IsCurrent

                }));
            } 
            return NotFound();
        }

        [Authorize]
        [HttpPost("setUserCurCar")]
        public async Task<IActionResult> SetUserCurCar([FromBody] ChangeUserCurrentCarModel changeModel)
        {
            if (ModelState.IsValid)
            {
                if( await _repository.SetUserCurCarAsync(changeModel.CarEntityId, UserId) )
                {
                    return Ok();
                }
                return NotFound();
            }
            return BadRequest();
        }

        [Authorize]
        [HttpGet("car")]
        public async Task<IActionResult> GetAsync()
        {
            var car = await _repository.GetCurrentCarAsync(UserId);

            if( car != null)
            {
                return Ok(car);
            }

            return NotFound();
        }

        [Authorize]
        [HttpPost("add/car")]
        public async Task<IActionResult> AddUserNewCar([FromBody] NewCarModel carEntity)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddUserNewCarAsync(new CarEntity
                {
                    CarEntityName = carEntity.CarEntityName,
                    CarsTotalRide = int.Parse(carEntity.CarsTotalRide),
                    IsCurrent = carEntity.IsCurrent,
                    UserId = UserId,
                    Id = ObjectId.GenerateNewId().ToString()
                });

                return Ok();
            }
            return BadRequest();
        }

        [Authorize]
        [HttpPut("put/car")]
        public async Task<IActionResult> UpdateUserCar([FromBody] EditCarModel carEntity)
        {
            if (ModelState.IsValid)
            {
                if (await _repository.UpdateUserCarAsync(carEntity, UserId))
                {
                    return Ok();
                }
                return NotFound();
            }
            return BadRequest();
        }

        [Authorize]
        [HttpDelete("delete/car")]
        public async Task<IActionResult> DeleteUserCar([FromQuery] string carEntityId)
        {
            if (await _repository.DeleteUserCarAsync(carEntityId, UserId))
            {
                return Ok();
            }
            return NotFound();
        }

        [Authorize]
        [HttpGet("find/caritem")]
        public async Task<IActionResult> FindCarItem([FromQuery] string name)
        {
            if (ModelState.IsValid)
            {
                var carItems = await _repository.FindCarItem(name, UserId);
                if (carItems != null)
                {
                    return Ok(carItems);
                }
            }
            return NotFound();
        }

        [Authorize]
        [HttpGet("cardetails")]
        public async Task<IActionResult> GetCarItemsAsync([FromQuery] int offset, [FromQuery] int limit)
        {
            var carItems = await _repository.GetCarItemsAsync(offset, limit, UserId);

            if(carItems != null)
            {
                return Ok(carItems);
            }

            return NotFound();

        }

        [Authorize]
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

        [Authorize]
        [HttpPost("totalride/set")]
        public async Task<IActionResult> SetTotalRideAsync([FromBody]  UpdateTotalRideModel value)
        {
            if(ModelState.IsValid)
            {
                if (await _repository.SetTotalRideAsync(value, UserId))
                {
                    return Ok();
                }
                return NotFound();
            }
            return BadRequest();
        }

        [Authorize]
        [HttpPost("add/caritem")]
        public async Task<IActionResult> AddNewCarItemAsync([FromBody] NewCarItemModel data)
        {
            if (ModelState.IsValid)
            {
                if (await _repository.AddNewCarItemAsync(new CarItem
                {
                    CarItemId = ObjectId.GenerateNewId().ToString(),
                    CarEntityId = data.CarEntityId,
                    Name = data.Name,
                    TotalRide = 0,
                    ChangeRide = int.Parse(data.ChangeRide),
                    PriceOfDetail = int.Parse(data.PriceOfDetail),
                    RecomendedReplace = int.Parse(data.RecomendedReplace),
                    DateOfReplace = DateTime.Parse(data.DateOfReplace)

                }, UserId))
                {
                    return Ok();
                }
                return NotFound();
            }
            return BadRequest();
        }

        [Authorize]
        [HttpPut("put/caritem")]
        public async Task<IActionResult> UpdateCarItemAsync([FromBody] UpdateCarItemModel value)
        {
            if (ModelState.IsValid)
            {
                if (await _repository.UpdateCarItemAsync(value, UserId))
                {
                    return Ok();
                }
                return NotFound();
            }
            return BadRequest();
        }

        [Authorize]
        [HttpDelete("delete/caritem/")]
        public async Task<IActionResult> DeleteAsync([FromQuery] string detailId)
        {

            if(await _repository.DeleteCarItemAsync(detailId, UserId))
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
