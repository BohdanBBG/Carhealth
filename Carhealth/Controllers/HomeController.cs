using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Carhealth.Models;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using Carhealth.ImportAndExport;
using Microsoft.Extensions.Configuration;
using System.Net.Mime;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Carhealth.Controllers
{
    public class HomeController : Controller
    {
      

        private readonly IRepository _repository;
        private readonly IConfiguration _configuration;
        private IHostingEnvironment _hostingEnvironment;


        public HomeController(IRepository repository, IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _repository = repository;
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

        // GET home/car
        [Route("home/car")]
        [HttpGet]
        public ActionResult<string> Get()
        {
            CarEntity carEntity = _repository.Import();


            return JsonSerializer.Serialize<CarEntity>(carEntity);
        }

        // GET home/cardetails/5/1
        [Route("home/cardetails/{offset}/{limit}")]
        [HttpGet("offset/limit")]
        public ActionResult<string> Get(int offset, int limit)
        {
            CarEntity carEntity = _repository.Import();

            CarEntity carEntitySendData = new CarEntity();
            carEntitySendData.CarDetails = new List<CarItem>();
            carEntitySendData.CarEntityName = carEntity.CarEntityName;
            carEntitySendData.CarId = carEntity.CarId;
            carEntitySendData.CountCarsItems = carEntity.CountCarsItems;
            carEntitySendData.CarsTotalRide = carEntity.CarsTotalRide;

            int counter = 1;
            int detailsTotalCount = carEntity.CountCarsItems;

            if (offset < 0 || limit < 0 || offset > carEntity.CountCarsItems)
            {
                return null;
            }

            carEntity.CarDetails.ForEach(item =>
            {
                if (counter > offset && (counter <= (offset + limit)) && counter <= detailsTotalCount)
                {
                    carEntitySendData.CarDetails.Add(item);
                    counter++;
                }
                else
                {
                    counter++;
                }

            });
            return JsonSerializer.Serialize<CarEntity>(carEntitySendData);
        }


        //// Add TotalRide Post home/totalride
        [Route("home/totalride")]
        [HttpPost]
        public void Post([FromBody] CarsTotalRide carsTotalRide)
        {

            CarEntity carEntity = _repository.Import();
            if (carEntity.CarsTotalRide < carsTotalRide.TotalRide)
            {

                _repository.ReCalcCarItemsRides(carEntity, carsTotalRide.TotalRide);

                carEntity.CarsTotalRide = carsTotalRide.TotalRide;

                _repository.UpdateAllFileData(carEntity);
            }

        }

        //Add new CarItem POST home/addcaritem
        [Route("home/addcaritem")]
        [HttpPost]
        public void Post([FromBody] CarItem carItem)
        {
            CarEntity carEntity = _repository.Import();

            carItem.Detail_id = carEntity.CarDetails.Last().Detail_id + 1;
            carEntity.CarDetails.Add(carItem);

            _repository.UpdateAllFileData(carEntity);
        }

        // PUT home/5
        [HttpPut("home/{id}")]
        public void Put(int id, [FromBody] CarItem value)
        {
            if (id >= 0)
            {
                CarEntity carEntity = _repository.Import();
                CarItem carItemToDelete = new CarItem();

                carEntity.CarDetails.ForEach(item =>
                {
                    if (item.Detail_id == id)
                    {
                        item.Name = value.Name;
                        item.TotalRide = value.TotalRide;
                        item.ChangeRide = value.ChangeRide;
                        item.PriceOfDetail = value.PriceOfDetail;
                        item.DateOfReplace = value.DateOfReplace;
                        item.RecomendedReplace = value.RecomendedReplace;
                    }
                });

                _repository.UpdateAllFileData(carEntity);
            }
        }

        // DELETE home/5
        [HttpDelete("home/{id}")]
        public void Delete(int id)
        {
            if (id >= 0)
            {
                CarEntity carEntity = _repository.Import();
                CarItem carItemToDelete = new CarItem();

                carEntity.CarDetails.ForEach(item =>
                {
                    if (item.Detail_id == id)
                    {
                        carItemToDelete = item;
                    }
                });

                int carItemToDeleteIndex = carEntity.CarDetails.IndexOf(carItemToDelete);

                if (carEntity.CarDetails.Contains(carItemToDelete))
                {
                    carEntity.CarDetails.RemoveAt(carItemToDeleteIndex);
                }
                _repository.UpdateAllFileData(carEntity);
            }
        }
    }
}
