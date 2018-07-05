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
            roles.Add(new Role() { Name = "Mentor" });
            roles.Add(new Role() { Name = "Student" });
            roles.Add(new Role() { Name = "Admin" });

            context.Roles.AddRange(roles);

            #region List of Users
            List<User> users = new List<User>();
            users.Add(new User() { FirstName = "Vyacheslav", LastName = "Koldovsky", Email = "koldovsky@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123"), Blocked = false });
            users.Add(new User() { FirstName = "Khrystyna ", LastName = "Romaniv", Email = "romaniv@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123"), Blocked = false });
            users.Add(new User() { FirstName = "Orysia", LastName = "Khoroshchak", Email = "khoroshchak@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123"), Blocked = false });
            users.Add(new User() { FirstName = "Lesya", LastName = "Klakovych", Email = "klakovych@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123"), Blocked = false });
            users.Add(new User() { FirstName = "Viktoria", LastName = "Ryazhska", Email = "ryazhska@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123"), Blocked = false });
            users.Add(new User() { FirstName = "Liubomyr", LastName = "Halamaha", Email = "halamaha@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123"), Blocked = false });
            users.Add(new User() { FirstName = "Igor", LastName = "Kohut", Email = "kohut@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123"), Blocked = false });
            users.Add(new User() { FirstName = "Andriy", LastName = "Korkuna", Email = "korkuna@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123"), Blocked = false });
            users.Add(new User() { FirstName = "Yaroslav", LastName = "Harasym", Email = "harasym@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123"), Blocked = false });
            users.Add(new User() { FirstName = "Mykhaylo", LastName = "Plesna", Email = "plesna@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123"), Blocked = false });
            users.Add(new User() { FirstName = "Maryana", LastName = "Lopatynska", Email = "lopatynska@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123"), Blocked = false });

            users.Add(new User() { FirstName = "Roman", LastName = "Maksymyshyn", Email = "maksymyshyn@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123"), Blocked = false });
            users.Add(new User() { FirstName = "Yurii-Stefan", LastName = "Zhydetskyi", Email = "zhydetskyi@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123"), Blocked = false });
            users.Add(new User() { FirstName = "Oleksandr", LastName = "Isakov", Email = "isakov@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123"), Blocked = false });
            users.Add(new User() { FirstName = "Roman", LastName = "Parobii", Email = "parobii@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123"), Blocked = false });
            users.Add(new User() { FirstName = "Andrii", LastName = "Lysyi", Email = "lysyi@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123"), Blocked = false });
            users.Add(new User() { FirstName = "Andrii", LastName = "Panchyshyn", Email = "panchyshyn@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123"), Blocked = false });
            users.Add(new User() { FirstName = "Yulia", LastName = "Pavlyk", Email = "pavlyk@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123"), Blocked = false });
            users.Add(new User() { FirstName = "Karim", LastName = "Benkhenni", Email = "benkhenni@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123"), Blocked = false });
            users.Add(new User() { FirstName = "Pedro", LastName = "Alvares", Email = "alvares@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123"), Blocked = false });
            users.Add(new User() { FirstName = "Dmytro", LastName = "Chalyi", Email = "chalyi@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123"), Blocked = false });
            users.Add(new User() { FirstName = "Adriana", LastName = "Prudyvus", Email = "prudyvus@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123"), Blocked = false });
            users.Add(new User() { FirstName = "Yaromyr", LastName = "Oryshchyn", Email = "oryshchyn@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123"), Blocked = false });
            users.Add(new User() { FirstName = "Andrii", LastName = "Danyliuk", Email = "danyliuk@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123"), Blocked = false });
            users.Add(new User() { FirstName = "Maksym", LastName = "Prytyka", Email = "prytyka@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123"), Blocked = false });
            users.Add(new User() { FirstName = "Mykhailo", LastName = "Kyzyma", Email = "kyzyma@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123"), Blocked = false });
            users.Add(new User() { FirstName = "Dmytro", LastName = "Khomyk", Email = "khomyk@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123"), Blocked = false });
            users.Add(new User() { FirstName = "Pavlo", LastName = "Kruk", Email = "kruk@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123"), Blocked = false });
            users.Add(new User() { FirstName = "Kateryna", LastName = "Obrizan", Email = "obrizan@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123"), Blocked = false });
            users.Add(new User() { FirstName = "Viktor", LastName = "Levak", Email = "levak@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123"), Blocked = false });
            users.Add(new User() { FirstName = "Oleksandr", LastName = "Mykhalchuk", Email = "mykhalchuk@gmail.com", Password = BCrypt.Net.BCrypt.HashPassword("123"), Blocked = false });
            #endregion

            //Assigning mentor roles for first 10 users
            for (int i = 0; i <= 10; i++)
            {
                users[i].Roles = roles[0];
            }
            //Assigning student roles for rest of the users
            for (int i = 11; i <= users.Count; i++)
            {
                users[i].Roles = roles[1];
            }

            context.Users.AddRange(users);

            #region List of Plans
            List<Plan> plans = new List<Plan>();
            plans.Add(new Plan() { Name = "C# Essential Training", Description = "Takes you through C#'s history, its core syntax, and the fundamentals of writing strong C# code." });
            plans.Add(new Plan() { Name = "ASP.NET", Description = "In this practical course, you will learn how to build a line-of-business, enterprise application with ASP.NET Core MVC, including topics such as security, logging, testing, validation, and much more." });
            plans.Add(new Plan() { Name = "Angular Guide", Description = "Use their gained, deep understanding of the Angular 6 fundamentals to quickly establish themselves as frontend developers" });
            plans.Add(new Plan() { Name = "Angular Material Guide", Description = "We'll build an entire, realistic app which looks absolutely beautiful, uses Google's Material Design and is extremely fast! Thanks to Firebase and Angularfire, we'll add real-time database functionalities and see our updates almost before we make them!" });
            plans.Add(new Plan() { Name = "SQL & Database Design: Learn MS SQL Server", Description = "In this course you will learn how to create queries in a MS SQL Management Studio" });
            plans.Add(new Plan() { Name = "Building Applications with Angular, ASP.NET Core, and Entity Framework Core", Description = "Creating a complete full-stack application requires integrating multiple components. The front-end piece must talk smoothly to the server, and within the server, you'll need multiple layers: one to talk with the client, and one to store information on the server. This course teaches integration through the perspective of a quiz project, with instructor Alexander Zanfir showing how to use Angular, ASP.NET Core, and Entity Framework Core to develop a full-stack application. Alexander explains how to display and edit data in Angular with ASP.NET Core, create forms, navigate to different views, and more." });
            plans.Add(new Plan() { Name = "The complete React Fullstack course", Description = "You will learn the whole React WebApp building process, from your pc to the server." });
            plans.Add(new Plan() { Name = "Java Essential Training", Description = " This course provides the foundation for learning Java SE (Standard Edition), so you can build your first apps or start exploring the language on your own." });
            plans.Add(new Plan() { Name = "The Complete JavaScript Course", Description = "JavaScript and programming fundamentals: variables, boolean logic, if/else, loops, functions, arrays, etc. A true understanding of how JavaScript works behind the scenes." });
            plans.Add(new Plan() { Name = "C++ Essential Training", Description = "Widely used for both systems and applications development, the C and C++ programming languages are available for virtually every operating system and are often the best choice for performance-critical applications. In this course, Bill Weinman dissects the anatomy of C and C++, from variables to functions and loops, and explores both the C Standard Library and the C++ Standard Template Library. " });
            plans.Add(new Plan() { Name = "Python 3 Course: Beginner to Advanced", Description = "This course is designed to fully immerse you in the Python language, so it is great for both beginners and veteran programmers! Learn Python through the basics of programming, advanced Python concepts, coding a calculator, essential modules, web scraping, PyMongo, WebPy development, Django web framework, GUI programming, data visualization, machine learning." });
            #endregion

            //Assigning creation date for plans on third of july
            foreach (var plan in plans)
            {
                plan.Create_Date = new DateTime(2018, 7, 3);
            }
            //Assigning creator of plans on each user by one
            for (int i = 0; i < 10; i++)
            {
                plans[i].Creator = users[i];
            }

            context.Plans.AddRange(plans);

            #region List of groups
            List<Group> groups = new List<Group>();
            groups.Add(new Group() { Name = "Lv-319.Net" });
            groups.Add(new Group() { Name = "Lv-320.Net" });
            groups.Add(new Group() { Name = "Lv-321.Web" });
            groups.Add(new Group() { Name = "Lv-322.Web" });
            groups.Add(new Group() { Name = "Lv-323.SQL" });
            groups.Add(new Group() { Name = "Lv-324.Net" });
            groups.Add(new Group() { Name = "Lv-325.Web" });
            groups.Add(new Group() { Name = "Lv-326.Web" });
            groups.Add(new Group() { Name = "Lv-327.C++" });
            groups.Add(new Group() { Name = "Lv-328.Python" });
            #endregion

            //Assigning mentors for groups
            for (int i = 0; i < groups.Count; i++)
            {
                groups[i].Mentor = users[i];
            }

            context.Groups.AddRange(groups);

            #region List of Tasks
            List<Task> tasks = new List<Task>();
            tasks.Add(new Task() { Name = "C#. Installing Visual Studio." });
            tasks.Add(new Task() { Name = "C#. Variables, Loops." });
            tasks.Add(new Task() { Name = "C#. Array." });
            tasks.Add(new Task() { Name = "C#. Class, Method, Exception." });
            tasks.Add(new Task() { Name = "ASP NET Controllers." });
            tasks.Add(new Task() { Name = "ASP.NET Routes." });
            tasks.Add(new Task() { Name = "ASP.NET View." });
            tasks.Add(new Task() { Name = "ASP.NET Making App." });
            tasks.Add(new Task() { Name = "Angular. Installing Atom." });
            tasks.Add(new Task() { Name = "Angular. App component." });
            tasks.Add(new Task() { Name = "Angular. Services." });
            tasks.Add(new Task() { Name = "Angular. Observables." });
            tasks.Add(new Task() { Name = "Angular Material. Installing Angular material" });
            tasks.Add(new Task() { Name = "Angular Material. Adding NavBar" });
            tasks.Add(new Task() { Name = "Angular Material. Working on welcome screen" });
            tasks.Add(new Task() { Name = "Angular Material. Working with Data and Angular Material" });
            tasks.Add(new Task() { Name = "SQL. Installing Microsoft SQL Server and Microsoft SQL Management studio" });
            tasks.Add(new Task() { Name = "SQL. Creating databases and tables." });
            tasks.Add(new Task() { Name = "SQL. Data insertion and manipulation." });
            tasks.Add(new Task() { Name = "SQL. JOIN" });
            tasks.Add(new Task() { Name = "Full stack WEB App. Modeling app structure" });
            tasks.Add(new Task() { Name = "Full stack WEB App. Creating Database" });
            tasks.Add(new Task() { Name = "Full stack WEB App. Creating Data Access layer and business logic layer." });
            tasks.Add(new Task() { Name = "Full stack WEB App. Creating fron-end part in angular" });
            tasks.Add(new Task() { Name = "React. React basics" });
            tasks.Add(new Task() { Name = "React. Routes, Lifecycle" });
            tasks.Add(new Task() { Name = "React. Transitions & Typechecking" });
            tasks.Add(new Task() { Name = "React. Working with forms" });
            tasks.Add(new Task() { Name = "Java. Getting Started. " });
            tasks.Add(new Task() { Name = "Java. Working with Variables." });
            tasks.Add(new Task() { Name = "Java. Working with Objects." });
            tasks.Add(new Task() { Name = "Java. Exception Handling and Debugging." });
            tasks.Add(new Task() { Name = "JavaScript. Language Basics" });
            tasks.Add(new Task() { Name = "JavaScript. How JavaScript Works Behind the Scenes" });
            tasks.Add(new Task() { Name = "JavaScript. DOM Manipulation and Events" });
            tasks.Add(new Task() { Name = "JavaScript. Objects and Functions" });
            tasks.Add(new Task() { Name = "C++. Basic Syntax. " });
            tasks.Add(new Task() { Name = "C++. Defining Functions" });
            tasks.Add(new Task() { Name = "C++. The Preprocessor" });
            tasks.Add(new Task() { Name = "C++. Classes and Objects" });
            tasks.Add(new Task() { Name = "Python. Programming Basics" });
            tasks.Add(new Task() { Name = "Python. Advanced Concepts" });
            tasks.Add(new Task() { Name = "Python. RPG Battle Script" });
            tasks.Add(new Task() { Name = "Python. Web Scraper" });
            #endregion


            //Assigning creators of tasks
            //Each creator created 4 plans
            int count = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = count; j < count + 4; j++)
                {
                    tasks[j].Creator = users[i];
                }
                count = count + 4;
            }
            //Setting tasks as public
            foreach (var task in tasks)
            {
                task.Private = false;
            }

            List<PlanTask> planTasks = new List<PlanTask>();


        }
    }
}

