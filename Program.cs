using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using ParseSchedule.Models;
using ParseSchedule;

namespace ParseSchedule
{

    class Program
    {

        public static ExcelPackage package { get; set; }
        public static ExcelWorksheet worksheet { get; set; }
        static void Main(string[] args)
        {
            Console.WriteLine("Set connetion");
            FileInfo existingFile = new FileInfo("d:\\schedule1.xlsx");
            Console.OutputEncoding = Encoding.UTF8;
            var spec = new List<SpecialityCell>();
            var grop = new List<GroupCell>();
            using (package = new ExcelPackage(existingFile))
            {
                // get the first worksheet in the workbook
                worksheet = package.Workbook.Worksheets.FirstOrDefault();
                var parser = new Parser(worksheet);

                spec = Entity.GetSpecialties();
                grop = Entity.GetGroups();
            }
            Entity.ConnectGroupToSpecialities(ref spec, ref grop);

            Console.WriteLine("Specialities count: {0}/39", spec.Count);
            Console.WriteLine("Groups count: {0}/50", grop.Count);
            var s = 0;
            foreach (var item in spec)
            {
                Console.WriteLine(item.Speciality.Name);
                foreach (var item2 in item.Groups)
                {
                    s++;
                    Console.WriteLine("    {1}. - {0}", item2.Name,s);
                }

            }
            Console.ReadLine();
        }
    }
}
