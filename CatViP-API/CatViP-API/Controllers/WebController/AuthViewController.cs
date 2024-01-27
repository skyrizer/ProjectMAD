using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace CatViP_API.Controllers.WebController
{
    [Route("auth")]
    [EnableCors("AllowAll")]
    public class AuthViewController : Controller
    {
        [HttpGet("forgot-password")]
        public IActionResult Index(string email)
        {
            return View("index", email);
        }
    }
}
