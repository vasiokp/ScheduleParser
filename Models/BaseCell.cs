namespace ParseSchedule.Models
{
    public abstract class BaseCell
    {
        public string StartCellIndex { get; set; }
        public string EndCellIndex { get; set; }
        public string TextValue { get; set; }
        public string CellIndex => IsMerged ? $"{StartCellIndex}:{EndCellIndex}" : StartCellIndex;
        public bool IsMerged => IsMergedRows || IsMergedColumns;
        public bool IsMergedRows => StartCellIndex.GetNumber() != EndCellIndex.GetNumber();
        public bool IsMergedColumns => StartCellIndex.GetLetters() != EndCellIndex.GetLetters();

        public bool IsInRange(string rangeStart, string rangeEnd)
        {
            return IsInRowRange(rangeStart, rangeEnd) && IsInColumnRange(rangeStart, rangeEnd);
        }
        public bool IsInRowRange(string itemStart, string itemEnd)
        {
            return StartCellIndex.GetNumber() <= itemStart.GetNumber() && itemEnd.GetNumber() <= EndCellIndex.GetNumber();
        }

        public bool IsInColumnRange(string itemStart, string itemEnd)
        {
            var isLeftColumnInRange = Helpers.GetColumnOrder(StartCellIndex, itemStart) <= 0;
            var isRigthColumnInRange = Helpers.GetColumnOrder(itemEnd, EndCellIndex) <= 0;
            return isLeftColumnInRange && isRigthColumnInRange;
        }
    }
}
