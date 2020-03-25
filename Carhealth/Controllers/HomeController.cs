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
            string host = _configuration["Config"];
            return "{ \"urls\" :{ \"api\" : \"https://localhost:5001\"} }";
        }

        // GET /home/car/1
        [Route("home/car/{id}")]
        [HttpGet]
        public ActionResult<string> Get(int id = 1)
        {

            var car = db.CarEntities.Where(x => x.Id == id).Include(x=>x.CarItems);

            return JsonSerializer.Serialize(car);
        }

        // GET home/cardetails/0/2
        [Route("home/cardetails/{offset}/{limit}")]
        [HttpGet("offset/limit")]
        public ActionResult<string> Get(int offset, int limit)
        {
            CarEntity carEntity = db.CarEntities.Find(1);
            CarEntity carEntitySendData = new CarEntity();
            carEntitySendData.CarEntityName = carEntity.CarEntityName;
            carEntitySendData.Id = carEntity.Id;
            carEntitySendData.CarsTotalRide = carEntity.CarsTotalRide;

            int counter = 1;
            int detailsTotalCount = db.CarItems.Count();

            if (offset < 0 || limit < 0 || offset > detailsTotalCount)
            {
                return null;
            }

            var carItems = db.CarItems;

            foreach(CarItem item in carItems)
            {

                if (counter > offset && (counter <= (offset + limit)) && counter <= detailsTotalCount)
                {
                    carEntitySendData.CarItems.Add(item);
                    counter++;
                }
                else
                {
                    counter++;
                }
            }

            
            return JsonSerializer.Serialize<CarEntity>(carEntitySendData);

        }


        // Add TotalRide Post home/totalride
        [Route("home/totalride")]
        [HttpPost]
        public void Post([FromBody] CarsTotalRide carsTotalRide)
        {
            CarEntity carEntity = db.CarEntities.Find(1);
           var carItems = db.CarItems.Where(x => x.CarEntityId == 1);

            if (carEntity.CarsTotalRide < carsTotalRide.TotalRide)
            {
               foreach(CarItem item in carItems)
                {
                    item.TotalRide += (carsTotalRide.TotalRide - carEntity.CarsTotalRide);
                }

                carEntity.CarsTotalRide = carsTotalRide.TotalRide;

                db.SaveChanges();
            }

          
        }

        //Add new CarItem POST home/addcaritem
        [Route("home/addcaritem")]
        [HttpPost]
        public void Post([FromBody] CarItem carItem)
        {
            carItem.CarEntityId = 1;

            db.CarItems.Add(carItem);

            db.SaveChanges();

        }

        // PUT home/5
        [HttpPut("home/{id}")]
        public void Put(int id, [FromBody] CarItem value)
        {
            if (id >= 0)
            {
                CarItem carItem = new CarItem();
                carItem = db.CarItems.FirstOrDefault(x => x.CarItemId == id);

                carItem.Name = value.Name;
                carItem.TotalRide = value.TotalRide;
                carItem.ChangeRide = value.ChangeRide;
                carItem.PriceOfDetail = value.PriceOfDetail;
                carItem.DateOfReplace = value.DateOfReplace;
                carItem.RecomendedReplace = value.RecomendedReplace;

                db.SaveChanges();
            }
        }


        // DELETE home/5
        [HttpDelete("home/{id}")]
        public void Delete(int id)
        {
            if (id >= 0)
            {
                CarItem carItem = new CarItem();
                carItem = db.CarItems.FirstOrDefault(x => x.CarItemId == id);

                if( carItem != null)
                {
                    db.CarItems.Remove(carItem);
                    db.SaveChanges();
                }
            }
        }

              
    }
}
