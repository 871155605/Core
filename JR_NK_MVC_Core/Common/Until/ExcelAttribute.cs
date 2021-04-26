using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace JR_NK_MVC_Core.Common.until
{
    /// <summary>
    /// excel转object
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcelAttribute : Attribute
    {
        public ExcelAttribute(string name)
        {
            Title = name;
        }

        public int Order { get; set; }
        public string Title { get; set; }
    }
    public class ExcelImporter
    {
        public IEnumerable<TModel> ExcelToObject<TModel>(string path, int? type = null) where TModel : class, new()
        {
            var result = GetDataRows(path);
            var dict = ExcelUtil.GetExportAttrDict<TModel>();
            var dictColumns = new Dictionary<int, KeyValuePair<PropertyInfo, ExcelAttribute>>();

            IEnumerator rows = result;
            var titleRow = (IRow)rows.Current;
            if (titleRow != null)
                foreach (var cell in titleRow.Cells)
                {
                    var prop = new KeyValuePair<PropertyInfo, ExcelAttribute>();
                    foreach (var item in dict)
                    {
                        if (cell.StringCellValue == item.Value.Title)
                        {
                            prop = item;
                        }
                    }

                    if (prop.Key != null && !dictColumns.ContainsKey(cell.ColumnIndex))
                    {
                        dictColumns.Add(cell.ColumnIndex, prop);
                    }
                }
            while (rows.MoveNext())
            {
                var row = (IRow)rows.Current;
                if (row != null)
                {
                    var firstCell = row.GetCell(0);
                    if (firstCell == null || firstCell.CellType == CellType.Blank ||
                        string.IsNullOrEmpty(firstCell.ToString()))
                        continue;
                }

                var model = new TModel();
                foreach (var pair in dictColumns)
                {
                    var propType = pair.Value.Key.PropertyType;
                    if (propType == typeof(DateTime?) ||
                        propType == typeof(DateTime))
                    {
                        pair.Value.Key.SetValue(model, GetCellDateTime(row, pair.Key), null);
                    }
                    else
                    {

                        try
                        {
                            var val = Convert.ChangeType(GetCellValue(row, pair.Key), propType);
                            pair.Value.Key.SetValue(model, val, null);
                        }
                        catch (Exception ex)
                        {
                            break;
                        }
                    }
                }
                yield return model;
            }

        }
        string GetCellValue(IRow row, int index)
        {
            var result = string.Empty;
            try
            {
                switch (row.GetCell(index).CellType)
                {
                    case CellType.Numeric:
                        result = row.GetCell(index).NumericCellValue.ToString();
                        break;
                    case CellType.String:
                        result = row.GetCell(index).StringCellValue;
                        break;
                    case CellType.Blank:
                        result = string.Empty;
                        break;

                    #region

                    //case CellType.Formula:
                    //    result = row.GetCell(index).CellFormula;
                    //    break;
                    //case CellType.Boolean:
                    //    result = row.GetCell(index).NumericCellValue.ToString();
                    //    break;
                    //case CellType.Error:
                    //    result = row.GetCell(index).NumericCellValue.ToString();
                    //    break;
                    //case CellType.Unknown:
                    //    result = row.GetCell(index).NumericCellValue.ToString();
                    //    break;

                    #endregion
                    default:
                        result = row.GetCell(index).ToString();
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return (result ?? "").Trim();
        }
        IEnumerator GetDataRows(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;
            HSSFWorkbook hssfworkbook;
            try
            {
                using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    hssfworkbook = new HSSFWorkbook(file);
                }
            }
            catch (Exception)
            {
                return null;
            }
            ISheet sheet = hssfworkbook.GetSheetAt(0);
            IEnumerator rows = sheet.GetRowEnumerator();
            rows.MoveNext();
            return rows;
        }
        DateTime? GetCellDateTime(IRow row, int index)
        {
            DateTime? result = null;
            try
            {
                switch (row.GetCell(index).CellType)
                {
                    case CellType.Numeric:
                        try
                        {
                            result = row.GetCell(index).DateCellValue;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        break;
                    case CellType.String:
                        var str = row.GetCell(index).StringCellValue;
                        if (str.EndsWith("年"))
                        {
                            DateTime dt;
                            if (DateTime.TryParse((str + "-01-01").Replace("年", ""), out dt))
                            {
                                result = dt;
                            }
                        }
                        else if (str.EndsWith("月"))
                        {
                            DateTime dt;
                            if (DateTime.TryParse((str + "-01").Replace("年", "").Replace("月", ""), out dt))
                            {
                                result = dt;
                            }
                        }
                        else if (!str.Contains("年") && !str.Contains("月") && !str.Contains("日"))
                        {
                            try
                            {
                                result = Convert.ToDateTime(str);
                            }
                            catch (Exception)
                            {
                                try
                                {
                                    result = Convert.ToDateTime((str + "-01-01").Replace("年", "").Replace("月", ""));
                                }
                                catch (Exception)
                                {
                                    result = null;
                                }
                            }
                        }
                        else
                        {
                            DateTime dt;
                            if (DateTime.TryParse(str.Replace("年", "").Replace("月", ""), out dt))
                            {
                                result = dt;
                            }
                        }
                        break;
                    case CellType.Blank:
                        break;
                        #region

                        #endregion
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return result;
        }
    }
    class ExcelExporter
    {
        public byte[] ObjectToExcelBytes<TModel>(IEnumerable<TModel> data)
        {
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet();
            var attrDict = ExcelUtil.GetExportAttrDict<TModel>();
            var attrArray = new KeyValuePair<PropertyInfo, ExcelAttribute>[] { };
            int aNum = 0;
            foreach (var item in attrDict)
            {
                attrArray[aNum] = item;
                aNum++;

            }

            for (int i = 0; i < attrArray.Length; i++)
            {
                sheet.SetColumnWidth(i, 50 * 256);
            }
            var headerRow = sheet.CreateRow(0);

            for (int i = 0; i < attrArray.Length; i++)
            {
                headerRow.CreateCell(i).SetCellValue(attrArray[i].Value.Title);
            }
            int rowNumber = 1;
            foreach (var item in data)
            {
                var row = sheet.CreateRow(rowNumber++);
                for (int i = 0; i < attrArray.Length; i++)
                {
                    row.CreateCell(i).SetCellValue((attrArray[i].Key.GetValue(item, null) ?? "").ToString());
                }
            }
            using (var output = new MemoryStream())
            {
                workbook.Write(output);
                var bytes = output.ToArray();
                return bytes;
            }
        }
    }

    public class ExcelHelper
    {
        /// <summary>
        /// import file excel file to a IEnumerable of TModel
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="path">excel full path</param>
        /// <returns></returns>
        public static IEnumerable<TModel> ExcelToObject<TModel>(string path) where TModel : class, new()
        {
            var importer = new ExcelImporter();
            return importer.ExcelToObject<TModel>(path);

        }

        /// <summary>
        /// Export object to excel file
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="data">a IEnumerable of TModel</param>
        /// <param name="path">excel full path</param>
        public static void ObjectToExcel<TModel>(IEnumerable<TModel> data, string path) where TModel : class, new()
        {
            var importer = new ExcelExporter();
            var bytes = importer.ObjectToExcelBytes(data);
            File.WriteAllBytes(path, bytes);
        }
    }
    internal class ExcelUtil
    {
        public static Dictionary<PropertyInfo, ExcelAttribute> GetExportAttrDict<T>()
        {
            var dict = new Dictionary<PropertyInfo, ExcelAttribute>();
            foreach (var propertyInfo in typeof(T).GetProperties())
            {
                var attr = new object();
                var ppi = propertyInfo.GetCustomAttributes(true);
                for (int i = 0; i < ppi.Length; i++)
                {
                    if (ppi[i] is ExcelAttribute)
                    {
                        attr = ppi[i];
                        break;
                    }
                }

                if (attr != null)
                {

                    dict.Add(propertyInfo, attr as ExcelAttribute);

                }
            }
            return dict;
        }
    }
}