using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab03
{
    class Program
    {
        static void Main(string[] args)
        {
            DBTestEntities context = new DBTestEntities();
            foreach (var item in context.Courses)
                Console.WriteLine(item.Id + "\t" 
                    + item.Title + "\t" 
                    + item.Professor.Surname);
        }
    }
}
