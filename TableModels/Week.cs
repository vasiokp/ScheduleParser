using ParseSchedule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseSchedule.TableModels
{
    public class Week : ITableModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
