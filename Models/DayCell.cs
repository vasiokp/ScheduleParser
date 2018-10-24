using ParseSchedule.TableModels;
using System.Collections.Generic;

namespace ParseSchedule.Models
{
   public class DayCell : BaseCell
    {
        public Day Day { get; set; }
        public List<LessonNumberCell> LessonNumbers { get; set; }
    }
}
