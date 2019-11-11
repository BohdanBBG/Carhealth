using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Carhealth.Models;
using Carhealth.ImportAndExport;
using System.Text.Json;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;


namespace Carhealth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class V1Controller : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly IConfiguration _configuration;

        public V1Controller(IRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }


        [Authorize]
        public IActionResult Index()
        {
            return Content(User.Identity.Name);
        }

        // GET api/V1/config
        [HttpGet("config")]
        public ActionResult<string> Config()
        {
            string host = _configuration["Config"];
            return "{ \"urls\" :{ \"api\" : \"https://localhost:5001\"} }";
        }

        // GET api/V1/car
        [Route("car")]
        [HttpGet]
        public ActionResult<string> Get()
        {
            CarEntity carEntity = _repository.Import();


            return JsonSerializer.Serialize<CarEntity>(carEntity);
        }

        // GET api/V1/cardetails/5/1
        [Route("cardetails/{offset}/{limit}")]
        [HttpGet("{offset}/{limit}")]
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


        //// Add TotalRide Post api/V1/totalride
        [Route("totalride")]
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

        //Add new CarItem POST api/V1/addcaritem
        [Route("addcaritem")]
        [HttpPost]
        public void Post([FromBody] CarItem carItem)
        {
            CarEntity carEntity = _repository.Import();

            carItem.Detail_id = carEntity.CarDetails.Last().Detail_id + 1;
            carEntity.CarDetails.Add(carItem);

            _repository.UpdateAllFileData(carEntity);
        }

        // PUT api/V1/5
        [HttpPut("{id}")]
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

        // DELETE api/V1/5
        [HttpDelete("{id}")]
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