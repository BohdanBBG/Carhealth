using CarHealth.Api;
using CarHealth.Api.Models.IdentityModels;
using CarHealth.Api.Repositories;
using CarHealth.ApiTest.Auth;
using CarHealth.ApiTest.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CarHealth.ApiTest
{
    public abstract class TestBase : IClassFixture<CustomWebApplicationBuilder<Startup>>
    {
        protected readonly CustomWebApplicationBuilder<Startup> _factory;
        protected readonly HttpClient _client;
        protected readonly DataUtil<Startup> _dataUtil;
        protected readonly ApiUtil<Startup> _apiUtil;
        protected readonly HttpUtil _httpUtil;

        protected readonly ICarRepository _dataRepository;

        protected User _user { get; set; }
        protected string _accessToken{ get; set; }

        public TestBase(CustomWebApplicationBuilder<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _dataUtil = new DataUtil<Startup>(factory);
            _apiUtil = new ApiUtil<Startup>(factory);
            _httpUtil = new HttpUtil(_client);
            _dataRepository = factory.Server.Host.Services.GetRequiredService<ICarRepository>();
        }

        protected async Task PrepareTestUser()
        {
            _user = await _dataUtil.CreateUserAsync();
            _accessToken = TestAuthenticationHelper.GenerateAccessToken(_user);
        }

    }
}
