using System.Net.Http;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class HomesnapController : Controller
    {

        private readonly IRealEstateContext _context;
        public HomesnapController(IRealEstateContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}