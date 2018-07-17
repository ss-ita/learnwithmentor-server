using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LearnWithMentorDAL.Entities;


namespace LearnWithMentorDAL.EF
{
    public class LearnWithMentorInitializer
    {
        public static void Initialize()
        {
            using (LearnWithMentor_DBEntities context = new LearnWithMentor_DBEntities())
            {

                context.Database.ExecuteSqlCommand("delete PlanSuggestion;");
                context.Database.ExecuteSqlCommand("delete Messages;");
                context.Database.ExecuteSqlCommand("delete Comments;");
                context.Database.ExecuteSqlCommand("delete UserTasks;");
                context.Database.ExecuteSqlCommand("delete PlanTasks;");
                context.Database.ExecuteSqlCommand("delete Tasks;");
                context.Database.ExecuteSqlCommand("delete GroupPlans;");
                context.Database.ExecuteSqlCommand("delete Sections;");
                context.Database.ExecuteSqlCommand("delete Plans;");
                context.Database.ExecuteSqlCommand("delete UserGroups;");
                context.Database.ExecuteSqlCommand("delete Groups;");
                context.Database.ExecuteSqlCommand("delete Users;");
                context.Database.ExecuteSqlCommand("delete Roles;");

                InitializeRoles(context);
                InitializeUsers(context);
                InitializePlans(context);
                InitializeGroups(context);
                InitializeTasks(context);
                InitializeSections(context);
                InitializePlanTask(context);
                InitializeUserTasks(context);
                InitializeMessages(context);
                InitializeComments(context);
                InitializePlanSugestion(context);
            }
        }

        public static void InitializeRoles(LearnWithMentor_DBEntities context)
        {
            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('Roles', RESEED, 0)");
            List<Role> roles = new List<Role>();
            roles.Add(new Role() { Id = 1, Name = "Mentor" });
            roles.Add(new Role() { Id = 2, Name = "Student" });
            roles.Add(new Role() { Id = 3, Name = "Admin" });
            context.Roles.AddRange(roles);
            context.SaveChanges();
        }

