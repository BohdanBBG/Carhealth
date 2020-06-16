using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CarHealth.Web.Controllers
{
    public class ConfigController : Controller
    {
        private readonly ApplicationSettings _settings;

        public ConfigController(IOptions<ApplicationSettings> settings)
        {
            _settings = settings.Value;
        }

        public IActionResult Index()
        {
            return Json(_settings);
        }
    }
}