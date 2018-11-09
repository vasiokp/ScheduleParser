using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using OfficeOpenXml;
using ParseSchedule.Models;

namespace ParseSchedule
{
    class Program
    {
        public static ExcelPackage Package { get; set; }
        public static ExcelWorksheet Worksheet { get; set; }
        static void Main(string[] args)
        {
            Console.WriteLine("Set connetion");
            FileInfo existingFile = new FileInfo("c:\\schedule1.xlsx");
            Console.OutputEncoding = Encoding.UTF8;

            using (Package = new ExcelPackage(existingFile))
            {
                // get the first worksheet in the workbook
                Worksheet = Package.Workbook.Worksheets.FirstOrDefault();
                var parser = new Parser(Worksheet);
                Schedule.Init();
            }

            Console.WriteLine("Specialities count: {0}/39", Schedule.Specialities.Count);
            Console.WriteLine("Groups count: {0}/50", Schedule.Groups.Count);
            Console.WriteLine("-------------------------------------------");

            foreach (var item in Schedule.Lessons.First(c => c.GroupName == "108").Lessons)
            {
                Console.WriteLine("Subject: {0}", item.Lesson.Name);
                Console.WriteLine("Teacher: {0}", item.Teacher?.Name);
                Console.WriteLine("Auditory: {0}", item.Auditory?.Name);
                Console.WriteLine("-------------------------------------------");

            }

            Console.ReadLine();
        }
    }
}
