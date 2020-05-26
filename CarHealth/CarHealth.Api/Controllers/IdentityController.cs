using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarHealth.Api.Controllers
{
    [ApiController]
    [Authorize]
    public class IdentityController : Controller
    {
        [HttpGet]
        [Route("superpowers")]
        [Authorize(Policy = "AdminsOnly")]
        public IActionResult Superpowers()
        {
            return new JsonResult("Superpowers!");
        }

        [HttpGet]
        [Route("powers")]
        [Authorize]
        public IActionResult Powers()
        {
            return new JsonResult("Powers!");
        }
    }
}