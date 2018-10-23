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

    }
}
