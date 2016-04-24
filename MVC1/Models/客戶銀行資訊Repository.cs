using System;
using System.Linq;
using System.Collections.Generic;
using PagedList;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace MVC1.Models
{
    public class 客戶銀行資訊Repository : EFRepository<客戶銀行資訊>, I客戶銀行資訊Repository
    {
        public override IQueryable<客戶銀行資訊> All()
        {
            return base.All().Where(p => p.是否已刪除 == false);
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
        /// <summary>
        /// 查詢
        /// </summary>
        /// <param name="data"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public IQueryable<客戶銀行資訊> Search(IQueryable<客戶銀行資訊> data, string keyword)
        {
            return data.Where(p => p.客戶資料.客戶名稱.Contains(keyword) ||
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
            sheet.GetRow(0).CreateCell(1).SetCellValue("銀行名稱");
            sheet.GetRow(0).CreateCell(2).SetCellValue("銀行代碼");
            sheet.GetRow(0).CreateCell(3).SetCellValue("分行代碼");
            sheet.GetRow(0).CreateCell(4).SetCellValue("帳戶名稱");
            sheet.GetRow(0).CreateCell(5).SetCellValue("帳戶號碼");
            var result = data.OrderBy(p => p.Id).ToArray();
            for (int i = 1; i <= result.Count(); i++)
            {
                sheet.CreateRow(i);
                sheet.GetRow(i).CreateCell(0).SetCellValue(result[i - 1].客戶資料.客戶名稱);
                sheet.GetRow(i).CreateCell(1).SetCellValue(result[i - 1].銀行名稱);
                sheet.GetRow(i).CreateCell(2).SetCellValue(result[i - 1].銀行代碼);
                sheet.GetRow(i).CreateCell(3).SetCellValue(result[i - 1].分行代碼.Value);
                sheet.GetRow(i).CreateCell(4).SetCellValue(result[i - 1].帳戶名稱);
                sheet.GetRow(i).CreateCell(5).SetCellValue(result[i - 1].帳戶號碼);
            }

            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Close();
            return ms.ToArray();
        }
    }

    public interface I客戶銀行資訊Repository : IRepository<客戶銀行資訊>
    {

    }
}