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


        private readonly IConfiguration _configuration;
        private IWebHostEnvironment _hostingEnvironment;
        private readonly ICarRepository _repository;


        public HomeController(IConfiguration configuration, IWebHostEnvironment hostingEnvironment, ICarRepository repository)
        {
            _repository = repository;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        [Authorize]
        protected string getUserId()
        {
            string currentUserId = null;

            if (User.HasClaim(x => x.Type == ClaimTypes.NameIdentifier))
            {
                currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            else if(User.HasClaim(x => x.Type == JwtClaimTypes.Subject))
            {
                currentUserId = User.FindFirst(JwtClaimTypes.Subject).Value;
            }
            

            return currentUserId;
        }

       // [Authorize]
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }

        [Authorize]
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Ok("Pong");
            }
            return Unauthorized("");
        }

      //  [Authorize]
        [HttpGet("config")]
        public ActionResult<string> Config()
        {
            return _configuration.Get<ApplicationSettings>().Urls.ToJson();
            // return "{ \"urls\" :{ \"api\" :\" https://localhost:5001\"} }";

        }

       

        [Authorize]
        [HttpGet("allUsersCars")]
        public async Task<ActionResult<List<CarEntitySendModel>>> GetUsersCarsAsync()
        {

            var cars = await _repository.GetAllUsersCarsAsync(getUserId());
            

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
            return null;
        }

        [Authorize]
        [HttpPost("setUserCurCar")]
        public async Task<IActionResult> SetUserCurCar([FromBody] ChangeUserCurrentCarModel changeModel)
        {
            if (ModelState.IsValid)
            {
                if( await _repository.SetUserCurCarAsync(changeModel.CarEntityId, getUserId()) )
                {
                    return Ok();
                }
                return NotFound();
            }
            return BadRequest();
        }

        [Authorize]
        [HttpGet("car")]
        public async Task<ActionResult<CarEntity>> GetAsync()
        {
            var car = await _repository.GetCurrentCarAsync(getUserId());

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
                    UserId = getUserId(),
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
                if (await _repository.UpdateUserCarAsync(carEntity, getUserId()))
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
            if (await _repository.DeleteUserCarAsync(carEntityId, getUserId()))
            {
                return Ok();
            }
            return NotFound();
        }

        [Authorize]
        [HttpGet("find/caritem")]
        public async Task<ActionResult<IList<CarItem>>> FindCarItem([FromQuery] string name)
        {
            if (ModelState.IsValid)
            {
                var carItems = await _repository.FindCarItem(name, getUserId());
                if (carItems != null)
                {
                    return Ok(carItems);
                }
            }
            return NotFound();
        }

        [Authorize]
        [HttpGet("cardetails")]
        public async Task<ActionResult<CarItemsSendModel>> GetCarItemsAsync([FromQuery] int offset, [FromQuery] int limit)
        {
            var carItems = await _repository.GetCarItemsAsync(offset, limit, getUserId());

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
            var result = await _repository.GetTotalRideAsync(getUserId());

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
                if (await _repository.SetTotalRideAsync(value, getUserId()))
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

                }, getUserId()))
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
                if (await _repository.UpdateCarItemAsync(value, getUserId()))
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

            if(await _repository.DeleteCarItemAsync(detailId, getUserId()))
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
