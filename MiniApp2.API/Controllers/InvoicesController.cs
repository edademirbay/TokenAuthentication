using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MiniApp2.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
       [HttpGet]
        public IActionResult GetInvoices()
        {
            var userName = HttpContext.User.Identity.Name;
            var userIdClaim= User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            return Ok($"Invoices Islemleri => UserName:{userName}- UserId:{userIdClaim.Value}");
        }

    }
}