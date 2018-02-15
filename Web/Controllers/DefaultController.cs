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
            using (HttpClient client = new HttpClient())
            {
                //https://www.realtor.com/realestateandhomes-search/22206/type-single-family-home,condo-townhome-row-home-co-op,multi-family-home/price-na-220000/pnd-hide/radius-50/sby-6/pg-3
                var propertyStatuses = _context.PropertyStatus.AsNoTracking().ToList();
                var propertyTypes = _context.PropertyType.AsNoTracking().ToList();
                var pageNumber = 1;

                var gatheredData = new List<Data.Model.Property>();
                var parser = new HtmlParser();

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
                                    var info = prop.QuerySelector("[class='data-wrap']");
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
                                        Latitude = prop.QuerySelector("[itemprop='latitude']")?.GetAttribute("content"),
                                        Longitude = prop.QuerySelector("[itemprop='longitude']")?.GetAttribute("content"),
                                        AskingPrice = GetNumber(info.GetAttribute("data-price")),
                                        Beds = GetNumber(info.GetAttribute("data-beds")),
                                        Sqft = GetNumber(prop.QuerySelector("[data-label='property-meta-sqft']")?.GetElementsByClassName("data-value").FirstOrDefault()?.TextContent),
                                        LotSize = GetNumber(prop.QuerySelector("[data-label='property-meta-lotsize']")?.GetElementsByClassName("data-value").FirstOrDefault()?.TextContent),
                                    };
                                    var fullBaths = GetNumber(info.GetAttribute("data-baths_full"));
                                    var halfBaths = GetNumber(info.GetAttribute("data-baths_half"));
                                    if (fullBaths != null && halfBaths != null)
                                    {
                                        newProp.Baths = (decimal)fullBaths + ((decimal)halfBaths * (decimal).5);
                                    }
                                    var status = prop.QuerySelector("[data-status]")?.GetAttribute("data-status");
                                    if (!string.IsNullOrEmpty(status))
                                    {
                                        if (!propertyStatuses.Any(x => x.Name == status))
                                        {
                                            _context.PropertyStatus.Add(new Data.Model.PropertyStatus() { Name = status });
                                            _context.SaveChanges();
                                            propertyStatuses = _context.PropertyStatus.AsNoTracking().ToList();
                                        }
                                        newProp.PropertyStatusId = propertyStatuses.FirstOrDefault(x => x.Name == status).Id;
                                    }
                                    gatheredData.Add(newProp);
                                }
                            }
                        }
                        else break;
                    }
                    pageNumber++;
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
                        if (!string.IsNullOrEmpty(property.RealtorUrl))
                        {
                            var url = $"https://www.realtor.com{property.RealtorUrl}";
                            property.RealtorUrl = url;
                            using (HttpResponseMessage response = await client.GetAsync(url))
                            {
                                if (response.IsSuccessStatusCode)
                                {
                                    using (HttpContent content = response.Content)
                                    {
                                        // ... Read the string.
                                        string pageHtml = await content.ReadAsStringAsync();
                                        var document = parser.Parse(pageHtml);
                                        var productID = document.QuerySelector("[itemprop='productID']")?.TextContent;
                                        property.SourcePropertyId = productID;
                                        var propType = document.QuerySelector("[name='prop_type']")?.GetAttribute("value");
                                        if (!string.IsNullOrEmpty(propType))
                                        {
                                            if (!propertyTypes.Any(x => x.Name == propType))
                                            {
                                                _context.PropertyType.Add(new Data.Model.PropertyType() { Name = propType });
                                                _context.SaveChanges();
                                                propertyTypes = _context.PropertyType.AsNoTracking().ToList();
                                            }
                                            property.PropertyTypeId = propertyTypes.First(x => x.Name == propType).Id;
                                        }
                                    }
                                }
                                else break;
                            }

                        }
                        //https://www.realtor.com/
                        _context.Property.Add(property);
                    }
                }
                _context.SaveChanges();
                return View();
            }
        }

        private decimal? GetDecimal(string value)
        {
            decimal val;
            if (decimal.TryParse(value, out val))
            {
                return val;
            }
            return null;
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