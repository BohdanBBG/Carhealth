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

        public string UserId
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

                return null;
                //return currentUserId ?? throw new NullReferenceException("UserId is null"); ;
            }
             set
            {
                _repository.UserId = value ?? throw new NullReferenceException("User Id is null");
            }
        }


        public HomeController(ICarRepository repository)
        {
            _repository = repository;

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
            //try
            //{
            //    _repository.UserId = UserId; // TODO change getUserIds
            //}

            //catch (NullReferenceException) { return Unauthorized(); }

            var cars = await _repository.GetAllUsersCarsAsync();

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
                try
                {
                    _repository.UserId = UserId;
                }

                catch (NullReferenceException) { return Unauthorized(); }

                if ( await _repository.SetUserCurCarAsync(changeModel.CarEntityId) )
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
            try
            {
                _repository.UserId = UserId;
            }

            catch (NullReferenceException) { return Unauthorized(); }

            var car = await _repository.GetCurrentCarAsync();

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
                try
                {
                    _repository.UserId = UserId;
                }

                catch (NullReferenceException) { return Unauthorized(); }

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
                try
                {
                    _repository.UserId = UserId;
                }

                catch (NullReferenceException) { return Unauthorized(); }

                if (await _repository.UpdateUserCarAsync(carEntity))
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
            try
            {
                _repository.UserId = UserId;
            }

            catch (NullReferenceException) { return Unauthorized(); }

            if (await _repository.DeleteUserCarAsync(carEntityId))
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
                try
                {
                    _repository.UserId = UserId;
                }

                catch (NullReferenceException) { return Unauthorized(); }

                var carItems = await _repository.FindCarItem(name);
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
            try
            {
                _repository.UserId = UserId;
            }

            catch (NullReferenceException) { return Unauthorized(); }

            var carItems = await _repository.GetCarItemsAsync(offset, limit);

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
            try
            {
                _repository.UserId = UserId;
            }

            catch (NullReferenceException) { return Unauthorized(); }

            var result = await _repository.GetTotalRideAsync();

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
                try
                {
                    _repository.UserId = UserId;
                }

                catch (NullReferenceException) { return Unauthorized(); }

                if (await _repository.SetTotalRideAsync(value))
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
                try
                {
                    _repository.UserId = UserId;
                }

                catch (NullReferenceException) { return Unauthorized(); }

                if (await _repository.AddNewCarItemAsync(new CarItem
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    CarEntityId = data.CarEntityId,
                    Name = data.Name,
                    TotalRide = 0,
                    ChangeRide = int.Parse(data.ChangeRide),
                    PriceOfDetail = int.Parse(data.PriceOfDetail),
                    RecomendedReplace = int.Parse(data.RecomendedReplace),
                    DateOfReplace = DateTime.Parse(data.DateOfReplace)

                }))
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
                try
                {
                    _repository.UserId = UserId;
                }

                catch (NullReferenceException) { return Unauthorized(); }

                if (await _repository.UpdateCarItemAsync(value))
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
            try
            {
                _repository.UserId = UserId;
            }

            catch (NullReferenceException) { return Unauthorized(); }

            if (await _repository.DeleteCarItemAsync(detailId))
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