        private static void InitializeUsers(LearnWithMentor_DBEntities context)
        {
            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('Users', RESEED, 0)");

            List<User> users = new List<User>();

            #region List of Mentors
            users.Add(new User() { FirstName = "Vyacheslav", LastName = "Koldovsky", Email = "koldovsky@gmail.com" });//1
            users.Add(new User() { FirstName = "Khrystyna ", LastName = "Romaniv", Email = "romaniv@gmail.com" });//2
            users.Add(new User() { FirstName = "Orysia", LastName = "Khoroshchak", Email = "khoroshchak@gmail.com" });//3
            users.Add(new User() { FirstName = "Lesya", LastName = "Klakovych", Email = "klakovych@gmail.com" });//4
            users.Add(new User() { FirstName = "Viktoria", LastName = "Ryazhska", Email = "ryazhska@gmail.com" });//5
            users.Add(new User() { FirstName = "Liubomyr", LastName = "Halamaha", Email = "halamaha@gmail.com" });//6
            users.Add(new User() { FirstName = "Igor", LastName = "Kohut", Email = "kohut@gmail.com" });//7
            users.Add(new User() { FirstName = "Andriy", LastName = "Korkuna", Email = "korkuna@gmail.com" });//8
            users.Add(new User() { FirstName = "Yaroslav", LastName = "Harasym", Email = "harasym@gmail.com" });//9
            users.Add(new User() { FirstName = "Mykhaylo", LastName = "Plesna", Email = "plesna@gmail.com" });//10
            users.Add(new User() { FirstName = "Maryana", LastName = "Lopatynska", Email = "lopatynska@gmail.com" });//11
            #endregion

            #region List of Students
            users.Add(new User() { FirstName = "Roman", LastName = "Maksymyshyn", Email = "maksymyshyn@gmail.com" });//12
            users.Add(new User() { FirstName = "Yurii-Stefan", LastName = "Zhydetskyi", Email = "zhydetskyi@gmail.com" });//13
            users.Add(new User() { FirstName = "Oleksandr", LastName = "Isakov", Email = "isakov@gmail.com" });//14
            users.Add(new User() { FirstName = "Roman", LastName = "Parobii", Email = "parobii@gmail.com" });//15
            users.Add(new User() { FirstName = "Andrii", LastName = "Lysyi", Email = "lysyi@gmail.com" });//16
            users.Add(new User() { FirstName = "Andrii", LastName = "Panchyshyn", Email = "panchyshyn@gmail.com" });//17
            users.Add(new User() { FirstName = "Yulia", LastName = "Pavlyk", Email = "pavlyk@gmail.com" });//18________
            users.Add(new User() { FirstName = "Karim", LastName = "Benkhenni", Email = "benkhenni@gmail.com" });//19
            users.Add(new User() { FirstName = "Pedro", LastName = "Alvares", Email = "alvares@gmail.com" });//20
            users.Add(new User() { FirstName = "Dmytro", LastName = "Chalyi", Email = "chalyi@gmail.com" });//21
            users.Add(new User() { FirstName = "Adriana", LastName = "Prudyvus", Email = "prudyvus@gmail.com" });//21
            users.Add(new User() { FirstName = "Yaromyr", LastName = "Oryshchyn", Email = "oryshchyn@gmail.com" });//22
            users.Add(new User() { FirstName = "Andrii", LastName = "Danyliuk", Email = "danyliuk@gmail.com" });//23
            users.Add(new User() { FirstName = "Maksym", LastName = "Prytyka", Email = "prytyka@gmail.com" });//24_____
            users.Add(new User() { FirstName = "Mykhailo", LastName = "Kyzyma", Email = "kyzyma@gmail.com" });//25
            users.Add(new User() { FirstName = "Dmytro", LastName = "Khomyk", Email = "khomyk@gmail.com" });//26
            users.Add(new User() { FirstName = "Pavlo", LastName = "Kruk", Email = "kruk@gmail.com" });//27
            users.Add(new User() { FirstName = "Kateryna", LastName = "Obrizan", Email = "obrizan@gmail.com" });//28
            users.Add(new User() { FirstName = "Viktor", LastName = "Levak", Email = "levak@gmail.com" });//29
            users.Add(new User() { FirstName = "Oleksandr", LastName = "Mykhalchuk", Email = "mykhalchuk@gmail.com" });//30
            users.Add(new User() { FirstName = "El", LastName = "Admino", Email = "admino@gmail.com" });//31
            users.Add(new User() { FirstName = "La", LastName = "Admina", Email = "admina@gmail.com" });//32
            #endregion

            //Assigning custom passwords and  blocked status "false" for users
            foreach (var user in users)
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword("123");
                user.Blocked = false;
            }

            
            List<ImageReader> readedImages = new List<ImageReader>();
            string dir = (AppDomain.CurrentDomain.BaseDirectory).Replace("LearnWithMentor", String.Empty);
            string path = Path.Combine(dir, @"LearnWithMentorDAL\EF\stringImages.txt");
            using (StreamReader reader = new StreamReader(path))
            {
                string record;
                int count = 0;
                while ((record = reader.ReadLine()) != null)
                {
                    readedImages.Add(ImageReader.Parse(record));
                }
            }
            


            //Assigning mentor roles for first 11 users
            for (int i = 0; i <= 10; i++)
            {
                users[i].Role_Id = 1;
                users[i].Image_Name = readedImages[0].Name;
                users[i].Image = readedImages[0].ImageEncoded;
            }

            //Assigning student roles for next 20 users
            for (int i = 11; i <= 29; i++)
            {
                users[i].Role_Id = 2;
                users[i].Image_Name = readedImages[1].Name;
                users[i].Image = readedImages[1].ImageEncoded;
            }

            //Assigning admin roles for rest of the users
            for (int i = 30; i < users.Count; i++)
            {
                users[i].Role_Id = 3;
                users[i].Image_Name = readedImages[2].Name;
                users[i].Image = readedImages[2].ImageEncoded; ;
            }
            context.Users.AddRange(users);
            context.SaveChanges();

        }

