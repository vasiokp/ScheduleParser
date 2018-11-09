using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseSchedule
{
    public static class Extensions
    {
        public static string GetMergedRangeAddress(this ExcelRange @this)
        {
            if (@this.Merge)
            {
                var idx = @this.Worksheet.GetMergeCellId(@this.Start.Row, @this.Start.Column);
                return @this.Worksheet.MergedCells[idx - 1]; //the array is 0-indexed but the mergeId is 1-indexed...
            }
            else
            {
                return @this.Address;
            }
        }

        public static string GetLetters(this string @this) => new string(@this.Where(char.IsLetter).ToArray());
        public static int GetNumber(this string @this)
        {
            var stringNumber = string.Join("", @this.Where(char.IsDigit).ToArray());
            return int.Parse(stringNumber);
        }
    }
}
