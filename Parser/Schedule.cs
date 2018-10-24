using ParseSchedule.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParseSchedule
{
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
                       Week = new TableModels.Week
                       {
                           WeekNumber = Helpers.GetNumberFromString(s.TextValue)
                       }
                   }).ToList();
        }

        public static List<LessonCell> GetLessons()
        {
            var list = Parser.GetDataFromTable<LessonCell>((int)ConstantIndexes.ContentRowIndex, (int)ConstantIndexes.ContentColumnIndex, Days.Last().EndCellIndex.GetNumber());
            var resultList = new List<LessonCell>();
            foreach (var item in list)
            {

                item.Groups = Groups.Where(g => g.IsInColumnRange(item.StartCellIndex, item.EndCellIndex)).Select(g => g.Group).ToList();
                if (item.Groups.Count != 0)
                {
                    item.Specialities = Specialities.Where(s => s.IsInColumnRange(item.StartCellIndex, item.EndCellIndex)).Select(s => s.Speciality).ToList();
                    item.Day = Days.Where(d => d.IsInRowRange(item.StartCellIndex, item.EndCellIndex)).Select(d => d.Day).FirstOrDefault();
                    item.LessonNumber = LessonNumbers.Where(l => l.IsInRowRange(item.StartCellIndex, item.EndCellIndex)).Select(s => s.LessonNumber).FirstOrDefault();
                    if (!item.IsMergedRows)
                    {
                        item.Week = Weeks.Where(w => w.IsInRowRange(item.StartCellIndex, item.EndCellIndex)).Select(w => w.Week).FirstOrDefault();
                    }
                    resultList.Add(item);
                }

            }
            var sd = resultList.GroupBy(l => new { l.Day, l.LessonNumber, l.Week, l.Groups.First().Name},
                (key,group) => new
                {
                    key.Day,
                    key.LessonNumber,
                    key.Week,
                    key.Name,
                    Result = group.ToList()

                }
                ).ToList();
            var ss = sd.Where(sq => sq.Result.Count != 3).ToList();

           
            return new List<LessonCell>();

        }

        public static void ConnectGroupToSpecialities(ref List<SpecialityCell> specialities, ref List<GroupCell> groups)
        {
            foreach (var speciality in specialities)
            {
                speciality.Groups = new List<TableModels.Group>();
                foreach (var group in groups)
                {
                    var start = Helpers.GetColumnOrder(speciality.StartCellIndex, group.StartCellIndex);
                    var end = Helpers.GetColumnOrder(speciality.EndCellIndex, group.EndCellIndex);
                    if (start < 1 && end > -1)
                    {
                        speciality.Groups.Add(group.Group);
                    }
                }
            }
        }

    }
}
