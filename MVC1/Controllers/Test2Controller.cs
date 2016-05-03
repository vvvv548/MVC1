using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC1.Controllers
{
    public class Test2Controller : Controller
    {
        // GET: Test2
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(int? input)
        {
            if (input.HasValue)
            {
                ViewBag.Result = input.Value * 2;

            }
            return View();
        }
    }
}