namespace ParseSchedule
{
   public class Helpers
    {
        public static int GetColumnOrder(string firstColumn, string secondColumn)
        {
            return string.Compare(firstColumn.GetLetters(), secondColumn.GetLetters(), true);
        }
    }
}
