using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Carhealth.Models;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using Carhealth.Repositories;
using Microsoft.Extensions.Configuration;
using System.Net.Mime;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Carhealth.Models.HttpModels;
using Carhealth.Models.IdentityModels;
using MongoDB.Bson;

namespace Carhealth.Controllers
{
    public class HomeController : ControllerBase
    {


        private readonly IConfiguration _configuration;
        private IWebHostEnvironment _hostingEnvironment;
        private readonly ICarRepository _repository;
        private UserManager<User> _userManager;


        public HomeController(IConfiguration configuration, IWebHostEnvironment hostingEnvironment, UserManager<User> userManager, ICarRepository repository)
        {
            _repository = repository;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
        }


        [Authorize]
        public IActionResult Index()
        {
            return Ok();
            // return _userManager.GetUserId(User);
           // return View("~/wwwroot/index.html");
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Ok("Pong");
            }
            return Unauthorized("");
        }

        [Authorize]
        [HttpGet("config")]
        public ActionResult<string> Config()
        {
            string host = _configuration["api"];
            return "{ \"urls\" :{ \"api\" :\"" + host + "\"} }";

        }

        [Authorize]
        [HttpGet("allUsersCars")]
        public async Task<ActionResult<List<CarEntitySendModel>>> GetUsersCarsAsync()
        {
            string userId = _userManager.GetUserId(User);

            var cars = await _repository.GetAllUsersCarsAsync(userId);
            

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
            string userId = _userManager.GetUserId(User);

            if (ModelState.IsValid)
            {
                if( await _repository.SetUserCurCarAsync(changeModel.CarEntityId, userId) )
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
            string userId = _userManager.GetUserId(User);

            var car = await _repository.GetCurrentCarAsync(userId);

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
            string userId = _userManager.GetUserId(User);

            if (ModelState.IsValid)
            {
                await _repository.AddUserNewCarAsync(new CarEntity
                {
                    CarEntityName = carEntity.CarEntityName,
                    CarsTotalRide = int.Parse(carEntity.CarsTotalRide),
                    IsCurrent = carEntity.IsCurrent,
                    UserId = userId,
                    Id = Guid.NewGuid().ToString()
                });

                return Ok();
            }
            return BadRequest();
        }

        [Authorize]
        [HttpPut("put/car")]
        public async Task<IActionResult> UpdateUserCar([FromBody] EditCarModel carEntity)
        {
            string userId = _userManager.GetUserId(User);

            if (ModelState.IsValid)
            {
                if (await _repository.UpdateUserCarAsync(carEntity, userId))
                {
                    return Ok();
                }
                return NotFound();
            }
            return BadRequest();
        }

        [Authorize]
        [HttpDelete("delete/car/{carEntityId}")]
        public async Task<IActionResult> DeleteUserCar(string carEntityId)
        {
            string userId = _userManager.GetUserId(User);

            if (await _repository.DeleteUserCarAsync(carEntityId, userId))
            {
                return Ok();
            }
            return NotFound();
        }

        [Authorize]
        [HttpGet("cardetails/{offset}/{limit}")]
        public async Task<ActionResult<CarItemsSendModel>> GetCarItemsAsync(int offset, int limit)
        {
            string userId = _userManager.GetUserId(User);

            var carItems = await _repository.GetCarItemsAsync(offset, limit, userId);

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
            string userId = _userManager.GetUserId(User);

            var result = await _repository.GetTotalRideAsync(userId);

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
            string userId = _userManager.GetUserId(User);

            if(ModelState.IsValid)
            {
                if (await _repository.SetTotalRideAsync(value, userId))
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
            string userId = _userManager.GetUserId(User);
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

                }, userId))
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
            string userId = _userManager.GetUserId(User);

            if (ModelState.IsValid)
            {
                if (await _repository.UpdateCarItemAsync(value, userId))
                {
                    return Ok();
                }
                return NotFound();
            }
            return BadRequest();
        }

        [Authorize]
        [HttpDelete("delete/caritem/{detailId}")]
        public async Task<IActionResult> DeleteAsync(string detailId)
        {
            string userId = _userManager.GetUserId(User);

            if(await _repository.DeleteCarItemAsync(detailId, userId))
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
