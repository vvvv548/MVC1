using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC1.Controllers
{
    public class 計算Action的執行時間Attribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.dtStart = DateTime.Now;
            base.OnActionExecuting(filterContext);
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            filterContext.Controller.ViewBag.dtEnd = DateTime.Now;
            var timespan = (DateTime)filterContext.Controller.ViewBag.dtEnd -
                (DateTime)filterContext.Controller.ViewBag.dtStart;
            filterContext.Controller.ViewBag.timespan = timespan;
            string msg = string.Format("Action執行時間為：{0}", timespan);
            System.Diagnostics.Debug.WriteLine(msg);

            base.OnActionExecuted(filterContext);
        }
    }
}