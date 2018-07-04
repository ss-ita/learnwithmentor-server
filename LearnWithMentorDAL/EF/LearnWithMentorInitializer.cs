using System;
using System.Collections.Generic;
using LearnWithMentorDAL.Entities;


namespace LearnWithMentorDAL.EF
{
    class LearnWithMentorInitializer
    {
        public static void Initialize(LearnWithMentor_DBEntities context)
        {
            List<Role> roles = new List<Role>();
            roles.Add(new Role() { Id = 0, Name = "Mentor" });
            roles.Add(new Role() { Id = 1, Name = "Student" });
            roles.Add(new Role() { Id = 2, Name = "Admin" });

            context.Roles.AddRange(roles);

            List<User> users = new List<User>();
            users.Add(new User() { Id = 0, FirstName = "Vyacheslav", LastName = "Koldovsky", Email  = "koldovsky@gmail.com", Blocked = false});
            users.Add(new User() { Id = 1, FirstName = "Khrystyna ", LastName = "Romaniv", Email = "romaniv@gmail.com", Blocked = false });
            users.Add(new User() { Id = 2, FirstName = "Orysia", LastName = "Khoroshchak", Email = "khoroshchak@gmail.com", Blocked = false });
            users.Add(new User() { Id = 3, FirstName = "Lesya", LastName = "Klakovych", Email = "klakovych@gmail.com", Blocked = false });
            users.Add(new User() { Id = 4, FirstName = "Viktoria", LastName = "Ryazhska", Email = "ryazhska@gmail.com", Blocked = false });
            users.Add(new User() { Id = 5, FirstName = "Liubomyr", LastName = "Halamaha", Email = "halamaha@gmail.com", Blocked = false });
            users.Add(new User() { Id = 6, FirstName = "Igor", LastName = "Kohut", Email = "kohut@gmail.com", Blocked = false });
            users.Add(new User() { Id = 7, FirstName = "Andriy", LastName = "Korkuna", Email = "korkuna@gmail.com", Blocked = false });
            users.Add(new User() { Id = 8, FirstName = "Yaroslav", LastName = "Harasym", Email = "harasym@gmail.com", Blocked = false });
            users.Add(new User() { Id = 9, FirstName = "Mykhaylo", LastName = "Plesna", Email = "plesna@gmail.com", Blocked = false });
            users.Add(new User() { Id = 10, FirstName ="Maryana", LastName = "Lopatynska", Email = "lopatynska@gmail.com", Blocked = false });

            users.Add(new User() { Id = 11, FirstName = "Roman", LastName = "Maksymyshyn", Email = "maksymyshyn@gmail.com", Blocked = false });
            users.Add(new User() { Id = 12, FirstName = "Yurii-Stefan", LastName = "Zhydetskyi", Email = "zhydetskyi@gmail.com", Blocked = false });
            users.Add(new User() { Id = 13, FirstName = "Oleksandr", LastName = "Isakov", Email = "isakov@gmail.com", Blocked = false });
            users.Add(new User() { Id = 14, FirstName = "Roman", LastName = "Parobii", Email = "parobii@gmail.com", Blocked = false });
            users.Add(new User() { Id = 15, FirstName = "Andrii", LastName = "Lysyi", Email = "lysyi@gmail.com", Blocked = false });
            users.Add(new User() { Id = 16, FirstName = "Andrii", LastName = "Panchyshyn", Email = "panchyshyn@gmail.com", Blocked = false });
            users.Add(new User() { Id = 17, FirstName = "Yulia", LastName = "Pavlyk", Email = "pavlyk@gmail.com", Blocked = false });
            users.Add(new User() { Id = 18, FirstName = "Karim", LastName = "Benkhenni", Email = "benkhenni@gmail.com", Blocked = false });
            users.Add(new User() { Id = 19, FirstName = "Pedro", LastName = "Alvares", Email = "alvares@gmail.com", Blocked = false });
            users.Add(new User() { Id = 20, FirstName = "Dmytro", LastName = "Chalyi", Email = "chalyi@gmail.com", Blocked = false });
            users.Add(new User() { Id = 21, FirstName = "Adriana", LastName = "Prudyvus", Email = "prudyvus@gmail.com", Blocked = false });
            users.Add(new User() { Id = 22, FirstName = "Yaromyr", LastName = "Oryshchyn", Email = "oryshchyn@gmail.com", Blocked = false });
            users.Add(new User() { Id = 23, FirstName = "Andrii", LastName = "Danyliuk", Email = "danyliuk@gmail.com", Blocked = false });
            users.Add(new User() { Id = 24, FirstName = "Maksym", LastName = "Prytyka", Email = "prytyka@gmail.com", Blocked = false });
            users.Add(new User() { Id = 25, FirstName = "Mykhailo", LastName = "Kyzyma", Email = "kyzyma@gmail.com", Blocked = false });
            users.Add(new User() { Id = 26, FirstName = "Dmytro", LastName = "Khomyk", Email = "khomyk@gmail.com", Blocked = false });
            users.Add(new User() { Id = 27, FirstName = "Pavlo", LastName = "Kruk", Email = "kruk@gmail.com", Blocked = false });
            users.Add(new User() { Id = 28, FirstName = "Kateryna", LastName = "Obrizan", Email = "obrizan@gmail.com", Blocked = false });
            users.Add(new User() { Id = 29, FirstName = "Viktor", LastName = "Levak", Email = "levak@gmail.com", Blocked = false });
            users.Add(new User() { Id = 30, FirstName = "Oleksandr", LastName = "Mykhalchuk", Email = "mykhalchuk@gmail.com", Blocked = false });

            for (int i = 0; i <= 10; i++)
            {
                users[i].Role_Id = 0;
            }

            for (int i = 11; i <= users.Count; i++)
            {
                users[i].Role_Id = 1;
            }

            context.Users.AddRange(users);
                       

            List<Plan> plans = new List<Plan>();
            plans.Add(new Plan() { Id = 0, Name = "C# Essential Training", Description = "Takes you through C#'s history, its core syntax, and the fundamentals of writing strong C# code." });
            plans.Add(new Plan() { Id = 1, Name = "ASP.NET Core", Description = "In this practical course, you will learn how to build a line-of-business, enterprise application with ASP.NET Core MVC, including topics such as security, logging, testing, validation, and much more." });
            plans.Add(new Plan() { Id = 2, Name = "Angular Complete Guide", Description = "Use their gained, deep understanding of the Angular 6 fundamentals to quickly establish themselves as frontend developers" });
            plans.Add(new Plan() { Id = 3, Name = "Angular Material Complete Guide", Description = "We'll build an entire, realistic app which looks absolutely beautiful, uses Google's Material Design and is extremely fast! Thanks to Firebase and Angularfire, we'll add real-time database functionalities and see our updates almost before we make them!" });
            plans.Add(new Plan() { Id = 4, Name = "SQL & Database Design: Learn MS SQL Server", Description = "In this course you will learn how to create queries in a MS SQL Management Studio" });
            plans.Add(new Plan() { Id = 5, Name = "Building Applications with Angular, ASP.NET Core, and Entity Framework Core", Description = "Creating a complete full-stack application requires integrating multiple components. The front-end piece must talk smoothly to the server, and within the server, you'll need multiple layers: one to talk with the client, and one to store information on the server. This course teaches integration through the perspective of a quiz project, with instructor Alexander Zanfir showing how to use Angular, ASP.NET Core, and Entity Framework Core to develop a full-stack application. Alexander explains how to display and edit data in Angular with ASP.NET Core, create forms, navigate to different views, and more." });
            plans.Add(new Plan() { Id = 6, Name = "The complete React Fullstack course", Description = "You will learn the whole React WebApp building process, from your pc to the server." });
            plans.Add(new Plan() { Id = 7, Name = "Java Essential Training", Description = " This course provides the foundation for learning Java SE (Standard Edition), so you can build your first apps or start exploring the language on your own." });
            plans.Add(new Plan() { Id = 8, Name = "The Complete JavaScript Course", Description = "JavaScript and programming fundamentals: variables, boolean logic, if/else, loops, functions, arrays, etc. A true understanding of how JavaScript works behind the scenes." });
            plans.Add(new Plan() { Id = 9, Name = "C++ Essential Training", Description = "Widely used for both systems and applications development, the C and C++ programming languages are available for virtually every operating system and are often the best choice for performance-critical applications. In this course, Bill Weinman dissects the anatomy of C and C++, from variables to functions and loops, and explores both the C Standard Library and the C++ Standard Template Library. " });
            plans.Add(new Plan() { Id = 10, Name = "Python 3 Course: Beginner to Advanced", Description = "This course is designed to fully immerse you in the Python language, so it is great for both beginners and veteran programmers! Learn Python through the basics of programming, advanced Python concepts, coding a calculator, essential modules, web scraping, PyMongo, WebPy development, Django web framework, GUI programming, data visualization, machine learning." });

            foreach (var plan in plans)
            {
                plan.Create_Date = new DateTime(2018, 7, 3);
            }

            for (int i = 0; i < plans.Count; i++)
            {
                plans[i].Creator = users[i];
            }


            context.Plans.AddRange(plans);

            List<Group> groups = new List<Group>();
            groups.Add(new Group() { Id = 0, Name = "Lv-319.Net" });
            groups.Add(new Group() { Id = 1, Name = "Lv-320.Net" });
            groups.Add(new Group() { Id = 3, Name = "Lv-321.Web" });
            groups.Add(new Group() { Id = 4, Name = "Lv-322.Web" });
            groups.Add(new Group() { Id = 5, Name = "Lv-323.SQL" });
            groups.Add(new Group() { Id = 6, Name = "Lv-324.Net" });
            groups.Add(new Group() { Id = 7, Name = "Lv-325.Web" });
            groups.Add(new Group() { Id = 8, Name = "Lv-326.Web" });
            groups.Add(new Group() { Id = 9, Name = "Lv-327.C++" });
            groups.Add(new Group() { Id = 10, Name = "Lv-328.Python" });

            for (int i = 0; i < groups.Count; i++)
            {
                groups[i].Mentor_Id = i;
            }

            context.Groups.AddRange(groups);

            List<Task> tasks = new List<Task>();
            tasks.Add(new Task() { Id = 0, Name = "C# Installing Visual Studio." });
            tasks.Add(new Task() { Id = 1, Name = "C# Variables, Loops." });
            tasks.Add(new Task() { Id = 2, Name = "C# Array." });
            tasks.Add(new Task() { Id = 3, Name = "C# Class, Method, Exception." });
            tasks.Add(new Task() { Id = 4, Name = "ASP.NET Controllers." });
            tasks.Add(new Task() { Id = 5, Name = "ASP.NET Routes." });
            tasks.Add(new Task() { Id = 6, Name = "ASP.NET View." });
            tasks.Add(new Task() { Id = 7, Name = "ASP.NET Making App." });
            tasks.Add(new Task() { Id = 8, Name = "Angular. Installing Atom." });
            tasks.Add(new Task() { Id = 9, Name = "Angular. App component." });
            tasks.Add(new Task() { Id = 10, Name = "Angular. Services." });
            tasks.Add(new Task() { Id = 11, Name = "Angular. Observables." });
            tasks.Add(new Task() { Id = 12, Name = "Angular Material. Installing Angular material" });
            tasks.Add(new Task() { Id = 13, Name = "Angular Material. Adding NavBar" });
            tasks.Add(new Task() { Id = 14, Name = "Angular Material. Working on welcome screen" });
            tasks.Add(new Task() { Id = 15, Name = "Angular Material. Working with Data and Angular Material" });
            tasks.Add(new Task() { Id = 16, Name = "SQL. Installing Microsoft SQL Server and Microsoft SQL Management studio" });
            tasks.Add(new Task() { Id = 17, Name = "SQL. Creating databases and tables." });
            tasks.Add(new Task() { Id = 18, Name = "SQL. Data insertion and manipulation." });
            tasks.Add(new Task() { Id = 19, Name = "SQL. JOIN" });
            tasks.Add(new Task() { Id = 20, Name = "WEB App. Modeling app structure" });
            tasks.Add(new Task() { Id = 21, Name = "WEB App. Creating Database" });
            tasks.Add(new Task() { Id = 22, Name = "WEB App. Creating Data Access layer and business logic layer." });
            tasks.Add(new Task() { Id = 23, Name = "WEB App. Creating fron-end part in angular" });
            tasks.Add(new Task() { Id = 24, Name = "React. React basics" });
            tasks.Add(new Task() { Id = 25, Name = "React. Routes, Lifecycle" });
            tasks.Add(new Task() { Id = 26, Name = "React. Transitions & Typechecking" });
            tasks.Add(new Task() { Id = 27, Name = "React. Working with forms" });
            tasks.Add(new Task() { Id = 28, Name = "Java. Getting Started. " });
            tasks.Add(new Task() { Id = 29, Name = "Java. Working with Variables." });
            tasks.Add(new Task() { Id = 30, Name = "Java. Working with Objects." });
            tasks.Add(new Task() { Id = 31, Name = "Java. Exception Handling and Debugging." });
            tasks.Add(new Task() { Id = 32, Name = "JavaScript. Language Basics" });
            tasks.Add(new Task() { Id = 34, Name = "JavaScript. How JavaScript Works Behind the Scenes" });
            tasks.Add(new Task() { Id = 35, Name = "JavaScript. DOM Manipulation and Events" });
            tasks.Add(new Task() { Id = 36, Name = "JavaScript. Objects and Functions" });
            tasks.Add(new Task() { Id = 37, Name = "C++. Basic Syntax. " });
            tasks.Add(new Task() { Id = 38, Name = "C++. Defining Functions" });
            tasks.Add(new Task() { Id = 39, Name = "C++. The Preprocessor" });
            tasks.Add(new Task() { Id = 40, Name = "C++. Classes and Objects" });
            tasks.Add(new Task() { Id = 37, Name = "Python. Programming Basics" });
            tasks.Add(new Task() { Id = 38, Name = "Python. Advanced Concepts" });
            tasks.Add(new Task() { Id = 39, Name = "Python. RPG Battle Script" });
            tasks.Add(new Task() { Id = 40, Name = "Python. Web Scraper" });

            PlanTask planTask = new PlanTask();
            planTask.Plan_Id = plans[0].Id;


        }
    }
}

