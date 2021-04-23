using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab03
{
    class Program
    {
        private static DBTestEntities context = new DBTestEntities();

        static void Main(string[] args)
        {
            WriteLINQ();
            Console.WriteLine("-----------------------");
            WriteSQL();
            Console.ReadLine();
        }

        static void WriteLINQ()
        {
            List<Professor> professors = context.Courses.Select(c => c.Professor).Where(c => c.Name.Length > 5).ToList();
            foreach (var item in professors)
                Console.WriteLine(item.Id + "\t" + item.Surname);
            Console.WriteLine("------------");
            List<Course> courses = context.Courses.OrderByDescending(c => c.Id).ToList();
            foreach (var item in courses)
                Console.WriteLine(item.Id + "\t" + item.Title);
        }

        static void WriteSQL()
        {
            List<Professor> professors = context.Professors.SqlQuery("SELECT * FROM PROFESSOR").ToList();
            foreach (var item in professors)
                Console.WriteLine(item.Id + "\t" + item.Surname);
            Console.WriteLine("------------");
        }

    }
}
