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
    public class 客戶銀行資訊Controller : BaseController
    {
        //private 客戶資料Entities db = new 客戶資料Entities();

        // GET: 客戶銀行資訊
        public ActionResult Index()
        {
            var data = repoCustBank.All(false).ToList();
            return View(data);
        }
        [HttpPost]
        public ActionResult Index(string name)
        {
            return View(repoCustBank.Search(name).ToList());
        }

        // GET: 客戶銀行資訊/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = repoCustBank.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            return View(客戶銀行資訊);
        }

        // GET: 客戶銀行資訊/Create
        public ActionResult Create()
        {
            ViewBag.Client = new SelectList(repoCustInfo.All(false).Select(p => new { Id = p.Id, 客戶名稱 = p.客戶名稱 }), "Id", "客戶名稱");
            return View();
        }

        // POST: 客戶銀行資訊/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼")] 客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                repoCustBank.Add(客戶銀行資訊);
                repoCustBank.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.Client = new SelectList(repoCustInfo.All(false).Select(p => new { Id = p.Id, 客戶名稱 = p.客戶名稱 }), "Id", "客戶名稱");
            return View(客戶銀行資訊);
        }

        // GET: 客戶銀行資訊/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = repoCustBank.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            ViewBag.Client = new SelectList(repoCustInfo.All(false).Select(p => new { Id = p.Id, 客戶名稱 = p.客戶名稱 }), "Id", "客戶名稱");
            return View(客戶銀行資訊);
        }

        // POST: 客戶銀行資訊/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼,是否已刪除")] 客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                var db客戶銀行資訊 = (客戶資料Entities)repoCustBank.UnitOfWork.Context;
                db客戶銀行資訊.Entry(客戶銀行資訊).State = EntityState.Modified;
                repoCustBank.UnitOfWork.Commit();
                TempData["CustBankSuccessMsg"] = 客戶銀行資訊.銀行名稱+ "更新成功";
                return RedirectToAction("Index");
            }
            ViewBag.Client = new SelectList(repoCustInfo.All(false).Select(p => new { Id = p.Id, 客戶名稱 = p.客戶名稱 }), "Id", "客戶名稱");
            return View(客戶銀行資訊);
        }

        // GET: 客戶銀行資訊/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = repoCustBank.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            return View(客戶銀行資訊);
        }

        // POST: 客戶銀行資訊/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶銀行資訊 客戶銀行資訊 = repoCustBank.Find(id);
            客戶銀行資訊.是否已刪除 = true;
            repoCustBank.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        public ActionResult ExportExcel()
        {
            return File(repoCustBank.ExportXLS(repoCustBank.All(false)), "application/vnd.ms-excel", "客戶銀行資訊.xls");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repoCustBank.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
