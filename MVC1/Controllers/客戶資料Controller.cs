using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC1.Models;
using System.Web.Security;
using PagedList;

namespace MVC1.Controllers
{
    public class 客戶資料Controller : BaseController
    {
        //private 客戶資料Entities db = new 客戶資料Entities();
        // GET: 客戶資料
        public ActionResult Index(string keyword, string Sort ,string Sidx, int page = 1)
        {
            var data = repoCustInfo.All();

            //查詢
            if (!string.IsNullOrEmpty(keyword))
            {                
                data = repoCustInfo.Search(data, keyword);
            }

            //排序
            if (!string.IsNullOrEmpty(Sort))
            {
                data = data.OrderBy(Sort + " " + Sidx);
            }
            else
            {
                data = data.OrderBy(p=>p.Id);
            }

            //分頁
            int pageSize = 3;
            return View(data.ToPagedList(page, pageSize));
        }

        // GET: 客戶資料/Details/5
        [HandleError(ExceptionType = typeof(InvalidOperationException), View = "Error2")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                throw new ArgumentException("參數錯誤");
            }
            客戶資料 客戶資料 = repoCustInfo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // GET: 客戶資料/Create
        public ActionResult Create()
        {
            ViewBag.Client = repoCustInfo.GetCustClass();
            return View();
        }

        // POST: 客戶資料/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類,帳號,密碼")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                repoCustInfo.Add(客戶資料);
                repoCustInfo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            return View(客戶資料);
        }
        // GET: 客戶資料/Edit/5
        [OverrideAuthorization()]
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repoCustInfo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            ViewBag.Client = repoCustInfo.GetCustClass();
            return View(客戶資料);
        }

        // POST: 客戶資料/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [OverrideAuthorization()]
        [Authorize]
        public ActionResult Edit(int id, FormCollection form)
        {
            var 客戶資料 = repoCustInfo.Find(id);
            if (TryUpdateModel(客戶資料, "客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類,帳號,密碼".Split(',')))
            {
                //var db客戶資料 = (客戶資料Entities)repoCustInfo.UnitOfWork.Context;
                //db客戶資料.Entry(客戶資料).State = EntityState.Modified;
                客戶資料.密碼 = FormsAuthentication.HashPasswordForStoringInConfigFile(string.Format("{0}{1}",客戶資料.帳號,客戶資料.密碼), "SHA1");
                repoCustInfo.UnitOfWork.Commit();
                if (!User.IsInRole("sysadmin"))
                {
                    return View(客戶資料);
                }
                TempData["CustInfoSuccessMsg"] = 客戶資料.客戶名稱 + "更新成功";
                return RedirectToAction("Index");
            }

            return View(客戶資料);
        }

        // GET: 客戶資料/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repoCustInfo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }

            return View(客戶資料);
        }

        // POST: 客戶資料/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶資料 客戶資料 = repoCustInfo.Find(id);
            repoCustInfo.Delete(客戶資料);
            repoCustInfo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }
        public ActionResult ExportExcel(string keyword)
        {
            byte[] file = repoCustInfo.Export(keyword);
            return File(file, "application/vnd.ms-excel", "客戶資料.xls");
        }

        public ActionResult _CustContactPartial(int id)
        {
            var data = repoCustContact.All(false).Where(p => p.客戶Id == id).ToList();
            return PartialView(data);
        }
        public ActionResult UpdatePartail(IList<客戶聯絡人> data)
        {
            if (ModelState.IsValid && data != null)
            {
                foreach (var item in data)
                {
                    var contactdata = repoCustContact.Find(item.Id);
                    contactdata.職稱 = item.職稱;
                    contactdata.手機 = item.手機;
                    contactdata.電話 = item.電話;
                }
                repoCustContact.UnitOfWork.Commit();
                return RedirectToAction("Delete", new { id= data[0].客戶Id });
            }
            
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repoCustInfo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
