using OfficeOpenXml;
using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

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

        public static void WriteExcel()
        {
            var excelFile = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "xlsx\\sample2.xlsx"));
            var list = new[] { new { col1 = 1, col2 = "A" }, new { col1 = 2, col2 = "B" }, new { col1 = 3, col2 = "C" } };
            //Creates a blank workbook. Use the using statment, so the package is disposed when we are done.
            using (var p = new ExcelPackage())
            {
                //A workbook must have at least on cell, so lets add one... 
                var ws = p.Workbook.Worksheets.Add("MySheet");
                //To set values in the spreadsheet use the Cells indexer.
                ws.Cells["A1"].LoadFromCollection(list, true);
                //Save the new workbook. We haven't specified the filename so use the Save as method.
                p.SaveAs(excelFile);
            }
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