using ParseSchedule.TableModels;
using System.Collections.Generic;

namespace ParseSchedule.Models
{
    public class SpecialityCell : BaseCell
    {
        public Speciality Speciality { get; set; }
        public List<Group> Groups { get; set; }
    }
}
