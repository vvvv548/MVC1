using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using PagedList;

namespace MVC1.Models
{
    public class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
    {
        public override IQueryable<客戶資料> All()
        {
            return base.All().Where(p => p.是否已刪除 == false);
        }
        public IQueryable<客戶資料> All(bool isAll)
        {
            if (isAll)
            {
                return base.All();
            }
            else
            {
                return this.All();
            }
        }
        public override void Add(客戶資料 entity)
        {
            entity.是否已刪除 = false;
            entity.密碼= FormsAuthentication.HashPasswordForStoringInConfigFile(entity.密碼, "SHA1");
            base.Add(entity);
        }

        public IQueryable<客戶資料> Search(string keyword)
        {
            return this.All().Where(p => p.客戶名稱.Contains(keyword) ||
                       p.統一編號.Contains(keyword) ||
                       p.電話.Contains(keyword) ||
                       p.傳真.Contains(keyword) ||
                       p.地址.Contains(keyword) ||
                       p.Email.Contains(keyword) ||
                       p.客戶分類.Contains(keyword));
        }
        public 客戶資料 Find(int id)
        {
            return this.All().FirstOrDefault(p => p.Id == id);
        }
        public IPagedList<客戶資料> PagedList(int page)
        {
            int currentPage = page < 1 ? 1 : page;
            int pageSize = 3;
            var data = this.All().OrderBy(p => p.Id)
                .ToPagedList(currentPage,pageSize);
            return data; 
        }

        public IPagedList<客戶資料> PagedList(IQueryable<客戶資料> data, int page)
        {
            int currentPage = page < 1 ? 1 : page;
            int pageSize = 3;
            return data.OrderBy(p => p.Id).ToPagedList(currentPage, pageSize);
        }
        public override void Delete(客戶資料 entity)
        {
            entity.是否已刪除 = true;
        }
        public List<SelectListItem> GetCustClass()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "高級客戶", Value = "高級客戶" });
            items.Add(new SelectListItem() { Text = "中級客戶", Value = "中級客戶" });
            items.Add(new SelectListItem() { Text = "低級客戶", Value = "低級客戶" });
            return items;
        }
    }

    public interface I客戶資料Repository : IRepository<客戶資料>
    {

    }
}