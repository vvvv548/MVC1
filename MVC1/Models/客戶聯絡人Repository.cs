using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using PagedList;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;

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
        /// <summary>
        /// 查詢
        /// </summary>
        /// <param name="data"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public IQueryable<客戶聯絡人> Search(IQueryable<客戶聯絡人> data, string keyword)
        {
            return data.Where(p => p.客戶資料.客戶名稱.Contains(keyword) ||
                         p.姓名.Contains(keyword) ||
                         p.職稱.Contains(keyword) ||
                         p.電話.Contains(keyword) ||
                         p.手機.Contains(keyword) ||
                         p.電話.Contains(keyword) ||
                         p.Email.Contains(keyword));
        }
        /// <summary>
        /// 職稱篩選
        /// </summary>
        /// <param name="data"></param>
        /// <param name="jobTitle"></param>
        /// <returns></returns>
        public IQueryable<客戶聯絡人> Filter(IQueryable<客戶聯絡人> data, string jobTitle)
        {
            data = data.Where(p => p.職稱 == jobTitle);
            return data;
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
            ISheet sheet = workbook.CreateSheet("客戶銀行資訊");
            // 建立標題列
            sheet.CreateRow(0);
            // 設定表頭資料
            sheet.GetRow(0).CreateCell(0).SetCellValue("客戶名稱");
            sheet.GetRow(0).CreateCell(1).SetCellValue("姓名");
            sheet.GetRow(0).CreateCell(2).SetCellValue("職稱");
            sheet.GetRow(0).CreateCell(3).SetCellValue("電話");
            sheet.GetRow(0).CreateCell(4).SetCellValue("手機");
            sheet.GetRow(0).CreateCell(5).SetCellValue("電話");
            sheet.GetRow(0).CreateCell(6).SetCellValue("Email");

            var result = data.OrderBy(p => p.客戶Id).ToArray();
            for (int i = 1; i <= result.Count(); i++)
            {
                sheet.CreateRow(i);
                sheet.GetRow(i).CreateCell(0).SetCellValue(result[i - 1].客戶資料.客戶名稱);
                sheet.GetRow(i).CreateCell(1).SetCellValue(result[i - 1].姓名);
                sheet.GetRow(i).CreateCell(2).SetCellValue(result[i - 1].職稱);
                sheet.GetRow(i).CreateCell(3).SetCellValue(result[i - 1].電話);
                sheet.GetRow(i).CreateCell(4).SetCellValue(result[i - 1].手機);
                sheet.GetRow(i).CreateCell(5).SetCellValue(result[i - 1].電話);
                sheet.GetRow(i).CreateCell(6).SetCellValue(result[i - 1].Email);
            }

            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Close();
            return ms.ToArray();
        }
    }

    public interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
    {

    }
}