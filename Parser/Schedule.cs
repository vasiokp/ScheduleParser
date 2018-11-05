using ParseSchedule.Models;
using ParseSchedule.TableModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParseSchedule
{
    public class GroupedItem
    {
        public Day Day { get; set; }
        public string GroupNumber { get; set; }
        public LessonNumber NumberOfLesson { get; set; }
        public int CountOfSpecialities { get; set; }
        public List<LessonCell> Rows { get; set; }
    }

    public static class Schedule
    {
        public static List<SpecialityCell> Specialities { get; set; }
        public static List<DayCell> Days { get; set; }
        public static List<WeekCell> Weeks { get; set; }
        public static List<GroupCell> Groups { get; set; }
        public static List<LessonNumberCell> LessonNumbers { get; set; }
        public static List<LessonCell> Lessons { get; set; }

        static Schedule()
        {
            Specialities = new List<SpecialityCell>();
            Groups = new List<GroupCell>();
            Days = new List<DayCell>();
            Weeks = new List<WeekCell>();
            LessonNumbers = new List<LessonNumberCell>();
            Lessons = new List<LessonCell>();
        }

        public static void Init()
        {
            Specialities = GetSpecialties();
            Groups = GetGroups();
            ConnectGroupToSpecialities();
            Days = GetDays();
            Weeks = GetWeeks();
            LessonNumbers = GetLessonNumberCells();
            Lessons = GetLessons();
        }

        public static List<GroupCell> GetGroups()
        {
            
            return Parser.GetDataFromRow<GroupCell>(Constants.GroupRowIndex, Constants.ContentStartIndex)
                   .Select(s => new GroupCell
                   {
                       StartCellIndex = s.StartCellIndex,
                       EndCellIndex = s.EndCellIndex,
                       Group = new TableModels.Group
                       {
                           Id = Guid.NewGuid(),
                           Name = s.TextValue
                       }
                   }).ToList();
        }

        public static List<SpecialityCell> GetSpecialties()
        {
            return Parser.GetDataFromRow<SpecialityCell>((int)ConstantIndexes.SpecialityRowIndex, (int)ConstantIndexes.ContentColumnIndex)
                   .Select(s => new SpecialityCell
                   {
                       StartCellIndex = s.StartCellIndex,
                       EndCellIndex = s.EndCellIndex,
                       Speciality = new TableModels.Speciality
                       {
                           Id = Guid.NewGuid(),
                           Name = s.TextValue
                       }
                   }).ToList();
        }

        public static List<DayCell> GetDays()
        {
            return Parser.GetDataFromColumn<DayCell>((int)ConstantIndexes.DayRowIndex, (int)ConstantIndexes.DayColumnIndex)
                   .Select(s => new DayCell
                   {
                       StartCellIndex = s.StartCellIndex,
                       EndCellIndex = s.EndCellIndex,
                       Day = new TableModels.Day
                       {
                           Id = Guid.NewGuid(),
                           Name = s.TextValue
                       }
                   }).ToList();
        }

        public static List<LessonNumberCell> GetLessonNumberCells()
        {
            return Parser.GetDataFromColumn<LessonNumberCell>((int)ConstantIndexes.LessonNumberRowIndex, (int)ConstantIndexes.LessonNumberColumIndex)
                   .Select(s => new LessonNumberCell
                   {
                       StartCellIndex = s.StartCellIndex,
                       EndCellIndex = s.EndCellIndex,
                       LessonNumber = new TableModels.LessonNumber
                       {
                           NumberOfLesson = int.Parse(s.TextValue)
                       }
                   }).ToList();
        }

        public static List<WeekCell> GetWeeks()
        {
            return Parser.GetDataFromColumn<WeekCell>((int)ConstantIndexes.WeekRowIndex, (int)ConstantIndexes.WeekColumnIndex)
                   .Select(s => new WeekCell
                   {
                       StartCellIndex = s.StartCellIndex,
                       EndCellIndex = s.EndCellIndex,
                       Week = new Week
                       {
                           Name = s.TextValue
                       }
                   }).ToList();
        }

        public static List<LessonCell> GetLessons()
        {
            var list = Parser.GetDataFromTable<LessonCell>((int)ConstantIndexes.ContentRowIndex, (int)ConstantIndexes.ContentColumnIndex, Days.Last().EndCellIndex.GetNumber());
            var resultList = new List<LessonCell>();
            var lessonsList = new List<LessonCell>();
            foreach (var item in list)
            {
                var itemGroups = Groups.Where(g => g.IsInColumnRange(item.StartCellIndex, item.EndCellIndex)).ToList();
                if (itemGroups.Count != 0) // will need to be changed
                {
                    item.Groups = itemGroups.Select(g => g.Group).ToList();
                    item.Specialities = itemGroups.Select(s => s.Speciality).Distinct().ToList();
                    item.Day = Days.Where(d => d.IsInRowRange(item.StartCellIndex, item.EndCellIndex)).Select(d => d.Day).FirstOrDefault();
                    item.LessonNumber = LessonNumbers.Where(l => l.IsInRowRange(item.StartCellIndex, item.EndCellIndex)).Select(s => s.LessonNumber).FirstOrDefault();
                    if (!item.IsMergedRows)
                    {
                        item.Week = Weeks.Where(w => w.IsInRowRange(item.StartCellIndex, item.EndCellIndex)).Select(w => w.Week).FirstOrDefault();
                    }
                    resultList.Add(item);
                }

            }
            var lessonWithOneGroup = resultList.Where(l => l.Groups.Count == 1);
            //var lessonForMoreGroup = resultList.Where(l => l.Groups.Count > 1);
            //var lessonsByDay = lessonForOneGroup.GroupBy(l => new  GroupedItem { st = l.Groups.Select(g1 => g1.Name), Day = l.Day },
            //    (key, group) => new GroupedItem
            //    {
            //        Day = key.Day,
            //        NumberOfLesson = key.NumberOfLesson,
            //        Rows = group.ToList()
            //    }
            //    ).ToList();

            var lessonsByDay = lessonWithOneGroup.GroupBy(l => new { groupNumber = l.Groups.First().Name, l.Day, l.LessonNumber },
            (key, group) => new GroupedItem
            {
                Day = key.Day,
                NumberOfLesson = key.LessonNumber,
                GroupNumber = key.groupNumber,
                Rows = group.ToList()
            }
            ).ToList();

            foreach (var item in lessonsByDay)
            {
                var info = new LessonCell();

                if (item.Rows.Count == 1) // fizra
                {
                    info.Day = item.Day;
                    info.Groups = item.Rows.First().Groups;
                    info.LessonNumber = item.Rows.First().LessonNumber;
                    info.Lesson = GetLesson(item.Rows);
                    info.Specialities = item.Rows.First().Specialities;
                    info.Week = item.Rows.First().Week;
                }

                if (item.Rows.Count == 2)
                {
                    info.Day = item.Day;
                    info.Groups = item.Rows.First().Groups;
                    info.LessonNumber = item.Rows.First().LessonNumber;
                    info.Lesson = GetLesson(item.Rows);
                    info.Auditory = GetAuditory(item.Rows, 1);
                    info.Week = item.Rows.First().Week;
                    info.Specialities = item.Rows.First().Specialities;

                    lessonsList.Add(info);

                }
                if (item.Rows.Count == 3)
                {
                    info = MapLesson(item);
                    lessonsList.Add(info);
                }
                if (item.Rows.Count == 4) // groups with the same name
                {

                }
                if (item.Rows.Count == 5) // like 2 and 3 or visa versa !?!?
                {

                }
                if (item.Rows.Count == 6)
                {
                    info = MapLesson(item);
                    lessonsList.Add(info);
                    info = MapLesson(item, 3);
                    lessonsList.Add(info);
                }
                if(item.Rows.Count > 6)
                {

                }
            }
            return lessonsList;
        }

        public static void ConnectGroupToSpecialities()
        {
            foreach (var speciality in Specialities)
            {
                speciality.Groups = new List<Group>();
                foreach (var group in Groups)
                {
                    var start = Helpers.GetColumnOrder(speciality.StartCellIndex, group.StartCellIndex);
                    var end = Helpers.GetColumnOrder(speciality.EndCellIndex, group.EndCellIndex);
                    if (start < 1 && end > -1)
                    {
                        speciality.Groups.Add(group.Group);
                        group.Speciality = speciality.Speciality;
                    }
                }
            }
        }

        public static Lesson GetLesson(IEnumerable<LessonCell> items) => GetEntity<Lesson>(items, 0);
        public static Teacher GetTeacher(IEnumerable<LessonCell> items) => GetEntity<Teacher>(items, 1);
        public static Auditory GetAuditory(IEnumerable<LessonCell> items, int itemIndex = 2) => GetEntity<Auditory>(items, itemIndex);

        static T GetEntity<T>(IEnumerable<LessonCell> items, int itemIndex) where T : ITableModel, new()
        {
            return new T
            {
                Id = Guid.NewGuid(),
                Name = items.ToList()[itemIndex].TextValue
            };

        }
        static LessonCell MapLesson(GroupedItem item, int skipNumber = 0)
        {
            return new LessonCell
            {
                Day = item.Day,
                Groups = item.Rows.First().Groups,
                Specialities = item.Rows.First().Specialities,
                LessonNumber = item.NumberOfLesson,
                Lesson = GetLesson(item.Rows.Skip(skipNumber)),
                Teacher = GetTeacher(item.Rows.Skip(skipNumber)),
                Auditory = GetAuditory(item.Rows.Skip(skipNumber)),
                Week = item.Rows.First().Week
            };
        }
    }
}
