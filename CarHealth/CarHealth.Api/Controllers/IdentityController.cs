using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarHealth.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CarHealth.Api.Controllers
{
    [Route("identity")]
    [ApiController]
    public class IdentityController : Controller
    {

        private readonly IConfiguration _configuration;
        private IWebHostEnvironment _hostingEnvironment;


        public IdentityController(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
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

        [Authorize]
        [HttpGet]
        [Route("GetUser")]
        public IActionResult GetUser()
        {
            return new JsonResult(User);
        }


        [HttpGet("identity")]
        public IActionResult Get()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }


    }
}