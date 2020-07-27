using CarHealth.Api.Models.HttpModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CarHealth.ApiTest.Utils
{
    public class ApiUtil<TStartup> where TStartup : class
    {
        protected readonly HttpClient _client;
        protected readonly HttpUtil _httpUtil;

        public ApiUtil(CustomWebApplicationBuilder<TStartup> factory)
        {
            _client = factory.CreateClient();
            _httpUtil = new HttpUtil(_client);
        }

     
        public async Task<List<CarEntitySendModel>> GetUsersCarsAsync(string accessToken)
        {
            var httpResponse = await _httpUtil.GetAsync("/allUsersCars", accessToken);

            //if(accessToken != null)
            //{
            //    throw new Exception(accessToken);
            //}

            _httpUtil.EnsureSuccessStatusCode(httpResponse);

            string stringResponse = await httpResponse.Content.ReadAsStringAsync(); // if you want to read status code than you need drop this
            var responeModel = JsonConvert.DeserializeObject<List<CarEntitySendModel>>(stringResponse);

            return responeModel;
        }

        //public async Task<IActionResult> SetUserCurCar([FromBody] ChangeUserCurrentCarModel changeModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _repository.UserId = UserId;
        //        }

        //        catch (NullReferenceException) { return Unauthorized(); }

        //        if (await _repository.SetUserCurCarAsync(changeModel.CarEntityId))
        //        {
        //            return Ok();
        //        }
        //        return NotFound();
        //    }
        //    return BadRequest();
        //}

        //public async Task<IActionResult> GetAsync()
        //{
        //    try
        //    {
        //        _repository.UserId = UserId;
        //    }

        //    catch (NullReferenceException) { return Unauthorized(); }

        //    var car = await _repository.GetCurrentCarAsync();

        //    if (car != null)
        //    {
        //        return Ok(car);
        //    }

        //    return NotFound();
        //}

       
        //public async Task<IActionResult> AddUserNewCar([FromBody] NewCarModel carEntity)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _repository.UserId = UserId;
        //        }

        //        catch (NullReferenceException) { return Unauthorized(); }

        //        await _repository.AddUserNewCarAsync(new CarEntity
        //        {
        //            CarEntityName = carEntity.CarEntityName,
        //            CarsTotalRide = int.Parse(carEntity.CarsTotalRide),
        //            IsCurrent = carEntity.IsCurrent,
        //            UserId = UserId,
        //            Id = ObjectId.GenerateNewId().ToString()
        //        });

        //        return Ok();
        //    }
        //    return BadRequest();
        //}
    
        //public async Task<IActionResult> UpdateUserCar([FromBody] EditCarModel carEntity)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _repository.UserId = UserId;
        //        }

        //        catch (NullReferenceException) { return Unauthorized(); }

        //        if (await _repository.UpdateUserCarAsync(carEntity))
        //        {
        //            return Ok();
        //        }
        //        return NotFound();
        //    }
        //    return BadRequest();
        //}

        //public async Task<IActionResult> DeleteUserCar([FromQuery] string carEntityId)
        //{
        //    try
        //    {
        //        _repository.UserId = UserId;
        //    }

        //    catch (NullReferenceException) { return Unauthorized(); }

        //    if (await _repository.DeleteUserCarAsync(carEntityId))
        //    {
        //        return Ok();
        //    }
        //    return NotFound();
        //}

        //public async Task<IActionResult> FindCarItem([FromQuery] string name)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _repository.UserId = UserId;
        //        }

        //        catch (NullReferenceException) { return Unauthorized(); }

        //        var carItems = await _repository.FindCarItem(name);
        //        if (carItems != null)
        //        {
        //            return Ok(carItems);
        //        }
        //    }
        //    return NotFound();
        //}

        //public async Task<IActionResult> GetCarItemsAsync([FromQuery] int offset, [FromQuery] int limit)
        //{
        //    try
        //    {
        //        _repository.UserId = UserId;
        //    }

        //    catch (NullReferenceException) { return Unauthorized(); }

        //    var carItems = await _repository.GetCarItemsAsync(offset, limit);

        //    if (carItems != null)
        //    {
        //        return Ok(carItems);
        //    }

        //    return NotFound();

        //}

        //public async Task<IActionResult> GetTotalRideAsync()
        //{
        //    try
        //    {
        //        _repository.UserId = UserId;
        //    }

        //    catch (NullReferenceException) { return Unauthorized(); }

        //    var result = await _repository.GetTotalRideAsync();

        //    if (result != null)
        //    {
        //        return Ok(result);
        //    }

        //    return NotFound();
        //}

        //public async Task<IActionResult> SetTotalRideAsync([FromBody] UpdateTotalRideModel value)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _repository.UserId = UserId;
        //        }

        //        catch (NullReferenceException) { return Unauthorized(); }

        //        if (await _repository.SetTotalRideAsync(value))
        //        {
        //            return Ok();
        //        }
        //        return NotFound();
        //    }
        //    return BadRequest();
        //}

        //public async Task<IActionResult> AddNewCarItemAsync([FromBody] NewCarItemModel data)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _repository.UserId = UserId;
        //        }

        //        catch (NullReferenceException) { return Unauthorized(); }

        //        if (await _repository.AddNewCarItemAsync(new CarItem
        //        {
        //            Id = ObjectId.GenerateNewId().ToString(),
        //            CarEntityId = data.CarEntityId,
        //            Name = data.Name,
        //            TotalRide = 0,
        //            ChangeRide = int.Parse(data.ChangeRide),
        //            PriceOfDetail = int.Parse(data.PriceOfDetail),
        //            RecomendedReplace = int.Parse(data.RecomendedReplace),
        //            DateOfReplace = DateTime.Parse(data.DateOfReplace)

        //        }))
        //        {
        //            return Ok();
        //        }
        //        return NotFound();
        //    }
        //    return BadRequest();
        //}

        //public async Task<IActionResult> UpdateCarItemAsync([FromBody] UpdateCarItemModel value)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _repository.UserId = UserId;
        //        }

        //        catch (NullReferenceException) { return Unauthorized(); }

        //        if (await _repository.UpdateCarItemAsync(value))
        //        {
        //            return Ok();
        //        }
        //        return NotFound();
        //    }
        //    return BadRequest();
        //}

        //public async Task<IActionResult> DeleteAsync([FromQuery] string detailId)
        //{
        //    try
        //    {
        //        _repository.UserId = UserId;
        //    }

        //    catch (NullReferenceException) { return Unauthorized(); }

        //    if (await _repository.DeleteCarItemAsync(detailId))
        //    {
        //        return Ok();
        //    }
        //    return NotFound();
        //}

    }
}
