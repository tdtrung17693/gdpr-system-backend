using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers
{
    public class ConversationController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return Ok("");
        }
    }
}