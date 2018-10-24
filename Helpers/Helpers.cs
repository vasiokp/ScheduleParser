using System.Linq;

namespace ParseSchedule
{
   public class Helpers
    {
        public static int GetColumnOrder(string firstColumn, string secondColumn)
        {
            return string.Compare(firstColumn.GetLetters(), secondColumn.GetLetters(), true);
        }

        public static int GetNumberFromString(string week)
        {
            if (string.IsNullOrWhiteSpace(week)) return 0;
            var number = week.Substring(0, 1);
            return int.Parse(number);
        }
    }
}
