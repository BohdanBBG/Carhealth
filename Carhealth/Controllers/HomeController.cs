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
        public async Task<IActionResult> GetUsersCarsAsync()
        {
            string userId = _userManager.GetUserId(User);

            var result = await db.CarEntities.Where(x => x.UserId == userId).Select(x => new
            {
                CarEntityName = x.CarEntityName,
                Id = x.Id,
                TotalRide = x.CarsTotalRide,
                IsDefault = x.IsCurrent

            }).ToListAsync();

            if (result != null)
            {
                return Ok(JsonSerializer.Serialize(result));
            }

            return NotFound("{}");

        }

        [Authorize]
        [HttpPost("setUserCurCar")]
        public async Task<IActionResult> PostAsync([FromBody] ChangeUserCurrentCar changeModel)
        {
            if (ModelState.IsValid)
            {
                string userId = _userManager.GetUserId(User);

                if (await db.CarEntities.AnyAsync(x => x.UserId == userId))
                {

                    if (await db.CarEntities.AnyAsync(x => x.Id == changeModel.CarEntityId))
                    {
                        await db.CarEntities.Where(x => x.UserId == userId).ForEachAsync(x => x.IsCurrent = false);

                        db.CarEntities.FirstOrDefault(x => x.Id == changeModel.CarEntityId).IsCurrent = true;

                        await db.SaveChangesAsync();

                        return Ok();
                    }
                    return BadRequest();
                }
                return NotFound();
            }
            return BadRequest();
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
        public async Task<IActionResult> CreateCar([FromBody] NewCar carEntity)
        {
            string userId = _userManager.GetUserId(User);


            if (carEntity != null)
            {
                if (!carEntity.IsCurrent && await db.CarEntities.AnyAsync(x => x.UserId == userId))
                {
                    await db.CarEntities.AddAsync(new CarEntity
                    {
                        CarEntityName = carEntity.CarEntityName,
                        CarsTotalRide = int.Parse(carEntity.CarsTotalRide),
                        IsCurrent = carEntity.IsCurrent,
                        UserId = userId,
                        Id = Guid.NewGuid().ToString()
                    });
                }
                else
                {
                    await db.CarEntities.Where(x => x.UserId == userId).ForEachAsync(x => x.IsCurrent = false);

                    await db.CarEntities.AddAsync(new CarEntity
                    {
                        CarEntityName = carEntity.CarEntityName,
                        CarsTotalRide = int.Parse(carEntity.CarsTotalRide),
                        IsCurrent = true,
                        UserId = userId,
                        Id = Guid.NewGuid().ToString()
                    });
                }

                await db.SaveChangesAsync();
            }
            return BadRequest();
        }

        [Authorize]
        [HttpPut("put/car")]
        public async Task<IActionResult> EditCar([FromBody] EditCar carEntity)
        {
            string userId = _userManager.GetUserId(User);

            if (await db.CarEntities.AnyAsync(x => x.UserId == userId))
            {

                if (carEntity != null)
                {
                    var car = await db.CarEntities.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == carEntity.Id);

                    if (car != null)
                    {
                        if (!carEntity.IsCurrent)
                        {
                            car.CarEntityName = carEntity.CarEntityName;
                            car.IsCurrent = carEntity.IsCurrent;
                        }
                        else
                        {
                            await db.CarEntities.Where(x => x.UserId == userId).ForEachAsync(x => x.IsCurrent = false);

                            car.CarEntityName = carEntity.CarEntityName;
                            car.IsCurrent = true;
                        }

                        await db.SaveChangesAsync();

                        return Ok();
                    }
                }
                return BadRequest();
            }
            return NotFound();
        }

        [Authorize]
        [HttpDelete("delete/car/{carEntityId}")]
        public async Task<IActionResult> DeleteCar(string carEntityId)
        {
            string userId = _userManager.GetUserId(User);

            if (await db.CarEntities.AnyAsync(x => x.UserId == userId))
            {
                if (carEntityId != null)
                {
                    var carToDelete = db.CarEntities.FirstOrDefault(x => x.UserId == userId && x.Id == carEntityId);

                    if (carToDelete != null)
                    {
                        db.CarItems.RemoveRange(db.CarItems.Where(x => x.CarEntityId == carToDelete.Id));

                        db.CarEntities.Remove(carToDelete);

                        await db.SaveChangesAsync();

                        return Ok();
                    }
                    return NotFound();
                }
                return BadRequest();
            }
            return NotFound();
        }

        [Authorize]
        [HttpGet("cardetails/{offset}/{limit}")]
        public async Task<ActionResult<string>> GetAsync(int offset, int limit)
        {
            string userId = _userManager.GetUserId(User);

            if (await db.CarEntities.AnyAsync(x => x.UserId == userId))
            {

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
                return BadRequest();
            }
            return NotFound();

        }

        [Authorize]
        [HttpGet("totalride")]
        public async Task<IActionResult> GetTotalRideAsync()
        {
            string userId = _userManager.GetUserId(User);

            if (await db.CarEntities.AnyAsync(x => x.UserId == userId))
            {

                var car = await db.CarEntities.FirstOrDefaultAsync(x => x.IsCurrent == true && x.UserId == userId);

                if (car != null)
                {
                    return Ok(new
                    {
                        CarsTotalRide = car.CarsTotalRide
                    });
                }
                return NotFound();
            }

            return NotFound();
        }

        [Authorize]
        [HttpPost("totalride/set")]
        public async Task<IActionResult> PostAsync([FromBody]  UpdateTotalRide value)
        {
            string userId = _userManager.GetUserId(User);

            if (await db.CarEntities.AnyAsync(x => x.UserId == userId))
            {
                if (value != null)
                {

                    var carEntity = await db.CarEntities.FirstOrDefaultAsync(x => x.Id == value.CarId && x.UserId == userId);


                    if (value.TotalRide > 0 &&
                        carEntity != null &&
                        value.TotalRide > carEntity.CarsTotalRide
                        )
                    {
                        int carEntityTotalRide = carEntity.CarsTotalRide;

                        await db.CarItems.Where(x => x.CarEntityId == carEntity.Id).
                            ForEachAsync(item =>
                            {
                                item.TotalRide += (value.TotalRide - carEntityTotalRide);
                            });

                        carEntity.CarsTotalRide = value.TotalRide;

                        await db.SaveChangesAsync();

                        return Ok();
                    }
                }
                return BadRequest();
            }
            return NotFound();

        }

        [Authorize]
        [HttpPost("add/caritem")]
        public async Task<IActionResult> PostAsync([FromBody] NewCarItem data)
        {
            string userId = _userManager.GetUserId(User);

            if (await db.CarEntities.AnyAsync(x => x.UserId == userId))
            {

                if (data != null)
                {

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
            return NotFound();
        }

        [Authorize]
        [HttpPut("put/caritem")]
        public async Task<IActionResult> PutAsync([FromBody] UpdateCarItem value)
        {
            string userId = _userManager.GetUserId(User);

            if (await db.CarEntities.AnyAsync(x => x.UserId == userId))
            {
                if (value != null)
                {

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
                return BadRequest();
            }
            return NotFound();
        }

        [Authorize]
        [HttpDelete("delete/caritem/{detailId}")]
        public async Task<IActionResult> DeleteAsync(string detailId)
        {
            string userId = _userManager.GetUserId(User);

            if (await db.CarEntities.AnyAsync(x => x.UserId == userId))
            {
                var carEntity = await db.CarEntities.FirstOrDefaultAsync(x => x.IsCurrent == true && x.UserId == userId);

                var carItemToDelete = await db.CarItems.FirstOrDefaultAsync(x => x.CarEntityId == carEntity.Id && x.CarItemId == detailId);


                if (carItemToDelete != null)
                {
                    db.CarItems.Remove(carItemToDelete);

                    await db.SaveChangesAsync();

                    return Ok();
                }

            }
            return NotFound();

        }


    }
}
