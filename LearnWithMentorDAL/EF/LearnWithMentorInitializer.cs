using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LearnWithMentorDAL.Entities;


namespace LearnWithMentorDAL.EF
{
    public class LearnWithMentorInitializer
    {
        static LearnWithMentorInitializer()
        {
            Initialize();
        }
        public static void Initialize()
        {
            using (var context = new LearnWithMentor_DBEntities())
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
        private static void InitializeRoles(LearnWithMentor_DBEntities context)
        {
            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('Roles', RESEED, 0)");
            var roles = new List<Role>
            {
                new Role() {Id = 1, Name = "Mentor"},
                new Role() {Id = 2, Name = "Student"},
                new Role() {Id = 3, Name = "Admin"}
            };
            context.Roles.AddRange(roles);
            context.SaveChanges();
        }
        private static void InitializeUsers(LearnWithMentor_DBEntities context)
        {
            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('Users', RESEED, 0)");
            var users = new List<User>
            {
                //mentors
                new User() {FirstName = "Vyacheslav", LastName = "Koldovsky", Email = "koldovsky@gmail.com"},
                new User() {FirstName = "Khrystyna ", LastName = "Romaniv", Email = "romaniv@gmail.com"},
                new User() {FirstName = "Orysia", LastName = "Khoroshchak", Email = "khoroshchak@gmail.com"},
                new User() {FirstName = "Lesya", LastName = "Klakovych", Email = "klakovych@gmail.com"},
                new User() {FirstName = "Viktoria", LastName = "Ryazhska", Email = "ryazhska@gmail.com"},
                new User() {FirstName = "Liubomyr", LastName = "Halamaha", Email = "halamaha@gmail.com"},
                new User() {FirstName = "Igor", LastName = "Kohut", Email = "kohut@gmail.com"},
                new User() {FirstName = "Andriy", LastName = "Korkuna", Email = "korkuna@gmail.com"},
                new User() {FirstName = "Yaroslav", LastName = "Harasym", Email = "harasym@gmail.com"},
                new User() {FirstName = "Mykhaylo", LastName = "Plesna", Email = "plesna@gmail.com"},
                new User() {FirstName = "Maryana", LastName = "Lopatynska", Email = "lopatynska@gmail.com"},
                //students of Lv-343.Net
                new User() {FirstName = "Bohdan", LastName = "Bondarets", Email = "bondarets.bogdan@gmail.com"},
                new User() {FirstName = "Yura", LastName = "Vasko", Email = "zhydetskyi@gmail.com"},
                new User() {FirstName = "Yura", LastName = "Kozlovsky", Email = "yurikozlovskiJ@gmail.com"},
                new User() {FirstName = "Nazar", LastName = "Polevyy", Email = "nazarp06@gmail.com"},
                new User() {FirstName = "Valentyn", LastName = "Kravchenko", Email = "kravchenkov.me@gmail.com"},
                new User() {FirstName = "Yura", LastName = "Stashko", Email = "yura.stashko98@gmail.com"},
                new User() {FirstName = "Solomia", LastName = "Yusko", Email = "solayusko@gmail.com"},
                new User() {FirstName = "Sofia", LastName = "Flys", Email = "flyssoffia@gmail.com"},
                //Next users are administators
                new User() {FirstName = "Pedro", LastName = "Alvares", Email = "alvares@gmail.com"},
                new User() {FirstName = "Dmytro", LastName = "Chalyi", Email = "chalyi@gmail.com"},
                new User() {FirstName = "Adriana", LastName = "Prudyvus", Email = "prudyvus@gmail.com"},
                new User() {FirstName = "Yaromyr", LastName = "Oryshchyn", Email = "oryshchyn@gmail.com"},
                new User() {FirstName = "Andrii", LastName = "Danyliuk", Email = "danyliuk@gmail.com"},
                new User() {FirstName = "Maksym", LastName = "Prytyka", Email = "prytyka@gmail.com"},
                new User() {FirstName = "Mykhailo", LastName = "Kyzyma", Email = "kyzyma@gmail.com"},
                new User() {FirstName = "Dmytro", LastName = "Khomyk", Email = "khomyk@gmail.com"},
                new User() {FirstName = "Pavlo", LastName = "Kruk", Email = "kruk@gmail.com"},
                new User() {FirstName = "Kateryna", LastName = "Obrizan", Email = "obrizan@gmail.com"},
                new User() {FirstName = "Viktor", LastName = "Levak", Email = "levak@gmail.com"},
                new User() {FirstName = "Oleksandr", LastName = "Mykhalchuk", Email = "mykhalchuk@gmail.com"},
                new User() {FirstName = "El", LastName = "Admino", Email = "admino@gmail.com"},
                new User() {FirstName = "La", LastName = "Admina", Email = "admina@gmail.com"}
            };
            foreach (var user in users)
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword("123");
                user.Blocked = false;
                user.Email_Confirmed = true;
            }

            var pathToImagesFolder = Path.Combine((AppDomain.CurrentDomain.BaseDirectory).
                Replace("LearnWithMentor", string.Empty),
                @"LearnWithMentorDAL\EF\images\");

            const int numOfMentors = 11;
            const int numOfStudents = 20;

            for (var i = 0; i < numOfMentors; i++)
            {
                users[i].Role_Id = 1;
                users[i].Image_Name = "mentorImage";
                users[i].Image = Convert.ToBase64String(File.ReadAllBytes(Path.Combine(pathToImagesFolder, @"mentor.jpg")));
            }
            for (var i = numOfMentors; i < numOfMentors + numOfStudents; i++)
            {
                users[i].Role_Id = 2;
                users[i].Image_Name = "studentImage";
                users[i].Image = Convert.ToBase64String(File.ReadAllBytes(Path.Combine(pathToImagesFolder, @"student.png")));
            }
            for (var i = numOfMentors + numOfStudents - 1; i < users.Count; i++)
            {
                users[i].Role_Id = 3;
                users[i].Image_Name = "adminImage";
                users[i].Image = Convert.ToBase64String(File.ReadAllBytes(Path.Combine(pathToImagesFolder, @"admin.png")));
            }
            for (var i = 12; i < 19; i++)
            {
                users[i - 1].Image = Convert.ToBase64String(File.ReadAllBytes(Path.Combine(pathToImagesFolder, @i + ".jpg")));
            }
            context.Users.AddRange(users);
            context.SaveChanges();
        }
        private static void InitializePlans(LearnWithMentor_DBEntities context)
        {
            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('[Plans]', RESEED, 0)");

            var pathToImagesFolder = Path.Combine((AppDomain.CurrentDomain.BaseDirectory).
                Replace("LearnWithMentor", string.Empty),
                @"LearnWithMentorDAL\EF\images\");

            var plans = new List<Plan>
            {
                new Plan()
                {
                    Name = "C# Essential Training",
                    Description =
                    "Takes you through C#'s history, it's core syntax, and the fundamentals of writing strong C# code.",
                    Image_Name = "CSharp",
                    Image = Convert.ToBase64String(File.ReadAllBytes(Path.Combine(pathToImagesFolder, @"сsharp.png")))
                },
                new Plan()
                {
                    Name = "ASP.NET",
                    Description =
                    "In this practical course, you will learn how to build a line-of-business, enterprise application with ASP.NET Core MVC, including topics such as security, logging, testing, validation, and much more.",
                    Image_Name = "AspNet",
                    Image = Convert.ToBase64String(File.ReadAllBytes(Path.Combine(pathToImagesFolder, @"asp_net.png")))
                },
                new Plan()
                {
                    Name = "Angular Guide",
                    Description =
                    "Use their gained, deep understanding of the Angular 6 fundamentals to quickly establish themselves as frontend developers",
                    Image_Name = "Angular",
                    Image = Convert.ToBase64String(File.ReadAllBytes(Path.Combine(pathToImagesFolder, @"angular.png")))

                },
                new Plan()
                {
                    Name = "Angular Material Guide",
                    Description =
                    "We'll build an entire, realistic app which looks absolutely beautiful, uses Google's Material Design and is extremely fast! Thanks to Firebase and Angularfire, we'll add real-time database functionalities and see our updates almost before we make them!",
                    Image_Name = "AngularMaterial",
                    Image = Convert.ToBase64String(File.ReadAllBytes(Path.Combine(pathToImagesFolder, @"angularMaterial.png")))
                },
                new Plan()
                {
                    Name = "SQL & Database Design: Learn MS SQL Server",
                    Description = "In this course you will learn how to create queries in a MS SQL Management Studio",
                    Image_Name = "Sql",
                    Image = Convert.ToBase64String(File.ReadAllBytes(Path.Combine(pathToImagesFolder, @"sql.png")))
                },
                new Plan()
                {
                    Name = "Building Full Stack Applications",
                    Description =
                    "Creating a complete full-stack application requires integrating multiple components.  This plan teaches integration through the perspective of a quiz project, using Angular, ASP.NET Core, and Entity Framework Core to develop a full-stack application.",
                    Image_Name = "FullStack",
                    Image = Convert.ToBase64String(File.ReadAllBytes(Path.Combine(pathToImagesFolder, @"fullStack.png")))
                },
                new Plan()
                {
                    Name = "The complete React course",
                    Description = "You will learn the whole React WebApp building process, from your pc to the server.",
                    Image_Name = "React",
                    Image = Convert.ToBase64String(File.ReadAllBytes(Path.Combine(pathToImagesFolder, @"react.png")))
                },
                new Plan()
                {
                    Name = "Java Essential Training",
                    Description =
                    " This course provides the foundation for learning Java SE (Standard Edition), so you can build your first apps or start exploring the language on your own.",
                    Image_Name = "Java",
                    Image = Convert.ToBase64String(File.ReadAllBytes(Path.Combine(pathToImagesFolder, @"java.png")))
                },
                new Plan()
                {
                    Name = "The Complete JavaScript Course",
                    Description =
                    "JavaScript and programming fundamentals: variables, boolean logic, if/else, loops, functions, arrays, etc. A true understanding of how JavaScript works behind the scenes.",
                    Image_Name = "JavaScript",
                    Image = Convert.ToBase64String(File.ReadAllBytes(Path.Combine(pathToImagesFolder, @"javascript.png")))
                },
                new Plan()
                {
                    Name = "C++ Essential Training",
                    Description =
                    "Widely used for both systems and applications development, the C and C++ programming languages are available for virtually every operating system and are often the best choice for performance-critical applications. In this course, Bill Weinman dissects the anatomy of C and C++, from variables to functions and loops, and explores both the C Standard Library and the C++ Standard Template Library.",
                    Image_Name = "C++",
                    Image = Convert.ToBase64String(File.ReadAllBytes(Path.Combine(pathToImagesFolder, @"c++.png")))
                },
                new Plan()
                {
                    Name = "Python Course: Beginner to Advanced",
                    Description =
                    "This course is designed to fully immerse you in the Python language, so it is great for both beginners and veteran programmers! Learn Python through the basics of programming, advanced Python concepts, coding a calculator, essential modules, web scraping, PyMongo, WebPy development, Django web framework, GUI programming, data visualization, machine learning.",
                    Image_Name = "Python",
                    Image = Convert.ToBase64String(File.ReadAllBytes(Path.Combine(pathToImagesFolder, @"python.png")))
                }
            };
            for (int i = 0, j = 1; i < plans.Count; i++, j++)
            {
                plans[i].Id = j;
            }
            foreach (var plan in plans)
            {
                plan.Create_Date = new DateTime(2018, 7, 3, 23, 59, 59);
                plan.Mod_Date = new DateTime(2018, 7, 16, 23, 59, 59);
                plan.Create_Id = plan.Id;
                plan.Mod_Id = plan.Id;
                plan.Published = true;
            }
            context.Plans.AddRange(plans);
            context.SaveChanges();
        }
        private static void InitializeGroups(LearnWithMentor_DBEntities context)
        {
            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('[Groups]', RESEED, 0)");
            #region List of groups
            var groups = new List<Group>
            {
                new Group() { Name = "Lv-343.Net" },
                new Group() { Name = "Lv-320.Net" },
                new Group() { Name = "Lv-321.Net" },
                new Group() { Name = "Lv-322.Web" },
                new Group() { Name = "Lv-323.SQL" },
                new Group() { Name = "Lv-324.Web" },
                new Group() { Name = "Lv-325.Web" },
                new Group() { Name = "Lv-326.Java" },
                new Group() { Name = "Lv-327.Web" },
                new Group() { Name = "Lv-328.C++" },
                new Group() { Name = "Lv-329.Python" }
            };
            #endregion
            for (int i = 0, j = 1; i < groups.Count; i++, j++)
            {
                groups[i].Id = j;
            }
            foreach (var group in groups)
            {
                group.Mentor_Id = group.Id;
            }
            var netFullStackPlans = context.Plans.Where(p => p.Id >= 1 && p.Id <= 6);
            var students343 = context.Users.Where(user => user.Id >= 12 && user.Id <= 18);

            foreach (var netFullStackPlan in netFullStackPlans)
            {
                groups[0].Plans.Add(netFullStackPlan);
                groups[1].Plans.Add(netFullStackPlan);
                groups[2].Plans.Add(netFullStackPlan);
            }
            foreach (var student343 in students343)
            {
                groups[0].Users.Add(student343);
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
            var tasks = new List<Task>
            {
                new Task() { Name = "C#. Installing Visual Studio.", Description = "Installing IDE Microsoft Visual Studio for further work in plan." },
                new Task() { Name = "C#. Variables, Loops.", Description = "Declaring variables. Difference between value types and reference types. Looping program flow." },
                new Task() { Name = "C#. Array.", Description = "Grouping varialbes in arrays" },
                new Task() { Name = "C#. Class, Method, Exception.", Description = "Declaring custom types, creating methods and catching exceptions during execution." },
                new Task() { Name = "ASP NET Controllers.", Description = "Explores the topic of ASP.NET MVC controllers, controller actions, and action results. " },
                new Task() { Name = "ASP.NET Routes.", Description = "The ASP.NET Routing module is responsible for mapping incoming browser requests to particular MVC controller actions." },
                new Task() { Name = "ASP.NET View.", Description = "Razor-based view templates have a .cshtml file extension, and provide an elegant way to create HTML output using C#." },
                new Task() { Name = "ASP.NET Making App.", Description = "Making close to real small project." },
                new Task() { Name = "Angular. Installing Atom.", Description = "Installing code editor for further work in plan" },
                new Task() { Name = "Angular. App structure.", Description = "Structure of angular appliication." },
                new Task() { Name = "Angular. Services.", Description = "Services are a great way to share information among classes that don't know each other." },
                new Task() { Name = "Angular. Observables.", Description = "Observables provide support for passing messages between publishers and subscribers in your application." },
                new Task() { Name = "Angular Material. Installing Angular material", Description = "Angular material installation ways." },
                new Task() { Name = "Angular Material. Adding NavBar", Description = "Adding material navigation bar for our application." },
                new Task() { Name = "Angular Material. Working on main page", Description = "Creating main page using angular material components." },
                new Task() { Name = "Angular Material. Working with Data ", Description = "Learning how to connect data with angular material components. " },
                new Task() { Name = "SQL. Installing SQL Server and Management studio", Description = "Installing DBMS Microsoft SQL Management Studio for further work in plan." },
                new Task() { Name = "SQL. Creating databases and tables.", Description = "Making queries for creation relational databases and tables within them." },
                new Task() { Name = "SQL. Data insertion and manipulation.", Description = "Inserting raw data in tables and making manipulation with. " },
                new Task() { Name = "SQL. JOIN", Description = "Learning types of JOIN's" },
                new Task() { Name = "Full stack WEB App. Modeling app structure", Description = "Getting started with project architectures. " },
                new Task() { Name = "Full stack WEB App. Creating Database", Description = "Approaches to data base creation. Data-first, code-first." },
                new Task() { Name = "Full stack WEB App. Data Access, business logic.", Description = "Using different patterns in DAL BLL" },
                new Task() { Name = "Full stack WEB App. Fron-end part in angular", Description = "Creating user interface using fron-end frameworks. " },
                new Task() { Name = "React. React basics", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" },
                new Task() { Name = "React. Routes, Lifecycle", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" },
                new Task() { Name = "React. Transitions & Typechecking", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" },
                new Task() { Name = "React. Working with forms", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" },
                new Task() { Name = "Java. Getting Started. ", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" },
                new Task() { Name = "Java. Working with Variables.", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" },
                new Task() { Name = "Java. Working with Objects.", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" },
                new Task() { Name = "Java. Exception Handling and Debugging.", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" },
                new Task() { Name = "JavaScript. Language Basics", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" },
                new Task() { Name = "JavaScript. Works Behind the Scenes", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" },
                new Task() { Name = "JavaScript. DOM Manipulation and Events", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" },
                new Task() { Name = "JavaScript. Objects and Functions", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" },
                new Task() { Name = "C++. Basic Syntax. ", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" },
                new Task() { Name = "C++. Defining Functions", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" },
                new Task() { Name = "C++. The Preprocessor", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" },
                new Task() { Name = "C++. Classes and Objects", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" },
                new Task() { Name = "Python. Programming Basics", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" },
                new Task() { Name = "Python. Advanced Concepts", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" },
                new Task() { Name = "Python. RPG Battle Script", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" },
                new Task() { Name = "Python. Web Scraper", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" }
            };
            for (int i = 0, j = 1; i < tasks.Count; i++, j++)
            {
                tasks[i].Id = j;
            }
            foreach (var task in tasks)
            {
                task.Private = false;
                task.Create_Date = new DateTime(2018, 7, 14, 23, 59, 59);
                task.Mod_Date = new DateTime(2018, 7, 16, 23, 59, 59);
            }
            var count = 1;
            var numOfMentors = context.Users.Count(user => user.Role_Id == 1);
            var tasksPerMentor = 4;

            for (var i = 1; i <= numOfMentors; i++)
            {
                for (var j = count; j < count + tasksPerMentor; j++)
                {
                    tasks[j - 1].Create_Id = i;
                    tasks[j - 1].Mod_Id = i;
                }
                count = count + tasksPerMentor;
            }
            context.Tasks.AddRange(tasks);
            context.SaveChanges();
        }
        private static void InitializeSections(LearnWithMentor_DBEntities context)
        {
            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('[Sections]', RESEED, 0)");
            #region List of Sections
            var sections = new List<Section>
            {
                new Section() { Name = "Ordinary" },
                new Section() { Name = "Base ASP.NET" },
                new Section() { Name = "Advanced ASP.NET" },
                new Section() { Name = "Base Angular" },
                new Section() { Name = "Advanced Angular" },
                new Section() { Name = "Base Angular Material" },
                new Section() { Name = "Advanced Angular Material" },
                new Section() { Name = "Base Angular SQL" },
                new Section() { Name = "Advanced SQL" },
                new Section() { Name = "Full Stack" },
                new Section() { Name = "Base React" },
                new Section() { Name = "Advanced React" },
                new Section() { Name = "Base Java" },
                new Section() { Name = "Advanced Java" },
                new Section() { Name = "Base JavaScript" },
                new Section() { Name = "Advanced JavaScript" },
                new Section() { Name = "Base C++" },
                new Section() { Name = "Advanced C++" },
                new Section() { Name = "Base Python" },
                new Section() { Name = "Advanced Python" }
            };
            #endregion
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
            var planTasks = new List<PlanTask>();
            var count = 1;

            for (var i = 1; i < context.Plans.Count(); i++)
            {
                for (var j = count; j < count + 4; j++)
                {
                    planTasks.Add(new PlanTask() { Plan_Id = i, Task_Id = j });
                }
                count = count + 4;
            }

            var numOfPlanTasksInSmallSection = 2;
            var numOfPlanTasksInBigSection = 4;

            for (int i = 0; i <= numOfPlanTasksInBigSection - 1; i++)
            {
                planTasks[i].Section_Id = 1;
            }

            count = numOfPlanTasksInBigSection;
            var sectionsBetweenEssentAndFullStack = context.Sections.Where(section => section.Id > 1 && section.Id < 10);
            foreach (var section in sectionsBetweenEssentAndFullStack)
            {
                for (int i = count; i < count + 2; i++)
                {
                    planTasks[i].Section_Id = section.Id;
                }
                count = count + numOfPlanTasksInSmallSection;
            }

            var fullStackSection = context.Sections.Where(section => section.Id == 10);
            foreach (var section in fullStackSection)
            {
                for (int i = count; i < count + numOfPlanTasksInSmallSection; i++)
                {
                    planTasks[i].Section_Id = section.Id;
                }
            }

            var sectionsAfterFullStack = context.Sections.Where(section => section.Id > 10);
            foreach (var section in sectionsAfterFullStack)
            {
                for (int i = count; i < count + 2; i++)
                {
                    planTasks[i].Section_Id = section.Id;
                }
            }


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
            var userTasks = new List<UserTask>();
            for (var i = 12; i <= 18; i++)
            {
                for (var j = 1; j <= 24; j++)
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
                userTask.End_Date = new DateTime(2018, 9, 1, 23, 59, 59);
                userTask.Propose_End_Date = new DateTime(2018, 9, 12, 23, 59, 59);
            }
            context.UserTasks.AddRange(userTasks);
            context.SaveChanges();
        }
        private static void InitializeMessages(LearnWithMentor_DBEntities context)
        {
            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('[Messages]', RESEED, 0)");
            var messagesTemplates = new[] { "Sorry, i've got cold.", "Hello. I've done this task. Can you review?", "I have problem with this task. Can you help me?", "There some bugs in my code." };
            var rnd = new Random();
            var messages = new List<Message>();
            var count = 1;
            for (var i = 12; i < 18; i++)
            {
                for (var j = count; j < count + 24; j++)
                {
                    for (var k = 1; k <= 2; k++)
                    {
                        messages.Add(new Message() { User_Id = i, UserTask_Id = j, Text = messagesTemplates[rnd.Next(messagesTemplates.Length - 1)] });
                    }
                }
                count = count + 24;
            }
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
            var rnd = new Random();
            var comments = new List<Comment>();
            var commentTemplates = new[] { "Nice task", "Easy task", "Hard task", "Interesting task", "I was needed help for this task" };
            for (var i = 1; i < 24; i++)
            {
                comments.Add(new Comment() { PlanTask_Id = i, Create_Id = rnd.Next(12, 18), Text = commentTemplates[rnd.Next(commentTemplates.Length - 1)] });
                comments.Add(new Comment() { PlanTask_Id = i, Create_Id = rnd.Next(12, 18), Text = commentTemplates[rnd.Next(commentTemplates.Length - 1)] });
                comments.Add(new Comment() { PlanTask_Id = i, Create_Id = rnd.Next(12, 18), Text = commentTemplates[rnd.Next(commentTemplates.Length - 1)] });
            }
            for (int i = 0; i < comments.Count; i++)
            {
                comments[i].Id = i + 1;
            }
            context.Comments.AddRange(comments);
            context.SaveChanges();
        }

        private static void InitializePlanSugestion(LearnWithMentor_DBEntities context)
        {
            var planSuggestions = new List<PlanSuggestion>();

            for (var j = 12; j < 30; j = j + 3)
            {
                for (var k = 1; k < 6; k++)
                {
                    planSuggestions.Add(new PlanSuggestion() { Mentor_Id = 1, Plan_Id = k, User_Id = j, Text = "Suggest you to increase number of tasks." });
                }
            }
            for (var j = 13; j < 30; j = j + 3)
            {
                for (var k = 1; k < 6; k++)
                {
                    planSuggestions.Add(new PlanSuggestion() { Mentor_Id = 2, Plan_Id = k, User_Id = j, Text = "Sugest you to set more time for task completion." });
                }
            }
            for (var j = 14; j < 30; j = j + 3)
            {
                for (var k = 1; k < 6; k++)
                {
                    planSuggestions.Add(new PlanSuggestion() { Mentor_Id = 3, Plan_Id = k, User_Id = j, Text = "Suggest you to make task more interesting" });
                }
            }
            context.PlanSuggestion.AddRange(planSuggestions);
            context.SaveChanges();
        }
    }
}