        private static void InitializePlans(LearnWithMentor_DBEntities context)
        {
            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('[Plans]', RESEED, 0)");

            List<ImageReader> readedImages = new List<ImageReader>();
            string dir = (AppDomain.CurrentDomain.BaseDirectory).Replace("LearnWithMentor", String.Empty);
            string path = Path.Combine(dir, @"LearnWithMentorDAL\EF\stringImages.txt");
            using (StreamReader reader = new StreamReader(path))
            {
                string record;
                int count = 0;
                while ((record = reader.ReadLine()) != null)
                {
                    readedImages.Add(ImageReader.Parse(record));
                }
            }

            List<Plan> plans = new List<Plan>();
            plans.Add(new Plan()
            {
                Name = "C# Essential Training",
                Description =
                    "Takes you through C#'s history, it's core syntax, and the fundamentals of writing strong C# code.",
                Image_Name = readedImages[3].Name,
                Image = readedImages[3].ImageEncoded
            });
            plans.Add(new Plan()
            {
                Name = "ASP.NET",
                Description =
                    "In this practical course, you will learn how to build a line-of-business, enterprise application with ASP.NET Core MVC, including topics such as security, logging, testing, validation, and much more.",
                Image_Name = readedImages[4].Name,
                Image = readedImages[4].ImageEncoded
            });
            plans.Add(new Plan()
            {
                Name = "Angular Guide",
                Description =
                    "Use their gained, deep understanding of the Angular 6 fundamentals to quickly establish themselves as frontend developers",
                Image_Name = readedImages[5].Name,
                Image = readedImages[5].ImageEncoded

            });
            plans.Add(new Plan()
            {
                Name = "Angular Material Guide",
                Description =
                    "We'll build an entire, realistic app which looks absolutely beautiful, uses Google's Material Design and is extremely fast! Thanks to Firebase and Angularfire, we'll add real-time database functionalities and see our updates almost before we make them!",
                Image_Name = readedImages[6].Name,
                Image = readedImages[6].ImageEncoded
            });
            plans.Add(new Plan()
            {
                Name = "SQL & Database Design: Learn MS SQL Server",
                Description = "In this course you will learn how to create queries in a MS SQL Management Studio",
                Image_Name = readedImages[7].Name,
                Image = readedImages[7].ImageEncoded
            });
            plans.Add(new Plan()
            {
                Name = "Building Full Stack Applications",
                Description =
                    "Creating a complete full-stack application requires integrating multiple components.  This plan teaches integration through the perspective of a quiz project, using Angular, ASP.NET Core, and Entity Framework Core to develop a full-stack application.",
                Image_Name = readedImages[8].Name,
                Image = readedImages[8].ImageEncoded
            });
            plans.Add(new Plan()
            {
                Name = "The complete React course",
                Description = "You will learn the whole React WebApp building process, from your pc to the server.",
                Image_Name = readedImages[9].Name,
                Image = readedImages[9].ImageEncoded
            });
            plans.Add(new Plan()
            {
                Name = "Java Essential Training",
                Description =
                    " This course provides the foundation for learning Java SE (Standard Edition), so you can build your first apps or start exploring the language on your own.",
                Image_Name = readedImages[10].Name,
                Image = readedImages[10].ImageEncoded
            });
            plans.Add(new Plan()
            {
                Name = "The Complete JavaScript Course",
                Description =
                    "JavaScript and programming fundamentals: variables, boolean logic, if/else, loops, functions, arrays, etc. A true understanding of how JavaScript works behind the scenes.",
                Image_Name = readedImages[11].Name,
                Image = readedImages[11].ImageEncoded
            });
            plans.Add(new Plan()
            {
                Name = "C++ Essential Training",
                Description =
                    "Widely used for both systems and applications development, the C and C++ programming languages are available for virtually every operating system and are often the best choice for performance-critical applications. In this course, Bill Weinman dissects the anatomy of C and C++, from variables to functions and loops, and explores both the C Standard Library and the C++ Standard Template Library.",
                Image_Name = readedImages[12].Name,
                Image = readedImages[12].ImageEncoded
            });
            plans.Add(new Plan()
            {
                Name = "Python Course: Beginner to Advanced",
                Description =
                    "This course is designed to fully immerse you in the Python language, so it is great for both beginners and veteran programmers! Learn Python through the basics of programming, advanced Python concepts, coding a calculator, essential modules, web scraping, PyMongo, WebPy development, Django web framework, GUI programming, data visualization, machine learning.",
                Image_Name = readedImages[13].Name,
                Image = readedImages[13].ImageEncoded
            });

            //Assigning Id's for plans
            for (int i = 0, j = 1; i < plans.Count; i++, j++)
            {
                plans[i].Id = j;
            }

            //Assigning creation date for plans on third of july
            foreach (var plan in plans)
            {
                plan.Create_Date = new DateTime(2018, 7, 3, 23, 59, 59);
            }

            //Assigning modification date for plans on third of july
            foreach (var plan in plans)
            {
                plan.Mod_Date = new DateTime(2018, 7, 16, 23, 59, 59);
            }

            //Assigning creator of plans on each user by one
            foreach (var plan in plans)
            {
                plan.Create_Id = plan.Id;
                plan.Mod_Id = plan.Id;
            }

            context.Plans.AddRange(plans);
            context.SaveChanges();

        }

