using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC1.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index(int? Number)
        {
            if (Number.HasValue)
            {
                ViewBag.Result = Number * 2;
            }
            return View();
        }        
    }
}