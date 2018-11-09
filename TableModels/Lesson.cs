using ParseSchedule.Models;
using System;

namespace ParseSchedule.TableModels
{
   public class Lesson : ITableModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
