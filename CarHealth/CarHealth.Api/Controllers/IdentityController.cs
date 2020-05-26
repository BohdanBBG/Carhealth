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
        public async Task<ActionResult<string>> Get()
        {
            //ClaimsPrincipal currentUser = this.User;
            //string currentUserId = null;

            //if (currentUser.HasClaim(x => x.Type == ClaimTypes.NameIdentifier))
            //{
            //    currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            //}
            //else if (currentUser.HasClaim(x => x.Type == JwtClaimTypes.Subject))
            //{
            //    currentUserId = currentUser.FindFirst(JwtClaimTypes.Subject).Value;
            //}
            //return BaseJsonResponse(currentUserId);
            return null;
        }
    }
}