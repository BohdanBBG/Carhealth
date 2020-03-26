﻿using System;
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

namespace Carhealth.Controllers
{
    public class HomeController : Controller
    {
      

        private readonly IConfiguration _configuration;
        private IHostingEnvironment _hostingEnvironment;
        private readonly CarContext db;


        public HomeController(IConfiguration configuration, IHostingEnvironment hostingEnvironment, CarContext context)
        {
            db = context;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }


        [Authorize]
        public IActionResult Start()
        {
            return View("~/wwwroot/index.html");
        }

        // GET home/config
        [Route("home/config")]
        [HttpGet]
        public ActionResult<string> Config()
        {
           // string host = _configuration["Config"];
            return "{ \"urls\" :{ \"api\" : \"https://localhost:5001\"} }";
        }

        // GET /home/car/1
        [Route("home/car/{id}")]
        [HttpGet]
        public async Task<ActionResult<string>> GetAsync(int? id)
        {
            if (id != null && await db.CarEntities.AnyAsync(x => x.Id == id))
            {
                var car = await db.CarEntities.Include(x => x.CarItems).FirstOrDefaultAsync(x => x.Id == id);

                return Ok(JsonSerializer.Serialize(car));

            }

            return NotFound();
        }

        // GET home/cardetails/1/0/2
        [Route("home/cardetails/{carId}/{offset}/{limit}")]
        [HttpGet("offset/limit")]
        public async Task<ActionResult<string>> GetAsync(int carId, int offset, int limit)
        {
            if (offset >= 0 && 
                limit > 0 &&
                offset <= await db.CarItems.CountAsync() &&
                await db.CarEntities.AnyAsync(x => x.Id == carId))
            {
                var carEntitySendData = db.CarItems.Where(x => x.CarEntityId == carId).Skip(offset).Take(limit);

                return Ok(JsonSerializer.Serialize(carEntitySendData));
            }
            return NotFound();

        }


        // Add TotalRide Post home/totalride
        [Route("home/totalride")]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CarsTotalRide carsTotalRide)
        {
            var carEntity = await db.CarEntities.FirstOrDefaultAsync(x => x.Id == carsTotalRide.CarEntityId);

            if (carsTotalRide != null &&
                carsTotalRide.CarEntityId != 0 &&
                carsTotalRide.TotalRide > 0 &&
                carEntity != null &&
                carsTotalRide.TotalRide >  carEntity.CarsTotalRide)
            {
                int carEntityTotalRide = carEntity.CarsTotalRide;

                await db.CarItems.Where(x => x.CarEntityId == carsTotalRide.CarEntityId).ForEachAsync(item =>
                                                                                                             {
                                                                                                                 item.TotalRide += (carsTotalRide.TotalRide - carEntityTotalRide);
                                                                                                             });

                carEntity.CarsTotalRide = carsTotalRide.TotalRide;

                await db.SaveChangesAsync();

                return Ok();
            }

            return NotFound();

        }

        //Add new CarItem POST home/addcaritem
        [Route("home/addcaritem")]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CarItem carItem)
        {
            await db.CarItems.AddAsync(carItem);

            await db.SaveChangesAsync();

            return Ok();
        }

        // PUT home/
        [HttpPut("home")]
        public async Task<IActionResult> PutAsync([FromBody] CarItem value)
        {
            var carItem = await db.CarItems.FirstOrDefaultAsync(x => x.CarEntityId == value.CarEntityId && x.CarItemId == value.CarItemId);

            if (carItem != null)
            {
                carItem.Name = value.Name;
                carItem.TotalRide = value.TotalRide;
                carItem.ChangeRide = value.ChangeRide;
                carItem.PriceOfDetail = value.PriceOfDetail;
                carItem.DateOfReplace = value.DateOfReplace;
                carItem.RecomendedReplace = value.RecomendedReplace;
                carItem.CarEntityId = value.CarEntityId;

                await db.SaveChangesAsync();

                return Ok();
            }

            return NotFound();

        }


        // DELETE home/5
        [HttpDelete("home/{carId}/{itemId}")]
        public async Task<IActionResult> DeleteAsync(int carId, int itemId)
        {
            var carItemToDelete = await db.CarItems.FirstOrDefaultAsync(x => x.CarItemId == itemId && x.CarEntityId == carId);

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
