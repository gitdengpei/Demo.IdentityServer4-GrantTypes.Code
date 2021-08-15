using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Demo.WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class IdentityController : ControllerBase
	{ 
		[Authorize("ApiScope")]
		public IActionResult Get()
		{
			return new JsonResult(from claim in User.Claims select new { claim.Type, claim.Value });
		}
	}
}
