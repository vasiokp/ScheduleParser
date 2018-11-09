using ParseSchedule.TableModels;
using System.Collections.Generic;

namespace ParseSchedule.Models
{
   public class LessonCell : BaseCell
    {
        public Day Day { get; set; }
        public Lesson Lesson { get; set; }
        public List<Group> Groups { get; set; }
        public List<Speciality> Specialities { get; set; }
        public Teacher Teacher { get; set; }
        public LessonNumber LessonNumber { get; set; }
        public Auditory Auditory { get; set; }
        public Week Week { get; set; }
    }
}
