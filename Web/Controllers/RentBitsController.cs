using System.Net.Http;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Mvc;
using Web.Models.RentBits;

namespace Web.Controllers
{
    public class RentBitsController : Controller
    {

        private readonly IRealEstateContext _context;
        public RentBitsController(IRealEstateContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var viewModel = new RentBitsLoadViewModel();
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(RentBitsLoadViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);
            string url = "http://rentbits.com/rb/rates.do?rid=&pageNo=0&cpn=0&location={viewModel.Location}&type=A&beds=all";
            using (HttpClient client = new HttpClient())
            {

                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent content = response.Content)
                        {

                        }

                    }
                }

            }
            return View();
        }
    }
}