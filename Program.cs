using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using ParseSchedule.Models;

namespace ParseSchedule
{

    class Program
    {

        public static ExcelPackage package { get; set; }
        public static ExcelWorksheet worksheet { get; set; }
        static void Main(string[] args)
        {
            Console.WriteLine("Set connetion");
            FileInfo existingFile = new FileInfo("d:\\schedule1.xlsx");
            Console.OutputEncoding = Encoding.UTF8;
            var spec = new List<SpecialityCell>();
            var grop = new List<GroupCell>();
            using (package = new ExcelPackage(existingFile))
            {
                // get the first worksheet in the workbook
                worksheet = package.Workbook.Worksheets.FirstOrDefault();
                spec = GetSpecialties();
                grop = GetGroups();
                //int col = 2; //The item description
                //             // output the data in column 2
                //for (int row = 2; row < 5; row++)
                //    Console.WriteLine("\tCell({0},{1}).Value={2}", row, col, worksheet.Cells[row, col].Value);

                //// output the formula in row 5
                //Console.WriteLine("\tCell({0},{1}).Formula={2}", 3, 5, worksheet.Cells[3, 5].Formula);
                //Console.WriteLine("\tCell({0},{1}).FormulaR1C1={2}", 3, 5, worksheet.Cells[3, 5].FormulaR1C1);

                //// output the formula in row 5
                //Console.WriteLine("\tCell({0},{1}).Formula={2}", 5, 3, worksheet.Cells[5, 3].Formula);
                //Console.WriteLine("\tCell({0},{1}).FormulaR1C1={2}", 5, 3, worksheet.Cells[5, 3].FormulaR1C1);

            } // the using statement automatically calls Dispose() which closes the package.
            ConnectGroupToSpecialities(ref spec, ref grop);
            Console.WriteLine("Specialities count: {0}/39", spec.Count);
            Console.WriteLine("Groups count: {0}/50", grop.Count);
            var s = 0;
            foreach (var item in spec)
            {
                Console.WriteLine(item.Speciality.Name);
                foreach (var item2 in item.Groups)
                {
                    s++;
                    Console.WriteLine("    {1}. - {0}", item2.Name,s);
                }

            }
            Console.ReadLine();
        }

        public static void ConnectGroupToSpecialities(ref List<SpecialityCell> specialities, ref List<GroupCell> groups)
        {
            foreach (var speciality in specialities)
            {
                speciality.Groups = new List<TableModels.Group>();
                foreach (var group in groups)
                {
                    var start = GetColumnOrder(speciality.StartCellIndex, group.StartCellIndex);
                    var end = GetColumnOrder(speciality.EndCellIndex, group.EndCellIndex);
                    if (start < 1 && end > -1)
                    {
                        speciality.Groups.Add(group.Group);
                    }
                }
            }
        }

        public static List<GroupCell> GetGroups()
        {
            return GetDataFromRow<GroupCell>(Constants.GroupRowIndex)
                   .Select(s => new GroupCell {
                       StartCellIndex = s.StartCellIndex,
                       EndCellIndex = s.EndCellIndex,
                       Group = new TableModels.Group {
                           Id = new Guid(),
                           Name = s.TextValue
                    }
                }).ToList();
        }

        public static List<SpecialityCell> GetSpecialties()
        {
            return GetDataFromRow<SpecialityCell>(Constants.SpecialityRowIndex)
                   .Select(s => new SpecialityCell
                   {
                       StartCellIndex = s.StartCellIndex,
                       EndCellIndex = s.EndCellIndex,
                       Speciality = new TableModels.Speciality
                       {
                           Id = new Guid(),
                           Name = s.TextValue
                       }
                   }).ToList();
        }
        public static List<T> GetDataFromRow<T>(int rowIndex) where T : CellBase, new()
        {
            var data = new List<T>();
            for (int i = Constants.ContentStartIndex; i < worksheet.Dimension.Columns; i++)
            {
                var cellRange = worksheet.Cells[rowIndex, i].GetMergedRangeAddress();
                if (cellRange != null)
                {
                    var cellValue = GetCellValue(rowIndex, i);
                    if (cellValue != null)
                    {
                        var item = new T();
                        item.TextValue = cellValue;

                        var range = cellRange.Split(':');
                        if (range.Length > 1)
                        {
                            item.StartCellIndex = range[0];
                            item.EndCellIndex = range[1];

                        }
                        else
                        {
                            item.StartCellIndex = cellRange;
                            item.EndCellIndex = cellRange;
                        }
                        data.Add(item);
                    }
                }
            }

            return data;
        }

        public static string GetCellValue(int rowIndex, int columnIndex)
        {
            var cellValue = worksheet.Cells[rowIndex, columnIndex].Value;
            if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue.ToString()))
                return cellValue.ToString();
            return null;
        }

        public static int GetColumnOrder(string firstColumn, string secondColumn)
        {
            return string.Compare(firstColumn.GetLetters(), secondColumn.GetLetters(), true);
        }

    }
}
