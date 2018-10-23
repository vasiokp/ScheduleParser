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
        public string Teacher { get; set; }
        public string LessonNumber { get; set; }
        public string Auditory { get; set; }
    }
}
