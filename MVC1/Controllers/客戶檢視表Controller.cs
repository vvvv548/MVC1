using MVC1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC1.Controllers
{
    public class 客戶檢視表Controller : Controller
    {
        private 客戶資料Entities db = new 客戶資料Entities();
        // GET: 客戶檢視表
        public ActionResult Index()
        {            
            return View(db.客戶檢視表.ToList());
        }
    }
}