        private static void InitializeGroups(LearnWithMentor_DBEntities context)
        {
            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('[Groups]', RESEED, 0)");

            #region List of groups
            List<Group> groups = new List<Group>();
            groups.Add(new Group() { Name = "Lv-319.Net" });
            groups.Add(new Group() { Name = "Lv-320.Net" });
            groups.Add(new Group() { Name = "Lv-321.Net" });
            groups.Add(new Group() { Name = "Lv-322.Web" });
            groups.Add(new Group() { Name = "Lv-323.SQL" });
            groups.Add(new Group() { Name = "Lv-324.Web" });
            groups.Add(new Group() { Name = "Lv-325.Web" });
            groups.Add(new Group() { Name = "Lv-326.Java" });
            groups.Add(new Group() { Name = "Lv-327.Web" });
            groups.Add(new Group() { Name = "Lv-328.C++" });
            groups.Add(new Group() { Name = "Lv-329.Python" });
            #endregion

            //Assigning Id's for plans
            for (int i = 0, j = 1; i < groups.Count; i++, j++)
            {
                groups[i].Id = j;
            }

            //Assigning mentors for groups
            foreach (var group in groups)
            {
                group.Mentor_Id = group.Id;
            }

            var netFullStackPlans = context.Plans.Where(p => p.Id >= 1 && p.Id <= 6);

            var students319 = context.Users.Where(user => user.Id >= 12 && user.Id <= 18);
            foreach (var netFullStackPlan in netFullStackPlans)
            {
                groups[0].Plans.Add(netFullStackPlan);
                groups[1].Plans.Add(netFullStackPlan);
                groups[2].Plans.Add(netFullStackPlan);
            }
            foreach (var student319 in students319)
            {
                groups[0].Users.Add(student319);
            }

            var students320 = context.Users.Where(user => user.Id >= 19 && user.Id <= 24);
            foreach (var student320 in students320)
            {
                groups[1].Users.Add(student320);
            }
            var students321 = context.Users.Where(user => user.Id >= 25 && user.Id <= 30);
            foreach (var student321 in students321)
            {
                groups[2].Users.Add(student321);
            }

            context.Groups.AddRange(groups);
            context.SaveChanges();

        }

