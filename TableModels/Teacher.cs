using System;
using ParseSchedule.Models;

namespace ParseSchedule.TableModels
{
   public class Teacher : ITableModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
