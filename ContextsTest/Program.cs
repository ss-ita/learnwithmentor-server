﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnWithMentorDAL.Entities;

namespace ContextsTest
{
    class Program
    {
        static void Main(string[] args)
        {


            using (LearnWithMentor_DBEntities db = new LearnWithMentor_DBEntities())
            {
                foreach (var g in db.Users)
                    Console.WriteLine("User: {0} {1}", g.FirstName, g.LastName);
            }

            Console.ReadKey();
        }
    }
}
