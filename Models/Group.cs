using ParseSchedule.TableModels;

namespace ParseSchedule.Models
{
    public class GroupCell : BaseCell
    {
        public Group Group { get; set; }
        public Speciality Speciality { get; set; }
    }
}
