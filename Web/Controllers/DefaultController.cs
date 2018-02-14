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
using Microsoft.EntityFrameworkCore;
using Web.Models.Default;

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
            var viewModel = new RealtorSearchViewModel();
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CaptureInfo(RealtorSearchViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);
            //https://www.realtor.com/realestateandhomes-search/22206/type-single-family-home,condo-townhome-row-home-co-op,multi-family-home/price-na-220000/pnd-hide/radius-50/sby-6/pg-3
            var propertyStatuses = _context.PropertyStatus.AsNoTracking().ToList();

            var pageNumber = 1;

            var gatheredData = new List<Data.Model.Property>();
            var parser = new HtmlParser();
            using (HttpClient client = new HttpClient())
            {
                while (pageNumber < viewModel.PagesToCapture)
                {
                    var url = $"https://www.realtor.com/realestateandhomes-search/{viewModel.ZipCode}/type-single-family-home,condo-townhome-row-home-co-op,multi-family-home/price-na-220000/pnd-hide/radius-{viewModel.Miles}/sby-6/{ (pageNumber == 1 ? "" : "pg-" + pageNumber.ToString())}";
                    using (HttpResponseMessage response = await client.GetAsync(url))
                    {
                        if (response.IsSuccessStatusCode)
                        {
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
                                        Baths = GetNumber(prop.QuerySelector("[data-label='property-meta-baths']")?.GetElementsByClassName("data-value").FirstOrDefault()?.TextContent),
                                        Beds = GetNumber(prop.QuerySelector("[data-label='property-meta-beds']")?.GetElementsByClassName("data-value").FirstOrDefault()?.TextContent),
                                        LotSize = GetNumber(prop.QuerySelector("[data-label='property-meta-lotsize']")?.GetElementsByClassName("data-value").FirstOrDefault()?.TextContent),
                                    };
                                    var status = prop.QuerySelector("[data-status]")?.GetAttribute("data-status") ?? "unknown";
                                    if (!propertyStatuses.Any(x => x.Name == status))
                                    {
                                        _context.PropertyStatus.Add(new Data.Model.PropertyStatus() { Name = status });
                                        _context.SaveChanges();
                                        propertyStatuses = _context.PropertyStatus.AsNoTracking().ToList();
                                    }
                                    newProp.PropertyStatusId = propertyStatuses.FirstOrDefault(x => x.Name == status).Id;
                                    gatheredData.Add(newProp);
                                }
                            }
                        }
                        else break;
                    }
                    pageNumber++;
                }
            }
            foreach (var property in gatheredData)
            {
                var currentObject = _context.Property.FirstOrDefault(x => x.RealtorListingId == property.RealtorListingId && x.RealtorPropertyId == property.RealtorPropertyId);
                if (currentObject != null)
                {
                    if (currentObject.AskingPrice != property.AskingPrice)
                    {
                        currentObject.AskingPrice = property.AskingPrice;
                    }
                    if (currentObject.PropertyStatusId != property.PropertyStatusId)
                    {
                        currentObject.PropertyStatusId = property.PropertyStatusId;
                    }
                }
                else
                {
                    _context.Property.Add(property);
                }
            }
            _context.SaveChanges();
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