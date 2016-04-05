using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace MVC1.Models
{
    public class ExcelRepository<T>
    {
        public virtual byte[] ExportXLS(IQueryable<T> entities, params string[] notExportCol)
        {
            var coltitle = GetColumnTitle(entities.ElementType, notExportCol);
            if (coltitle.Count() > 0)
            {
                int i = 0, j = 0;
                HSSFWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet(entities.ElementType.Name);
                IRow row = sheet.CreateRow(i);
                foreach (var col in coltitle)
                {
                    row.CreateCell(j).SetCellValue(col.DisplayName);
                    j++;
                }

                foreach (var item in entities)
                {
                    i++;
                    j = 0;
                    row = sheet.CreateRow(i);
                    PropertyInfo[] ps = item.GetType().GetProperties();
                    foreach (var col in coltitle)
                    {
                        if (ps.Any(p => p.Name == col.Name))
                        {
                            var value = ps.Where(p => p.Name == col.Name).First().GetValue(item);
                            if (!string.IsNullOrEmpty(col.Description))
                            {
                                var Description = col.Description.Split('.');
                                if (Description[0] == "FK")
                                {
                                    var obj = ps.Where(p => p.Name == Description[1]).First().GetValue(item);
                                    value = obj.GetType().GetProperties().Where(p => p.Name == Description[2]).First().GetValue(obj);
                                }
                                /*
                                else if(Description[0] == "CodeConfig")
                                {
                                    value = ps.Where(p => p.Name == col.Name).First().GetValue(item);
                                    if (value != null) {
                                        var context = new EFUnitOfWork().Context;
                                        var configcode = context.Database.SqlQuery<ConfigCode>("Select * from ConfigCode Where CodeType = @param1 and CodeKey = @param2",
                                            new SqlParameter("param1", Description[1]),
                                            new SqlParameter("param2", value.ToString())).FirstOrDefault();
                                        value = configcode.GetType().GetProperties().Where(p => p.Name == Description[2]).First().GetValue(configcode);
                                    }
                                }*/
                            }

                            if (value == null)
                            {
                                row.CreateCell(j).SetCellValue("");
                            }
                            else
                            {
                                row.CreateCell(j).SetCellValue(value.ToString());
                            }
                            j++;
                        }
                    }
                }
                MemoryStream ms = new MemoryStream();
                workbook.Write(ms);
                return ms.ToArray();
            }
            return null;
        }

        public virtual byte[] ExportXLSX(IQueryable<T> entities, params string[] notExportCol)
        {
            var coltitle = GetColumnTitle(entities.ElementType, notExportCol);
            if (coltitle.Count() > 0)
            {
                int i = 0, j = 0;
                XSSFWorkbook workbook = new XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet(entities.ElementType.Name);
                IRow row = sheet.CreateRow(i);
                foreach (var col in coltitle)
                {
                    row.CreateCell(j).SetCellValue(col.DisplayName);
                    j++;
                }

                foreach (var item in entities)
                {
                    i++;
                    j = 0;
                    row = sheet.CreateRow(i);
                    PropertyInfo[] ps = item.GetType().GetProperties();
                    foreach (var col in coltitle)
                    {
                        if (ps.Any(p => p.Name == col.Name))
                        {
                            var value = ps.Where(p => p.Name == col.Name).First().GetValue(item);
                            if (!string.IsNullOrEmpty(col.Description))
                            {
                                var Description = col.Description.Split('.');
                                if (Description[0] == "FK")
                                {
                                    var obj = ps.Where(p => p.Name == Description[1]).First().GetValue(item);
                                    value = obj.GetType().GetProperties().Where(p => p.Name == Description[2]).First().GetValue(obj);
                                }
                                /*
                                else if (Description[0] == "CodeConfig")
                                {
                                    value = ps.Where(p => p.Name == col.Name).First().GetValue(item);
                                    if (value != null)
                                    {
                                        var context = new EFUnitOfWork().Context;
                                        var configcode = context.Database.SqlQuery<ConfigCode>("Select * from ConfigCode Where CodeType = @param1 and CodeKey = @param2",
                                            new SqlParameter("param1", Description[1]),
                                            new SqlParameter("param2", value.ToString())).FirstOrDefault();
                                        value = configcode.GetType().GetProperties().Where(p => p.Name == Description[2]).First().GetValue(configcode);
                                    }
                                }*/
                            }

                            if (value == null)
                            {
                                row.CreateCell(j).SetCellValue("");
                            }
                            else
                            {
                                row.CreateCell(j).SetCellValue(value.ToString());
                            }
                            j++;
                        }
                    }
                }
                MemoryStream ms = new MemoryStream();
                workbook.Write(ms);
                return ms.ToArray();
            }
            return null;
        }

        #region 私有方法
        private List<ColumnTitle> GetColumnTitle(Type entityType, params string[] notExportCol)
        {
            var coltitle = new List<ColumnTitle>();

            var Properties = entityType.GetProperties().Where(p => p.SetMethod.Attributes == (MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName)).ToList();
            foreach (var prop in Properties)
            {
                if (!notExportCol.Any(p => p == prop.Name))
                {
                    var attr = (DisplayAttribute)entityType.GetProperty(prop.Name).GetCustomAttributes(typeof(DisplayAttribute), true).SingleOrDefault();
                    if (attr == null)
                    {
                        MetadataTypeAttribute metadataType = (MetadataTypeAttribute)entityType.GetCustomAttributes(typeof(MetadataTypeAttribute), true).FirstOrDefault();
                        if (metadataType != null)
                        {
                            var property = metadataType.MetadataClassType.GetProperty(prop.Name);
                            if (property != null)
                            {
                                attr = (DisplayAttribute)property.GetCustomAttributes(typeof(DisplayAttribute), true).SingleOrDefault();
                            }
                        }
                    }

                    var column = new ColumnTitle();
                    if (attr == null)
                    {
                        column = new ColumnTitle()
                        {
                            Name = prop.Name,
                            DisplayName = prop.Name
                        };
                        coltitle.Add(column);
                    }
                    else if (!notExportCol.Any(p => p == attr.Name))
                    {
                        column = new ColumnTitle()
                        {
                            Name = prop.Name,
                            DisplayName = attr.GetName() ?? prop.Name,
                            ShortName = attr.GetShortName(),
                            Description = attr.GetDescription(),
                            GroupName = attr.GetGroupName(),
                            Prompt = attr.GetPrompt(),
                            Order = attr.GetOrder()
                        };
                        coltitle.Add(column);
                    }
                }
            }
            if (coltitle.Any(p => p.Order == null))
            {
                return coltitle;
            }
            else
            {
                return coltitle.OrderBy(p => p.Order).ToList();
            }
        }

        private class ColumnTitle
        {
            public string Name { get; set; }

            public string DisplayName { get; set; }

            public string ShortName { get; set; }

            public string Description { get; set; }

            public string GroupName { get; set; }

            public string Prompt { get; set; }

            public int? Order { get; set; }
        }
        #endregion
    }
}