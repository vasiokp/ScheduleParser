using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseSchedule.Models
{
    interface IModelData
    {
        Guid Id { get; set; }
        string Name { get; set; }
    }
}
