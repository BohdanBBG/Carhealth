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

namespace Carhealth.Controllers
{
    public class HomeController : Controller
    {
      

        private readonly IConfiguration _configuration;
        private IWebHostEnvironment _hostingEnvironment;
        private readonly CarContext db;
        private UserManager<User> _userManager;


        public HomeController(IConfiguration configuration, IWebHostEnvironment hostingEnvironment, UserManager<User> userManager, CarContext context)
        {
            db = context;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
        }

        public string Test()
        {
            return "";
        }

        [Authorize]
        public IActionResult Index()
        {
           // return _userManager.GetUserId(User);
            return View("~/wwwroot/index.html");
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            if(User.Identity.IsAuthenticated)
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
            return "{ \"urls\" :{ \"api\" :\""+ host +"\"} }";

        }

        [Authorize]
        [HttpGet("allUsersCars")]
        public async Task<IActionResult> GetUsersCarsAsync()
        {
            string userId = _userManager.GetUserId(User);

            var result = await db.CarEntities.Where(x => x.UserId == userId).Select(x => new
            {
                CarEntityName = x.CarEntityName,
                Id = x.Id,
                IsDefault = x.IsCurrent

            }).ToListAsync();

            if (result != null)
            {
                return Ok(JsonSerializer.Serialize(result));
            }

            return NotFound();

        }

        [Authorize]
        [HttpPost("setUserCurCar")]
        public async Task<IActionResult> PostAsync([FromBody] ChangeUserCurrentCar changeModel)
        {
            if (ModelState.IsValid)
            {
                string userId = _userManager.GetUserId(User);

                if (await db.CarEntities.AnyAsync(x => x.Id == changeModel.CarEntityId))
                {
                    await db.CarEntities.Where(x=> x.UserId == userId).ForEachAsync(x => x.IsCurrent = false);

                    db.CarEntities.FirstOrDefault(x => x.Id == changeModel.CarEntityId).IsCurrent = true;

                    await db.SaveChangesAsync();

                    return Ok();
                }
            }

                return NotFound();
        }

        [Authorize]
        [HttpGet("car")]
        public async Task<ActionResult<string>> GetAsync()
        {
            string userId = _userManager.GetUserId(User);

            if (await db.CarEntities.AnyAsync(x => x.UserId == userId))
            {
                var car = await db.CarEntities.Include(x => x.CarItems).FirstOrDefaultAsync(x => x.IsCurrent == true && x.UserId == userId);

                return Ok(JsonSerializer.Serialize(car));

            }

            return NotFound();
        }

        [Authorize]
        [HttpPost("add/car")]
        public async Task<IActionResult> CreateCar([FromBody] CarEntity carEntity)
        {
            string userId = _userManager.GetUserId(User);

            return NotFound();
        }

        [Authorize]
        [HttpPut("put/car")]
        public async Task<IActionResult> EditCar([FromBody] CarEntity carEntity)
        {
            string userId = _userManager.GetUserId(User);

            return NotFound();
        }

        [Authorize]
        [HttpDelete("delete/car")]
        public async Task<IActionResult> DeleteCar(string carEntity)
        {
            string userId = _userManager.GetUserId(User);

            return NotFound();
        }

        [Authorize]
        [HttpGet("cardetails/{offset}/{limit}")]
        public async Task<ActionResult<string>> GetAsync(int offset, int limit)
        {

            string userId = _userManager.GetUserId(User);

            if (offset >= 0 &&
                limit > 0 &&
                offset <= await db.CarItems.CountAsync() &&
                await db.CarEntities.AnyAsync(x => x.IsCurrent == true && x.UserId == userId)
                )
            {
                var car = await db.CarEntities.FirstOrDefaultAsync(x => x.IsCurrent == true && x.UserId == userId);

                var carEntitySendData = new
                {
                    CountCarsItems = await db.CarItems.Where(x => x.CarEntityId == car.Id).CountAsync(),
                    CarItems = db.CarItems.Where(x => x.CarEntityId == car.Id).Skip(offset).Take(limit).Select(x => new
                    {
                        CarItemId = x.CarItemId,
                        Name = x.Name,
                        TotalRide = x.TotalRide,
                        ChangeRide = x.ChangeRide,
                        PriceOfDetail = x.PriceOfDetail,
                        DateOfReplace = x.DateOfReplace,
                        RecomendedReplace = x.RecomendedReplace,

                    })
                };
                return Ok(JsonSerializer.Serialize(carEntitySendData));
            }

            return NotFound();

        }

