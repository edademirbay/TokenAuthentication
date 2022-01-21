using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MiniApp1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StokController : ControllerBase
    {
      [HttpGet]
        public IActionResult GetStock()
        {
            var userName = HttpContext.User.Identity.Name;
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            return Ok($" Stok İşlemleri => UserName:{userName}- UserId:{userIdClaim.Value}");
        }
    }
}