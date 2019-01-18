using OfficeOpenXml;
using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using OfficeOpenXml.Style;
using System.Drawing;

namespace HelloEPPlus
{
    public static class SimpleEPPlus
    {
        public static void ReadExcel()
        {
            var excelFile = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "xlsx\\sample.xlsx"));
            using (var package = new ExcelPackage(excelFile))
            {
                //var sheet = package.Workbook.Worksheets.FirstOrDefault();
                foreach (var sheet in package.Workbook.Worksheets)
                {
                    var list = new List<Dictionary<string, object>>();

                    // Fetch the WorkSheet size
                    ExcelCellAddress startCell = sheet.Dimension.Start;
                    ExcelCellAddress endCell = sheet.Dimension.End;

                    // headers
                    var headers = new string[endCell.Column + 1];
                    for (int col = startCell.Column; col <= endCell.Column; col++)
                    {
                        var cell = sheet.Cells[startCell.Row, col];
                        headers[col] = (cell.Value == null ? cell.Address : cell.Text);
                    }

                    // cells
                    for (int row = startCell.Row + 1; row <= endCell.Row; row++)
                    {
                        var obj = sheet.Cells[row, startCell.Column, row, endCell.Column].ToDictionary(c => headers[c.Start.Column], c => c.Value);
                        list.Add(obj);
                    }

                    var listObj = list.Select(c => DictionaryToObject<SimpleModel>(c)).ToList();
                    Console.WriteLine(JsonConvert.SerializeObject(new { sheet = sheet.Name, cells = list, models = listObj }, Formatting.Indented));
                }
            }
        }

        public static void ReadExcelManual()
        {
            var excelFile = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "xlsx\\sample.xlsx"));
            using (var package = new ExcelPackage(excelFile))
            {
                var list = new List<SimpleModel>();
                var sheet = package.Workbook.Worksheets[1];
                // Fetch the WorkSheet size
                ExcelCellAddress startCell = sheet.Dimension.Start;
                ExcelCellAddress endCell = sheet.Dimension.End;
                // cells
                for (int row = startCell.Row + 1; row <= endCell.Row; row++)
                {
                    var item = new SimpleModel();
                    item.col1 = Convert.ToInt32(sheet.Cells[row, 1].Value);
                    item.col2 = sheet.Cells[row, 2].Text;
                    item.col3 = sheet.Cells[row, 3].Text;
                    list.Add(item);
                }
                Console.WriteLine(JsonConvert.SerializeObject(new { cells = list }, Formatting.Indented));
            }
        }

        public static void WriteExcel()
        {
            var excelFile = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "xlsx\\sample2.xlsx"));
            var list = new[]
            {
                new { col1 = 1, col2 = "ABC" },
                new { col1 = 2, col2 = "DEF" },
                new { col1 = 3, col2 = "XYZ" }
            };
            // Creates a blank workbook. Use the using statment, so the package is disposed when we are done.
            using (var p = new ExcelPackage())
            {
                // A workbook must have at least on cell, so lets add one... 
                var ws = p.Workbook.Worksheets.Add("MySheet");
                // To set values in the spreadsheet use the Cells indexer.
                ws.Cells["A1"].LoadFromCollection(list, true);
                // Style
                using (var range = ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(92, 184, 92));
                    range.Style.Font.Color.SetColor(Color.White);
                }
                using (var range = ws.Cells[ws.Dimension.Start.Row, ws.Dimension.Start.Column, ws.Dimension.End.Row, ws.Dimension.End.Column])
                {
                    range.Style.Border.Top.Style = ExcelBorderStyle.Hair;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Hair;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Hair;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Hair;
                }
                using (var range = ws.Cells[1, 2, ws.Dimension.End.Row, 2])
                    foreach (var cell in range)
                        if (cell.Text == "ABC")
                            cell.Style.Font.Bold = true;
                // Autofit columns for all cells
                ws.Cells.AutoFitColumns(0);
                // Save the new workbook. We haven't specified the filename so use the Save as method.
                p.SaveAs(excelFile);
            }
        }
        
        private static DateTime? DateTimeExact(string value)
        {
            DateTime dateTime = new DateTime();
            long dateNum = 0;
            if (long.TryParse(value, out dateNum))
                return DateTime.FromOADate(dateNum);
            else if (DateTime.TryParse(value, out dateTime))
                return dateTime;
            else
                return null;
        }

        private static T DictionaryToObject<T>(IDictionary<string, object> dict) where T : new()
        {
            var t = new T();
            PropertyInfo[] properties = t.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (!dict.Any(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase)))
                    continue;

                KeyValuePair<string, object> item = dict.First(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase));

                // Find which property type (int, string, double? etc) the CURRENT property is...
                Type tPropertyType = t.GetType().GetProperty(property.Name).PropertyType;

                // Fix nullables...
                Type newT = Nullable.GetUnderlyingType(tPropertyType) ?? tPropertyType;

                // ...and change the type
                object newA = null;
                try
                {
                    newA = Convert.ChangeType(item.Value, newT);
                }
                catch
                {
                    if (newT.IsValueType)
                        newA = Activator.CreateInstance(newT);
                }
                t.GetType().GetProperty(property.Name).SetValue(t, newA, null);
            }
            return t;
        }
    }
}
