using System;
using ParseSchedule.TableModels;

namespace ParseSchedule.Models
{
    public class GroupCell : CellBase
    {
        public Group Group { get; set; }
        public Speciality Speciality { get; set; }
    }
}
