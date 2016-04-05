using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MVC1.Models
{   
	public  class 客戶銀行資訊Repository : EFRepository<客戶銀行資訊>, I客戶銀行資訊Repository
	{
        public override IQueryable<客戶銀行資訊> All()
        {
            return base.All().Where(p=>p.是否已刪除==false);
        }
        public IQueryable<客戶銀行資訊> All(bool isAll)
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
        public override void Add(客戶銀行資訊 entity)
        {
            entity.是否已刪除 = false;
            base.Add(entity);
        }
        public IQueryable<客戶銀行資訊> Search(string keyword)
        {
            return this.All()
                       .Where(p => p.客戶資料.客戶名稱.Contains(keyword) ||
                              p.銀行名稱.ToString().Contains(keyword) ||
                              p.銀行代碼.ToString().Contains(keyword) ||
                              p.帳戶名稱.ToString().Contains(keyword) ||
                              p.帳戶號碼.ToString().Contains(keyword) ||
                              p.分行代碼.ToString().Contains(keyword));
        }
        public 客戶銀行資訊 Find(int id)
        {
            return this.All().FirstOrDefault(p => p.Id == id);
        }
        public override void Delete(客戶銀行資訊 entity)
        {
            entity.是否已刪除 = false;
        }
    }

	public  interface I客戶銀行資訊Repository : IRepository<客戶銀行資訊>
	{

	}
}