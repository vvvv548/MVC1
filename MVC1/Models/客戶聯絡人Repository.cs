using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using PagedList;

namespace MVC1.Models
{
    public class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
    {
        public override IQueryable<客戶聯絡人> All()
        {
            return base.All().Where(p => p.是否已刪除 == false);
        }

        public IQueryable<客戶聯絡人> All(bool isAll)
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


        public IQueryable<客戶聯絡人> Search(string keyword,string jobtitle)
        {
            if (string.IsNullOrEmpty(jobtitle))
            {
                return this.All().Where(p => p.客戶資料.客戶名稱.Contains(keyword) ||
                       p.姓名.Contains(keyword) ||
                       p.職稱.Contains(keyword) ||
                       p.電話.Contains(keyword) ||
                       p.手機.Contains(keyword) ||
                       p.電話.Contains(keyword) ||
                       p.Email.Contains(keyword));
            }
            else
            {
                return this.All().Where(p => p.客戶資料.客戶名稱.Contains(keyword) ||
                       p.姓名.Contains(keyword) ||
                       p.職稱.Contains(keyword) ||
                       p.電話.Contains(keyword) ||
                       p.手機.Contains(keyword) ||
                       p.電話.Contains(keyword) ||
                       p.Email.Contains(keyword))
                       .Where(p=>p.職稱==jobtitle);
            }
            
        }
        public override void Add(客戶聯絡人 entity)
        {
            entity.是否已刪除 = false;
            base.Add(entity);
        }
        public 客戶聯絡人 Find(int id)
        {
            return this.All().FirstOrDefault(p => p.Id == id);
        }
        public IPagedList<客戶聯絡人> PagedList(int page)
        {
            int currentPage = page < 1 ? 1 : page;
            int pageSize = 3;
            var data = this.All().OrderBy(p => p.Id)
                .ToPagedList(currentPage, pageSize);
            return data;
        }

        public IPagedList<客戶聯絡人> PagedList(IQueryable<客戶聯絡人> data, int page)
        {
            int currentPage = page < 1 ? 1 : page;
            int pageSize = 3;
            return data.OrderBy(p => p.客戶Id).ToPagedList(currentPage, pageSize);
        }
        public IQueryable<客戶聯絡人> Filter(IQueryable<客戶聯絡人> data, string jobtitle)
        {
            if (string.IsNullOrEmpty(jobtitle)){
                return data;
            }
            else
            {
                return data.Where(p => p.職稱 == jobtitle);
            }
            
        }
        public override void Delete(客戶聯絡人 entity)
        {
            entity.是否已刪除 = true;
        }
    }

    public interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
    {

    }
}