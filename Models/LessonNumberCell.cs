using ParseSchedule.TableModels;

namespace ParseSchedule.Models
{
    public class LessonNumberCell : BaseCell
    {
        public LessonNumber LessonNumber { get; set; }
        public Week Week { get; set; }
    }
}
