﻿using System.Linq;

namespace ParseSchedule
{
   public class Helpers
    {
        public static int GetColumnOrder(string firstColumn, string secondColumn)
        {
            var first = firstColumn.GetLetters();
            var second = secondColumn.GetLetters();
            if (second.Count() == first.Count())
                return string.Compare(firstColumn.GetLetters(), secondColumn.GetLetters(), true);
            else if (second.Count() > first.Count())
            {
                return -1;
            }
            else return 1;
        }

        public static int GetNumberFromString(string week)
        {
            if (string.IsNullOrWhiteSpace(week)) return 0;
            var number = week.Substring(0, 1);
            return int.Parse(number);
        }
    }
}
