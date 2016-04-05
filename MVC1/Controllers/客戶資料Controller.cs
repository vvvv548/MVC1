using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC1.Models;

namespace MVC1.Controllers
{
    public class 客戶資料Controller : BaseController
    {
        //private 客戶資料Entities db = new 客戶資料Entities();

        // GET: 客戶資料
        public ActionResult Index()
        {
            var data = repoCustInfo.All(false).ToList();
            return View(data);
        }

        [HttpPost]
        public ActionResult Index(string name)
        {
            var data = repoCustInfo.Search(name).ToList();
            return View(data);
        }

        // GET: 客戶資料/Details/5
        public ActionResult Details(int? id)
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
        public ActionResult Create([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶分類")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                repoCustInfo.Add(客戶資料);
                repoCustInfo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            ViewBag.Client = repoCustInfo.GetCustClass();
            return View(客戶資料);
        }

        // GET: 客戶資料/Edit/5
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
        public ActionResult Edit([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,是否已刪除,客戶分類")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                var db客戶資料 = (客戶資料Entities)repoCustInfo.UnitOfWork.Context;
                db客戶資料.Entry(客戶資料).State = EntityState.Modified;
                repoCustInfo.UnitOfWork.Commit();
                TempData["CustInfoSuccessMsg"] = 客戶資料.客戶名稱 + "更新成功";
                return RedirectToAction("Index");
            }
            ViewBag.Client = repoCustInfo.GetCustClass();
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
        public ActionResult ExportExcel()
        {
           return File(repoCustInfo.ExportXLS(repoCustInfo.All(false)), "application/vnd.ms-excel", "客戶資料.xls");            
        }

        public ActionResult _CustContactPartial(int id)
        {
            var data = repoCustContact.All(false).Where(p => p.客戶Id == id).ToList();
            return PartialView(data);
        }
        [HttpPost]
        public ActionResult _CustContactPartial(IList<客戶聯絡人> data)
        {
            if (ModelState.IsValid &&　data != null)
            {
                int custId = -1;
                
                foreach (var item in data)
                {
                    var contactdata = repoCustContact.Find(item.Id);
                    contactdata.職稱 = item.職稱;
                    contactdata.手機 = item.手機;
                    contactdata.電話 = item.電話;
                    custId = item.客戶Id;
                }
                repoCustContact.UnitOfWork.Commit();
                return View(repoCustContact.All(false).Where(p => p.客戶Id == custId));
            }
            
            return View();
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