        [Authorize]
        [HttpGet("totalride")]
        public async Task<IActionResult> GetTotalRideAsync()
        {
            string userId = _userManager.GetUserId(User);

            var car = await db.CarEntities.FirstOrDefaultAsync(x => x.IsCurrent == true && x.UserId == userId);

            if(car != null)
            {
                return Ok( new 
                {
                    CarsTotalRide = car.CarsTotalRide
                });
            }

            return NotFound();
        }

        [Authorize]
        [HttpGet("totalride/set/{value}")]
        public async Task<IActionResult> PostAsync(int value = 0)
        {
            if (value != 0)
            {
                string userId = _userManager.GetUserId(User);

                var carEntity = await db.CarEntities.FirstOrDefaultAsync(x => x.IsCurrent == true && x.UserId == userId);


                if (value > 0 &&
                    carEntity != null &&
                    value > carEntity.CarsTotalRide
                    )
                {
                    int carEntityTotalRide = carEntity.CarsTotalRide;

                    await db.CarItems.Where(x => x.CarEntityId == carEntity.Id).
                        ForEachAsync(item =>
                        {
                             item.TotalRide += (value - carEntityTotalRide);
                        });

                    carEntity.CarsTotalRide = value;

                    await db.SaveChangesAsync();

                    return Ok();
                }
                return Ok();
            }

            return NotFound();
        }

        [Authorize]
        [HttpPost("add/caritem")]
        public async Task<IActionResult> PostAsync([FromBody] NewCarItem data)
        {
            if (data != null)
            {
                string userId = _userManager.GetUserId(User);

                await db.CarItems.AddAsync(new CarItem
                {
                    CarItemId = Guid.NewGuid().ToString(),
                    CarEntityId = db.CarEntities.FirstOrDefault(x => x.IsCurrent == true && x.UserId == userId).Id,
                    Name = data.Name,
                    TotalRide = 0,
                    ChangeRide = int.Parse(data.ChangeRide),
                    PriceOfDetail = int.Parse(data.PriceOfDetail),
                    RecomendedReplace = int.Parse(data.RecomendedReplace),
                    DateOfReplace = DateTime.Parse(data.DateOfReplace)

                });

                await db.SaveChangesAsync();

                return Ok();
            }
            return BadRequest();
        }

        [Authorize]
        [HttpPut("put/caritem")]
        public async Task<IActionResult> PutAsync([FromBody] UpdateCarItem value)
        {
            if (value != null)
            {
                string userId = _userManager.GetUserId(User);

                var carEntity = await db.CarEntities.FirstOrDefaultAsync(x => x.IsCurrent == true && x.UserId == userId);

                var carItem = await db.CarItems.FirstOrDefaultAsync(x => x.CarEntityId == carEntity.Id && x.CarItemId == value.CarItemId);

                if (carItem != null)
                {
                    carItem.Name = value.Name;
                    carItem.TotalRide = value.IsTotalRideChanged ? 0 : carItem.TotalRide;
                    carItem.ChangeRide = int.Parse(value.ChangeRide);
                    carItem.PriceOfDetail = int.Parse(value.PriceOfDetail);
                    carItem.DateOfReplace = DateTime.Parse(value.DateOfReplace);
                    carItem.RecomendedReplace = int.Parse(value.RecomendedReplace);

                    await db.SaveChangesAsync();

                    return Ok();
                }
            }
            return NotFound();
        }

        [Authorize]
        [HttpDelete("delete/caritem/{detailId}")]
        public async Task<IActionResult> DeleteAsync(string detailId)
        {
            string userId = _userManager.GetUserId(User);

            var carEntity = await db.CarEntities.FirstOrDefaultAsync(x => x.IsCurrent == true && x.UserId == userId);

            var carItemToDelete = await db.CarItems.FirstOrDefaultAsync(x => x.CarEntityId == carEntity.Id && x.CarItemId == detailId);


            if (carItemToDelete != null)
            {
                db.Remove(carItemToDelete);

                await db.SaveChangesAsync();

                return Ok();
            }

            return NotFound();
        }

        
    }
}
