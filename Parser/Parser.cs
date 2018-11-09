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

        static ExcelWorksheet Worksheet { get; set; }

        public Parser(ExcelWorksheet w)
        {
            Worksheet = w;
        }

        public static List<T> GetDataFromRow<T>(int rowIndex, int columnIndex) where T : BaseCell, new()
        {
            var data = new List<T>();
            for (int i = columnIndex; i < Worksheet.Dimension.Columns; i++)
            {
                var cellRange = Worksheet.Cells[rowIndex, i].GetMergedRangeAddress();
                if (cellRange != null)
                {
                    var cellValue = GetCellValue(rowIndex, i);
                    if (cellValue != null)
                    {
                        var item = new T
                        {
                            TextValue = cellValue
                        };
                        GetCellIndexes(ref item, cellRange);
                        data.Add(item);
                    }
                }
            }

            return data;
        }

        public static List<T> GetDataFromColumn<T>(int rowIndex, int columnIndex) where T : BaseCell, new ()
        {
            var data = new List<T>();
            for (int i = rowIndex; i < Worksheet.Dimension.Rows; i++)
            {
                var cellRange = Worksheet.Cells[i, columnIndex].GetMergedRangeAddress();
                if (cellRange != null)
                {
                    var cellValue = GetCellValue(i, columnIndex);
                    if (cellValue != null)
                    {
                        var item = new T
                        {
                            TextValue = cellValue
                        };
                        GetCellIndexes(ref item, cellRange);
                        data.Add(item);
                    }
                }
            }

            return data;
        }

        public static List<T> GetDataFromTable<T>(int rowIndex, int columnIndex, int maxRowIndex) where T: BaseCell, new()
        {
            var data = new List<T>();
            for (int i = rowIndex; i < maxRowIndex + 1; i++)
            {
                for (int j = columnIndex; j < Worksheet.Dimension.Columns; j++)
                {
                    var cellRange = Worksheet.Cells[i, j].GetMergedRangeAddress();
                    if (cellRange != null)
                    {
                        var cellValue = GetCellValue(i, j);
                        if (cellValue != null)
                        {
                            var item = new T
                            {
                                TextValue = cellValue
                            };
                            GetCellIndexes(ref item, cellRange);
                            data.Add(item);
                        }
                    }
                }
            }

            return data;
        }

        public static string GetCellValue(int rowIndex, int columnIndex)
        {
            var cellValue = Worksheet.Cells[rowIndex, columnIndex].Value;
            if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue.ToString()))
                return cellValue.ToString();
            return null;
        }

        public static void GetCellIndexes<T>(ref T cell, string cellRange) where T: BaseCell
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
