namespace ParseSchedule.Models
{
    public abstract class BaseCell
    {
        public string StartCellIndex { get; set; }
        public string EndCellIndex { get; set; }
        public string TextValue { get; set; }
        public string CellIndex => IsMerged ? $"{StartCellIndex}:{EndCellIndex}" : StartCellIndex;
        public bool IsMerged => EndCellIndex != StartCellIndex && StartCellIndex != null && EndCellIndex != null;
    }
}
