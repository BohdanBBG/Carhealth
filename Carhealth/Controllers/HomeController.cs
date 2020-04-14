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
    [Authorize]
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

        public IActionResult Index()
        {
           // return _userManager.GetUserId(User);
            return View("~/wwwroot/index.html");
        }


        [HttpGet("ping")]
        public string Ping()
        {
            return "Pong";
        }

        [HttpGet("home/config")]
        public ActionResult<string> Config()
        {
            string host = _configuration["api"];
            return "{ \"urls\" :{ \"api\" :\""+ host +"\"} }";

        }

        [HttpGet("home/allUsersCars")]
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

        [HttpPost("home/setUserCurCar")]
        public async Task<IActionResult> PostAsync([FromBody] ChangeUserCurrentCar changeModel)
        {
            if (ModelState.IsValid)
            {
                if (await db.CarEntities.AnyAsync(x => x.Id == changeModel.CarEntityId))
                {
                    db.CarEntities.FirstOrDefault(x => x.Id == changeModel.CarEntityId).IsCurrent = true;

                    await db.SaveChangesAsync();

                    return Ok();
                }
            }

                return NotFound();
        }

        [HttpGet("home/car")]
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

        [HttpGet("home/cardetails/{offset}/{limit}")]
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

                var carEntitySendData = db.CarItems.Where(x => x.CarEntityId == car.Id).Skip(offset).Take(limit).Select( x => new
                {
                    CarItemId = x.CarItemId,
                    Name = x.Name,
                    TotalRide = x.TotalRide,
                    ChangeRide = x.ChangeRide,
                    PriceOfDetail = x.PriceOfDetail,
                    DateOfReplace = x.DateOfReplace,
                    RecomendedReplace = x.RecomendedReplace
                });

                return Ok(JsonSerializer.Serialize(carEntitySendData));
            }

            return NotFound();

        }

        [HttpPost("home/totalride")]
        public async Task<IActionResult> PostAsync([FromBody] CarsTotalRide carsTotalRide)
        {
            if (carsTotalRide != null)
            {
                string userId = _userManager.GetUserId(User);

                var carEntity = await db.CarEntities.FirstOrDefaultAsync(x => x.IsCurrent == true && x.UserId == userId);


                if (carsTotalRide.TotalRide > 0 &&
                    carEntity != null &&
                    carsTotalRide.TotalRide > carEntity.CarsTotalRide
                    )
                {
                    int carEntityTotalRide = carEntity.CarsTotalRide;

                    await db.CarItems.Where(x => x.CarEntityId == carEntity.Id).
                        ForEachAsync(item =>
                        {
                             item.TotalRide += (carsTotalRide.TotalRide - carEntityTotalRide);
                        });

                    carEntity.CarsTotalRide = carsTotalRide.TotalRide;

                    await db.SaveChangesAsync();

                    return Ok();
                }
            }

            return NotFound();
        }

        [HttpPost("home/add/caritem")]
        public async Task<IActionResult> PostAsync([FromBody] CarItem carItem)
        {
            string userId = _userManager.GetUserId(User);

            carItem.CarEntityId = db.CarEntities.FirstOrDefault(x => x.IsCurrent == true && x.UserId == userId).Id;

            await db.CarItems.AddAsync(carItem);

            await db.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("home/put/caritem")]
        public async Task<IActionResult> PutAsync([FromBody] CarItem value)
        {
            string userId = _userManager.GetUserId(User);

            var carEntity = await db.CarEntities.FirstOrDefaultAsync(x => x.IsCurrent == true && x.UserId == userId);

            var carItem = await db.CarItems.FirstOrDefaultAsync(x => x.CarEntityId == carEntity.Id && x.CarItemId == value.CarItemId);

            if (carItem != null)
            {
                carItem.Name = value.Name;
                carItem.TotalRide = value.TotalRide;
                carItem.ChangeRide = value.ChangeRide;
                carItem.PriceOfDetail = value.PriceOfDetail;
                carItem.DateOfReplace = value.DateOfReplace;
                carItem.RecomendedReplace = value.RecomendedReplace;
                carItem.CarEntityId = carEntity.Id;

                await db.SaveChangesAsync();

                return Ok();
            }

            return NotFound();
        }

        [HttpDelete("home/delete/caritem")]
        public async Task<IActionResult> DeleteAsync(CarItemDelete deleteModel)
        {
            string userId = _userManager.GetUserId(User);

            var carEntity = await db.CarEntities.FirstOrDefaultAsync(x => x.IsCurrent == true && x.UserId == userId);

            var carItemToDelete = await db.CarItems.FirstOrDefaultAsync(x => x.CarEntityId == carEntity.Id && x.CarItemId == deleteModel.ItemId);


            if (carItemToDelete != null)
            {
                db.Remove(carItemToDelete);

                await db.SaveChangesAsync();

                return Ok();
            }

            return NotFound();
        }

        [HttpPost("home/add/car")]
        public async Task<IActionResult> CreateCar (CarEntity carEntity)
        {
            string userId = _userManager.GetUserId(User);

            return NotFound();
        }

        [HttpPut("home/put/car")]
        public async Task<IActionResult> EditCar(CarEntity carEntity)
        {
            string userId = _userManager.GetUserId(User);

            return NotFound();
        }

        [HttpDelete("home/delete/car")]
        public async Task<IActionResult> DeleteCar(CarItemDelete carEntity)
        {
            string userId = _userManager.GetUserId(User);

            return NotFound();
        }
    }
}
