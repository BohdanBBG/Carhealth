using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarHealth.Api.Helpers;
using CarHealth.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CarHealth.Api.Controllers
{
    [Route("test")]
    [ApiController]
    public class TestController : Controller
    {
        public TestController()
        {

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

        [Authorize(Policy = "AdminsOnly")]
        [HttpGet("superpowers")]
        public IActionResult Superpowers()
        {
            return new JsonResult("Superpowers!");
        }

        [HttpGet("powers")]
        public IActionResult Powers()
        {
            return new JsonResult("Powers!");
        }

        [HttpGet("environment")]
        public IActionResult Environment()
        {
            return new  JsonResult(HostingEnvironmentHelper.Environment);
        }
    }
}