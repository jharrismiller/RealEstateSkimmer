using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            _context.Property.Add(new Data.Model.Property() { Address = "TEST" });
            _context.SaveChanges();
            return View();
        }
    }
}