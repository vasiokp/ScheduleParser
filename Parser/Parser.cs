using OfficeOpenXml;
using ParseSchedule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseSchedule
{
   public class Parser
    {

        static ExcelWorksheet worksheet { get; set; }

        public Parser(ExcelWorksheet w)
        {
            worksheet = w;
        }

        public static List<T> GetDataFromRow<T>(int rowIndex, int columnIndex) where T : CellBase, new()
        {
            var data = new List<T>();
            for (int i = rowIndex; i < worksheet.Dimension.Columns; i++)
            {
                var cellRange = worksheet.Cells[rowIndex, i].GetMergedRangeAddress();
                if (cellRange != null)
                {
                    var cellValue = GetCellValue(rowIndex, i);
                    if (cellValue != null)
                    {
                        var item = new T();
                        item.TextValue = cellValue;
                        GetCellIndexes(ref item, cellRange);
                        data.Add(item);
                    }
                }
            }

            return data;
        }

        public static List<T> GetDataFromColumn<T>(int rowIndex, int columnIndex) where T : CellBase, new ()
        {
            var data = new List<T>();
            for (int i = rowIndex; i < worksheet.Dimension.Rows; i++)
            {
                var cellRange = worksheet.Cells[i, columnIndex].GetMergedRangeAddress();
                if (cellRange != null)
                {
                    var cellValue = GetCellValue(i, columnIndex);
                    if (cellValue != null)
                    {
                        var item = new T();
                        item.TextValue = cellValue;
                        GetCellIndexes(ref item, cellRange);
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

        public static void GetCellIndexes<T>(ref T cell, string cellRange) where T: CellBase
        {
            var range = cellRange.Split(':');
            if (range.Length > 1)
            {
                cell.StartCellIndex = range[0];
                cell.EndCellIndex = range[1];
            }
            else
            {
                cell.StartCellIndex = cellRange;
                cell.EndCellIndex = cellRange;
            }
        }

    }
}
