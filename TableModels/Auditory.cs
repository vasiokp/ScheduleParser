using ParseSchedule.Models;
using System;
namespace ParseSchedule.TableModels
{
   public class Auditory : ITableModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
