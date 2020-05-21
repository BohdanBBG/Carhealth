using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Carhealth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Carhealth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class V2Controller : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private IWebHostEnvironment _hostingEnvironment;
        private readonly CarContext db;
        private UserManager<User> _userManager;


        public V2Controller(IConfiguration configuration, IWebHostEnvironment hostingEnvironment, UserManager<User> userManager, CarContext context)
        {
            db = context;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
        }

        
    }
}