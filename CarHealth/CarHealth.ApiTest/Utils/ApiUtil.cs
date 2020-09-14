using CarHealth.Api.Models;
using CarHealth.Api.Models.HttpModels;
using MongoDB.Bson;
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

        #region GET methods

        public async Task<T> GetAsync<T>(string url, string accessToken)
        {
            var httpResponse = await _httpUtil.GetAsync(url, accessToken);

            _httpUtil.EnsureSuccessStatusCode(httpResponse);

            string stringResponse = await httpResponse.Content.ReadAsStringAsync(); // if you want to read status code than you need drop this
            var responeModel = JsonConvert.DeserializeObject<T>(stringResponse);

            return responeModel;
        }

        public async Task<List<CarItem>> FindCarItem(string url, string accessToken)
        {
            var httpRespone = await _httpUtil.GetAsync(url , accessToken);

            _httpUtil.EnsureSuccessStatusCode(httpRespone);

            string stringResponse = await httpRespone.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<List<CarItem>>(stringResponse);

            return responseModel;
        }

        #endregion

        #region Post methods

        public async Task<T> CreateAsync<T>(string url, T data, string accessToken)
        {
            var httpResponse = await _httpUtil.PostJsonAsync(url, data, accessToken);

            _httpUtil.EnsureSuccessStatusCode(httpResponse);

            string stringResponse = await httpResponse.Content.ReadAsStringAsync(); // if you want to read status code than you need drop this
            var responeModel = JsonConvert.DeserializeObject<T>(stringResponse);

            return responeModel;
        }

        #endregion

        #region PUT methods

        public async Task<T> PutAsync<T>(string url, T data, string accessToken)
        {
            var httpResponse = await _httpUtil.PutJsonAsync(url, data, accessToken);

            _httpUtil.EnsureSuccessStatusCode(httpResponse);

            string stringResponse = await httpResponse.Content.ReadAsStringAsync(); // if you want to read status code than you need drop this
            var responeModel = JsonConvert.DeserializeObject<T>(stringResponse);

            return responeModel;
        }

        #endregion

        #region DELETE methods

        public async Task<T> DeleteAsync<T>(string url, string accessToken)
        {
            var httpResponse = await _httpUtil.DeleteAsync(url, accessToken);

            _httpUtil.EnsureSuccessStatusCode(httpResponse);

            string stringResponse = await httpResponse.Content.ReadAsStringAsync(); // if you want to read status code than you need drop this
            var responeModel = JsonConvert.DeserializeObject<T>(stringResponse);

            return responeModel;
        }

        #endregion

    }
}
