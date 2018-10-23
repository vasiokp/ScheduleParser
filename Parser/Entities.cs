using ParseSchedule.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParseSchedule
{
    public class Entity
    {

        public static List<GroupCell> GetGroups()
        {
            return Parser.GetDataFromRow<GroupCell>(Constants.GroupRowIndex)
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
            return Parser.GetDataFromRow<SpecialityCell>(Constants.SpecialityRowIndex)
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
