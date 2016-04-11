using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC1.Models;
using PagedList;

namespace MVC1.Controllers
{
    public class 客戶聯絡人Controller : BaseController
    {
        private 客戶資料Entities db = new 客戶資料Entities();
        private int pageSize = 3;
        // GET: 客戶聯絡人
        public ActionResult Index(int page = 1)
        {
            var data = repoCustContact.PagedList(page);
            ViewBag.JobTitle = data.Select(p => new SelectListItem() { Value = p.職稱, Text = p.職稱 }).Distinct().ToList();
            return View(data);
        }

        [HttpPost]
        public ActionResult Index(string name, int page = 1)
        {
            var data = repoCustContact.Search(name,string.Empty);
            var result = repoCustContact.PagedList(data, page);
            ViewBag.JobTitle = result.Select(p => new SelectListItem() { Value = p.職稱, Text = p.職稱 }).Distinct().ToList();
            if (!string.IsNullOrEmpty(name))
            {
                ViewBag.keyword = name;
            }


            return View(result);
        }
        public ActionResult Filter(string keyword,string jobtitle,int page) 
        {
            var data = repoCustContact.Search(keyword,jobtitle);
            ViewBag.JobTitle = repoCustContact.All(false).Select(p => new SelectListItem() { Value = p.職稱, Text = p.職稱 }).Distinct().ToList();
            var result = repoCustContact.PagedList(data, page);
            if (!string.IsNullOrEmpty(keyword))
            {
                ViewBag.keyword = keyword;
            }
                        
            return PartialView("ContantPartial", result);
        }
        // GET: 客戶聯絡人/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = repoCustContact.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Create
        public ActionResult Create()
        {
            ViewBag.Client = new SelectList(repoCustInfo.All(false).Select(p => new { Id = p.Id, 客戶名稱 = p.客戶名稱 }), "Id", "客戶名稱");
            return View();
        }

        // POST: 客戶聯絡人/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                repoCustContact.Add(客戶聯絡人);
                repoCustContact.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.Client = new SelectList(repoCustInfo.All(false).Select(p => new { Id = p.Id, 客戶名稱 = p.客戶名稱 }), "Id", "客戶名稱");
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = repoCustContact.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            ViewBag.Client = new SelectList(repoCustInfo.All(false).Select(p => new { Id = p.Id, 客戶名稱 = p.客戶名稱 }), "Id", "客戶名稱");
            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話,是否已刪除")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                var db客戶聯絡人 = (客戶資料Entities)repoCustContact.UnitOfWork.Context;
                db客戶聯絡人.Entry(客戶聯絡人).State = EntityState.Modified;
                repoCustInfo.UnitOfWork.Commit();
                TempData["CustContactSuccessMsg"] = 客戶聯絡人.姓名 + "更新成功";
                return RedirectToAction("Index");
            }
            ViewBag.Client = new SelectList(repoCustInfo.All(false).Select(p => new { Id = p.Id, 客戶名稱 = p.客戶名稱 }), "Id", "客戶名稱");
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = repoCustContact.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶聯絡人 客戶聯絡人 = repoCustContact.Find(id);
            repoCustContact.Delete(客戶聯絡人);
            repoCustContact.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        public ActionResult ExportExcel()
        {
            return File(repoCustContact.ExportXLS(repoCustContact.All(false)), "application/vnd.ms-excel", "客戶聯絡人.xls");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repoCustContact.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
