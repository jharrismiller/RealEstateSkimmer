using System.Net.Http;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Mvc;
using Web.Models.RentBits;
using System.Linq;
using AngleSharp.Parser.Html;
using Data.Model;
using Common;

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

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> GetDataForCurrentDbZips()
        {
            var needingData = (
                from p in _context.Property
                let rb = _context.RentBits.FirstOrDefault(r => r.Zip == p.Zip && r.Type == "House")
                where rb == null && p.Zip != null
                select new { p.Zip, PropertyType = "H" }
                ).Union(
                 from p in _context.Property
                 let rb = _context.RentBits.FirstOrDefault(r => r.Zip == p.Zip && r.Type == "Apartment")
                 where rb == null && p.Zip != null
                 select new { p.Zip, PropertyType = "A" }
                ).ToList();
            var parser = new HtmlParser();
            //string baseStringUrl = "http://rentbits.com/rb/rates.do?rid=&pageNo=0&cpn=0&location={0}&type={1}&beds=all";
            string baseStringUrl = "http://rentbits.com/rb/rate/stats.do?location={0}&type={1}";
            //http://rentbits.com/rb/rate/stats.do?location=20171&type=H


            using (HttpClient client = new HttpClient())
            {
                foreach (var needDataParameters in needingData)
                {
                    var url = string.Format(baseStringUrl, needDataParameters.Zip, needDataParameters.PropertyType);
                    using (HttpResponseMessage response = await client.GetAsync(url))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (HttpContent content = response.Content)
                            {
                                string pageHtml = await content.ReadAsStringAsync();
                                var document = parser.Parse(pageHtml);
                                var table = document.QuerySelector("[class='statsTable']");
                                if (table == null) continue;
                                var averageSecton = table.QuerySelectorAll("tr").FirstOrDefault(x => x.InnerHtml.Contains("Average:"));
                                if (averageSecton == null) continue;
                                var td = averageSecton.QuerySelectorAll("td");
                                if (td.Length == 5)
                                {
                                    var rent = new RentBits()
                                    {
                                        Zip = needDataParameters.Zip,
                                        Url = url,
                                        OneBedRoom = NumberHelper.GetNumber(td[2].TextContent) ?? 0,
                                        TwoBedRoom = NumberHelper.GetNumber(td[3].TextContent) ?? 0,
                                        ThreeOrMoreBedRoom = NumberHelper.GetNumber(td[4].TextContent) ?? 0,
                                        Type = needDataParameters.PropertyType=="H"  ? "House" : "Apartment"
                                    };
                                    _context.RentBits.Add(rent);
                                }

                            }

                        }
                    }
                }
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        //private class NeededData {
        //    public string Zip { get; set; }
        //    public string QueryType { get; set; }
        //}
    }
}