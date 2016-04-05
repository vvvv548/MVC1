using MVC1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC1.Controllers
{
    public abstract class BaseController : Controller
    {
        protected 客戶資料Repository repoCustInfo = RepositoryHelper.Get客戶資料Repository();
        protected 客戶聯絡人Repository repoCustContact = RepositoryHelper.Get客戶聯絡人Repository();
        protected 客戶銀行資訊Repository repoCustBank = RepositoryHelper.Get客戶銀行資訊Repository();
        // GET: Base
        protected override void HandleUnknownAction(string actionName)
        {
            this.RedirectToAction("Index", "Home")
                .ExecuteResult(this.ControllerContext);
        }
    }
}