        private static void InitializeTasks(LearnWithMentor_DBEntities context)
        {
            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('[Tasks]', RESEED, 0)");

            #region List of Tasks
            List<Task> tasks = new List<Task>();
            tasks.Add(new Task() { Name = "C#. Installing Visual Studio.", Description = "Installing IDE Microsoft Visual Studio for further work in plan." });//Section 1 [1]
            tasks.Add(new Task() { Name = "C#. Variables, Loops.", Description = "Declaring variables. Difference between value types and reference types. Looping program flow." });//Section 1 [2]
            tasks.Add(new Task() { Name = "C#. Array.", Description = "Grouping varialbes in arrays" });//Section 1 [3]
            tasks.Add(new Task() { Name = "C#. Class, Method, Exception.", Description = "Declaring custom types, creating methods and catching exceptions during execution." });//Section 1 [4]
            tasks.Add(new Task() { Name = "ASP NET Controllers.", Description = "Explores the topic of ASP.NET MVC controllers, controller actions, and action results. " });//Section 2 [5]
            tasks.Add(new Task() { Name = "ASP.NET Routes.", Description = "The ASP.NET Routing module is responsible for mapping incoming browser requests to particular MVC controller actions." });//Section 2 [6]
            tasks.Add(new Task() { Name = "ASP.NET View.", Description = "Razor-based view templates have a .cshtml file extension, and provide an elegant way to create HTML output using C#." });//Section 3  [7]
            tasks.Add(new Task() { Name = "ASP.NET Making App.", Description = "Making close to real small project." });//Section 3 [8]
            tasks.Add(new Task() { Name = "Angular. Installing Atom.", Description = "Installing code editor for further work in plan" });//Section 4 [9]
            tasks.Add(new Task() { Name = "Angular. App structure.", Description = "Structure of angular appliication." });//Section 4 [10]
            tasks.Add(new Task() { Name = "Angular. Services.", Description = "Services are a great way to share information among classes that don't know each other." });//Section 5 [11]
            tasks.Add(new Task() { Name = "Angular. Observables.", Description = "Observables provide support for passing messages between publishers and subscribers in your application." });//Section 5 [12]
            tasks.Add(new Task() { Name = "Angular Material. Installing Angular material", Description = "Angular material installation ways." });//Section 6 [13]
            tasks.Add(new Task() { Name = "Angular Material. Adding NavBar", Description = "Adding material navigation bar for our application." });//Section 6 [14]
            tasks.Add(new Task() { Name = "Angular Material. Working on main page", Description = "Creating main page using angular material components." });//Section 7 [15]
            tasks.Add(new Task() { Name = "Angular Material. Working with Data ", Description = "Learning how to connect data with angular material components. " });//Section 7 [16]
            tasks.Add(new Task() { Name = "SQL. Installing SQL Server and Management studio", Description = "Installing DBMS Microsoft SQL Management Studio for further work in plan." });//Section 8 [17]
            tasks.Add(new Task() { Name = "SQL. Creating databases and tables.", Description = "Making queries for creation relational databases and tables within them." });//Section 8 [18]
            tasks.Add(new Task() { Name = "SQL. Data insertion and manipulation.", Description = "Inserting raw data in tables and making manipulation with. " });//Section 9 [19]
            tasks.Add(new Task() { Name = "SQL. JOIN", Description = "Learning types of JOIN's" });//Section 9 [20]
            tasks.Add(new Task() { Name = "Full stack WEB App. Modeling app structure", Description = "Getting started with project architectures. " });//Section 10
            tasks.Add(new Task() { Name = "Full stack WEB App. Creating Database", Description = "Approaches to data base creation. Data-first, code-first." });//Section 10
            tasks.Add(new Task() { Name = "Full stack WEB App. Data Access, business logic.", Description = "Using different patterns in DAL BLL" });//Section 10
            tasks.Add(new Task() { Name = "Full stack WEB App. Fron-end part in angular", Description = "Creating user interface using fron-end frameworks. " });//Section 10
            tasks.Add(new Task() { Name = "React. React basics", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" });
            tasks.Add(new Task() { Name = "React. Routes, Lifecycle", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" });
            tasks.Add(new Task() { Name = "React. Transitions & Typechecking", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" });
            tasks.Add(new Task() { Name = "React. Working with forms", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" });
            tasks.Add(new Task() { Name = "Java. Getting Started. ", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" });
            tasks.Add(new Task() { Name = "Java. Working with Variables.", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" });
            tasks.Add(new Task() { Name = "Java. Working with Objects.", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" });
            tasks.Add(new Task() { Name = "Java. Exception Handling and Debugging.", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" });
            tasks.Add(new Task() { Name = "JavaScript. Language Basics", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" });
            tasks.Add(new Task() { Name = "JavaScript. Works Behind the Scenes", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" });
            tasks.Add(new Task() { Name = "JavaScript. DOM Manipulation and Events", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" });
            tasks.Add(new Task() { Name = "JavaScript. Objects and Functions", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" });
            tasks.Add(new Task() { Name = "C++. Basic Syntax. ", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" });
            tasks.Add(new Task() { Name = "C++. Defining Functions", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" });
            tasks.Add(new Task() { Name = "C++. The Preprocessor", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" });
            tasks.Add(new Task() { Name = "C++. Classes and Objects", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" });
            tasks.Add(new Task() { Name = "Python. Programming Basics", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" });
            tasks.Add(new Task() { Name = "Python. Advanced Concepts", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" });
            tasks.Add(new Task() { Name = "Python. RPG Battle Script", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" });
            tasks.Add(new Task() { Name = "Python. Web Scraper", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" });
            #endregion

            //Assigning Id's for tasks
            for (int i = 0, j = 1; i < tasks.Count; i++, j++)
            {
                tasks[i].Id = j;
            }

            //Setting tasks as public 
            foreach (var task in tasks)
            {
                task.Private = false;
                task.Mod_Date = new DateTime(2018, 7, 16, 23, 59, 59);
            }

            //Assigning creators of tasks
            //Each creator created 4 plans
            int count = 1;
            for (int i = 1; i <= 11; i++)
            {
                for (int j = count; j < count + 4; j++)
                {
                    tasks[j - 1].Create_Id = i;
                    tasks[j - 1].Mod_Id = i;
                }
                count = count + 4;
            }

            context.Tasks.AddRange(tasks);
            context.SaveChanges();

        }

        private static void InitializeSections(LearnWithMentor_DBEntities context)
        {
            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('[Sections]', RESEED, 0)");

            #region List of Sections

            List<Section> sections = new List<Section>();
            sections.Add(new Section() { Name = "Ordinary" });
            sections.Add(new Section() { Name = "Base ASP.NET" });
            sections.Add(new Section() { Name = "Advanced ASP.NET" });
            sections.Add(new Section() { Name = "Base Angular" });
            sections.Add(new Section() { Name = "Advanced Angular" });
            sections.Add(new Section() { Name = "Base Angular Material" });
            sections.Add(new Section() { Name = "Advanced Angular Material" });
            sections.Add(new Section() { Name = "Base Angular SQL" });
            sections.Add(new Section() { Name = "Advanced SQL" });
            sections.Add(new Section() { Name = "Full Stack" });
            sections.Add(new Section() { Name = "Base React" });
            sections.Add(new Section() { Name = "Advanced React" });
            sections.Add(new Section() { Name = "Base Java" });
            sections.Add(new Section() { Name = "Advanced Java" });
            sections.Add(new Section() { Name = "Base JavaScript" });
            sections.Add(new Section() { Name = "Advanced JavaScript" });
            sections.Add(new Section() { Name = "Base C++" });
            sections.Add(new Section() { Name = "Advanced C++" });
            sections.Add(new Section() { Name = "Base Python" });
            sections.Add(new Section() { Name = "Advanced Python" });
            #endregion

            //Assigning Id's for sections
            for (int i = 0, j = 1; i < sections.Count; i++, j++)
            {
                sections[i].Id = j;
            }

            context.Sections.AddRange(sections);
            context.SaveChanges();

        }

        private static void InitializePlanTask(LearnWithMentor_DBEntities context)
        {
            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('[Plantasks]', RESEED, 0)");


            List<PlanTask> planTasks = new List<PlanTask>();
            //Assigning tasks for plantasks
            //We Have 11 plans, each will create 4 plantasks
            int count = 1;
            for (int i = 1; i <= 11; i++)
            {
                for (int j = count; j < count + 4; j++)
                {
                    planTasks.Add(new PlanTask() { Id = i, Plan_Id = i, Task_Id = j });
                }
                count = count + 4;
            }

            //Assigning setion_Id for planTasks
            //First section will have first 4 plantasks 
            for (int i = 0; i <= 3; i++)
            {
                planTasks[i].Section_Id = 1;
            }


            //Next 8 sections will have next 2 plantasks each (1 section for 2 planTask)
            count = 4;
            for (int i = 2; i <= 10; i++)
            {
                for (int j = count; j < count + 2; j++)
                {
                    planTasks[j].Section_Id = i;
                }
                count = count + 2;

            }

            //Next 10 section will have next 4 plantasks
            for (int j = 20; j <= 23; j++)
            {
                planTasks[j].Section_Id = 10;
            }
            //Last 10 sections will have 16 planTasks (1 section for 2 planTask)
            count = 24;
            for (int i = 11; i <= 20; i++)
            {
                for (int j = count; j < count + 2; j++)
                {
                    planTasks[j].Section_Id = i;
                }
                count = count + 2;

            }

            //Assigning priorities for plantasks
            foreach (var planTask in planTasks)
            {
                if (planTask.Section_Id == 1 && planTask.Section_Id == 10)
                {
                    planTask.Priority = 1;
                }
                else
                {
                    planTask.Priority = 2;
                }
            }

            //Assigning Id's for planTasks
            for (int i = 0, j = 1; i < planTasks.Count; i++, j++)
            {
                planTasks[i].Id = j;
            }

            context.PlanTasks.AddRange(planTasks);
            context.SaveChanges();

        }

        private static void InitializeUserTasks(LearnWithMentor_DBEntities context)
        {
            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('[UserTasks]', RESEED, 0)");

            //Assigning tasks for usertasks
            //Task will have only first seven students
            List<UserTask> userTasks = new List<UserTask>();


            //Assigning userTasks for students
            //12...18 Students Id's
            //1..25 planTasks Id's
            for (int i = 12; i <= 18; i++)
            {
                for (int j = 1; j <= 24; j++)
                {
                    userTasks.Add(new UserTask()
                    {
                        User_Id = i,
                        PlanTask_Id = j,
                        Mentor_Id = 1,
                        Result = "Ref to GitHub"
                    });
                }
            }

            //Assigning Id's for userTasks
            for (int i = 0, j = 1; i < userTasks.Count; i++, j++)
            {
                userTasks[i].Id = j;
            }

            foreach (var userTask in userTasks)
            {
                switch (userTask.User_Id)
                {
                    case 12:
                    case 13:
                        userTask.State = "D";
                        break;
                    case 14:
                    case 15:
                        userTask.State = "P";
                        break;
                    case 16:
                    case 17:
                        userTask.State = "A";
                        break;
                    case 18:
                        userTask.State = "R";
                        break;
                }
            }

            foreach (var userTask in userTasks)
            {
                userTask.Propose_End_Date = new DateTime(2018, 9, 1, 23, 59, 59);
                userTask.Propose_End_Date = new DateTime(2018, 9, 12, 23, 59, 59);
            }

            context.UserTasks.AddRange(userTasks);
            context.SaveChanges();

        }

        private static void InitializeMessages(LearnWithMentor_DBEntities context)
        {
            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('[Messages]', RESEED, 0)");

            string[] messagesTemplates = new[] { "Sorry, i've got cold.", "Hello. I've done this task. Can you review?", "I have problem with this task. Can you help me?", "There some bugs in my code." };
            Random rnd = new Random();
            //Creating messages
            List<Message> messages = new List<Message>();

            int count = 1;
            for (int i = 12; i < 18; i++)
            {
                for (int j = count; j < count + 24; j++)
                {
                    for (int k = 1; k <= 2; k++)
                    {
                        messages.Add(new Message() { User_Id = i, UserTask_Id = j, Text = messagesTemplates[rnd.Next(messagesTemplates.Length - 1)] });
                    }
                }
                count = count + 24;
            }

            //Assigning Id's for messages
            for (int i = 0, j = 1; i < messages.Count; i++, j++)
            {
                messages[i].Id = j;
            }

            context.Messages.AddRange(messages);
            context.SaveChanges();

        }

        private static void InitializeComments(LearnWithMentor_DBEntities context)
        {
            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('[Comments]', RESEED, 1)");

            //Creating comments
            Random rnd = new Random();
            List<Comment> comments = new List<Comment>();
            string[] commentTemplates = new[] { "Nice task", "Easy task", "Hard task", "Interesting task", "I was needed help for this task" };

            for (int i = 1; i < 24; i++)
            {
                comments.Add(new Comment() { PlanTask_Id = i, Create_Id = rnd.Next(12, 18), Text = commentTemplates[rnd.Next(commentTemplates.Length - 1)] });
                comments.Add(new Comment() { PlanTask_Id = i, Create_Id = rnd.Next(12, 18), Text = commentTemplates[rnd.Next(commentTemplates.Length - 1)] });
                comments.Add(new Comment() { PlanTask_Id = i, Create_Id = rnd.Next(12, 18), Text = commentTemplates[rnd.Next(commentTemplates.Length - 1)] });
            }

            //Assigning Id's for comments
            for (int i = 0, j = 1; i < comments.Count; i++, j++)
            {
                comments[i].Id = j;
            }

            context.Comments.AddRange(comments);
            context.SaveChanges();

        }

        private static void InitializePlanSugestion(LearnWithMentor_DBEntities context)
        {

            //Creating PlanSuggestion
            List<PlanSuggestion> planSuggestions = new List<PlanSuggestion>();

            for (int j = 12; j < 30; j = j + 3)
            {
                for (int k = 1; k < 6; k++)
                {
                    planSuggestions.Add(new PlanSuggestion() { Mentor_Id = 1, Plan_Id = k, User_Id = j, Text = "Suggest you to increase number of tasks." });
                }
            }
            for (int j = 13; j < 30; j = j + 3)
            {
                for (int k = 1; k < 6; k++)
                {
                    planSuggestions.Add(new PlanSuggestion() { Mentor_Id = 2, Plan_Id = k, User_Id = j, Text = "Sugest you to set more time for task completion." });
                }
            }
            for (int j = 14; j < 30; j = j + 3)
            {
                for (int k = 1; k < 6; k++)
                {
                    planSuggestions.Add(new PlanSuggestion() { Mentor_Id = 3, Plan_Id = k, User_Id = j, Text = "Suggest you to make task more interesting" });
                }
            }

            context.PlanSuggestion.AddRange(planSuggestions);
            context.SaveChanges();
        }

    }
}
