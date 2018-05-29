using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.Models;

namespace ContextsTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //using (LearnWithMentorModelContext db = new LearnWithMentorModelContext())
            //{
            //    var coll = db.Groups;
            //    foreach (var g in coll)
            //        Console.WriteLine("Group: {0} with Id {1}", g.Name, g.Id);
            //}
            //Console.WriteLine("--------------------------------------------------------");
            using (LearnWithMentorContext db = new LearnWithMentorContext())
            {
                var coll = db.Groups;
                foreach (var g in coll)
                    Console.WriteLine("Group: {0} with Id {1}", g.Name, g.Id);
            }

            Console.ReadKey();
        }
    }
}
