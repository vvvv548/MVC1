using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using PagedList;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;

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
        /// <summary>
        /// 查詢
        /// </summary>
        /// <param name="data"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public IQueryable<客戶資料> Search(IQueryable<客戶資料> data,string keyword)
        {
            return data.Where(p => p.客戶名稱.Contains(keyword) ||
                       p.帳號.Contains(keyword) ||
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
        /// <summary>
        /// 匯出EXCEL功能 by NPOI
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public byte[] Export(string keyword)
        {
            var data = this.All();
            if (!string.IsNullOrEmpty(keyword))
            {
                data = this.Search(data, keyword);
            }
            //記憶體中創建一個空的2003 Excel檔
            HSSFWorkbook workbook = new HSSFWorkbook();
            //建立Sheet頁面
            ISheet sheet = workbook.CreateSheet("客戶資料");
            //建立標題列
            sheet.CreateRow(0);
            //寫入欄位標題
            sheet.GetRow(0).CreateCell(0).SetCellValue("客戶分類");
            sheet.GetRow(0).CreateCell(1).SetCellValue("帳號");
            sheet.GetRow(0).CreateCell(2).SetCellValue("客戶名稱");
            sheet.GetRow(0).CreateCell(3).SetCellValue("統一編號");
            sheet.GetRow(0).CreateCell(4).SetCellValue("電話");
            sheet.GetRow(0).CreateCell(5).SetCellValue("傳真");
            sheet.GetRow(0).CreateCell(6).SetCellValue("地址");
            sheet.GetRow(0).CreateCell(7).SetCellValue("Email");

            //寫入欄位內容
            var result = data.OrderBy(p => p.Id).ToArray();
            for (int i = 1; i <= result.Count(); i++)
            {
                sheet.CreateRow(i);
                sheet.GetRow(i).CreateCell(0).SetCellValue(result[i-1].客戶分類);
                sheet.GetRow(i).CreateCell(1).SetCellValue(result[i-1].帳號);
                sheet.GetRow(i).CreateCell(2).SetCellValue(result[i-1].客戶名稱);
                sheet.GetRow(i).CreateCell(3).SetCellValue(result[i-1].統一編號);
                sheet.GetRow(i).CreateCell(4).SetCellValue(result[i-1].電話);
                sheet.GetRow(i).CreateCell(5).SetCellValue(result[i-1].傳真);
                sheet.GetRow(i).CreateCell(6).SetCellValue(result[i-1].地址);
                sheet.GetRow(i).CreateCell(7).SetCellValue(result[i-1].Email);
            }
            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Close();
            return ms.ToArray();
        }
    }

    public interface I客戶資料Repository : IRepository<客戶資料>
    {

    }
}