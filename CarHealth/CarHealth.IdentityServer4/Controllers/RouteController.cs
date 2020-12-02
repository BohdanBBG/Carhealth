using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CarHealth.IdentityServer4.Controllers
{
    [Authorize]
    public class RouteController : Controller
    {
        public IConfiguration _config { get; set; }
        public RouteController(IConfiguration configuration)
        {
            _config = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GoToApiIndex()
        {
            return Redirect(_config.Get<ApplicationSettings>().Urls.Api);
        }

        public IActionResult GoToWebSpaIndex()
        {
            return Redirect(_config.Get<ApplicationSettings>().Urls.WebSpa);
        }

        public IActionResult GoToWebReactIndex()
        {
            return Redirect(_config.Get<ApplicationSettings>().Urls.WebSpaReact);
        }
    }
}