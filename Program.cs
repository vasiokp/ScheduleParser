﻿using System;
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
        public static ExcelPackage Package { get; set; }
        public static ExcelWorksheet Worksheet { get; set; }
        static void Main(string[] args)
        {
            Console.WriteLine("Set connetion");
            FileInfo existingFile = new FileInfo("d:\\schedule1.xlsx");
            Console.OutputEncoding = Encoding.UTF8;
            var spec = new List<SpecialityCell>();
            var grop = new List<GroupCell>();
            var days = new List<DayCell>();
            using (Package = new ExcelPackage(existingFile))
            {
                // get the first worksheet in the workbook
                Worksheet = Package.Workbook.Worksheets.FirstOrDefault();
                var parser = new Parser(Worksheet);

                spec = Entity.GetSpecialties();
                grop = Entity.GetGroups();
                days = Entity.GetDays();
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
