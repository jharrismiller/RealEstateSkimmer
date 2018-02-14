using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Parser.Html;
using Data;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class DefaultController : Controller
    {

        private readonly IRealEstateContext _context;
        public DefaultController(IRealEstateContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CaptureInfo()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CaptureInfo(string url)
        {
            var gatheredData = new List<Data.Model.Property>();
            var parser = new HtmlParser();
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(url))
            using (HttpContent content = response.Content)
            {
                // ... Read the string.
                string pageHtml = await content.ReadAsStringAsync();
                var document = parser.Parse(pageHtml);
                var props = document.QuerySelectorAll("li[data-listingid]");
                foreach (var prop in props)
                {
                    var address = prop.QuerySelector("span[itemprop='address']");
                    var newProp = new Data.Model.Property()
                    {
                        Address = address.QuerySelector("[itemprop='streetAddress']")?.TextContent,
                        City = address.QuerySelector("[itemprop='addressLocality']")?.TextContent,
                        State = address.QuerySelector("[itemprop='addressRegion']")?.TextContent,
                        Zip = address.QuerySelector("[itemprop='postalCode']")?.TextContent,
                        RealtorUrl = prop.GetAttribute("data-url"),
                        RealtorListingId = prop.GetAttribute("data-listingid"),
                        RealtorPropertyId = prop.GetAttribute("data-propertyid"),
                        AskingPrice = GetNumber(
                            (prop.QuerySelector("[data-label='property-price']")?.GetElementsByClassName("data-price").FirstOrDefault()
                            ??
                            prop.GetElementsByClassName("data-price-display").FirstOrDefault()
                            )?.TextContent),
                        ListedAddress = prop.QuerySelector("img[itemprop='image']")?.GetAttribute("alt"),
                        Baths =  GetNumber(prop.QuerySelector("[data-label='property-meta-baths']")?.GetElementsByClassName("data-value").FirstOrDefault()?.TextContent),
                        Beds = GetNumber(prop.QuerySelector("[data-label='property-meta-beds']")?.GetElementsByClassName("data-value").FirstOrDefault()?.TextContent),
                        LotSize = GetNumber(prop.QuerySelector("[data-label='property-meta-lotsize']")?.GetElementsByClassName("data-value").FirstOrDefault()?.TextContent),
                    };
                   var  status = prop.QuerySelector("[data-status]")?.GetAttribute("data-status") ?? "unknown";
                    gatheredData.Add(newProp);
                }

                var i = 1;
            }
            //var client = new HttpClient();
            //var httpResponse = await client.GetAsync(url);
            //if (httpResponse.IsSuccessStatusCode)
            //{
            //    var data = httpResponse.Content;
            //    var i = 1;
            //}


            return View();
        }


        private int? GetNumber(string value)
        {
            int val;
            if (int.TryParse(value, out val))
            {
                return val;
            }
            else if (value != null)
            {
                var validNum = new StringBuilder();
                foreach (var c in value)
                {
                    if (char.IsNumber(c))
                        validNum.Append(c);
                    else if (c == '.')
                        break;
                }
                if (int.TryParse(validNum.ToString(), out val))
                {
                    return val;
                }
            }
            return null;
        }
    }




}