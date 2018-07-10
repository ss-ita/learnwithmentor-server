using System;
using System.Collections.Generic;
using System.Linq;
using LearnWithMentorDAL.Entities;


namespace LearnWithMentorDAL.EF
{
    //TO DO
    //check if base is already created
    //launch before first request
    //Кожна сутність - інший метод - in progress
    //перевірка на ініціалізацію бази
    //перевірка на наявних користувачів
    //обгорнути try catch
    //скрипт after connect??
    //Графічна ініціалізація

    public class LearnWithMentorInitializer
    {
        public static void Initialize()
        {
            using (LearnWithMentor_DBEntities context = new LearnWithMentor_DBEntities())
            {
                InitializeUsers(context);
                InitializeRoles(context);
            }

        }
        public static void InitializeRoles(LearnWithMentor_DBEntities context)
        {
            if (!context.Roles.Any())
                {
                    List<Role> roles = new List<Role>();
                    roles.Add(new Role() { Id = 1, Name = "Mentor" });
                    roles.Add(new Role() { Id = 2, Name = "Student" });
                    roles.Add(new Role() { Id = 3, Name = "Admin" });
                    context.Roles.AddRange(roles);
                    context.SaveChanges();
                }

        }

        public static void InitializeUsers(LearnWithMentor_DBEntities context)
        {
            if (!context.Users.Any())
            {
                List<User> users = new List<User>();

                #region List of Mentors
                users.Add(new User() { FirstName = "Vyacheslav", LastName = "Koldovsky", Email = "koldovsky@gmail.com" });
                users.Add(new User() { FirstName = "Khrystyna ", LastName = "Romaniv", Email = "romaniv@gmail.com" });
                users.Add(new User() { FirstName = "Orysia", LastName = "Khoroshchak", Email = "khoroshchak@gmail.com" });
                users.Add(new User() { FirstName = "Lesya", LastName = "Klakovych", Email = "klakovych@gmail.com" });
                users.Add(new User() { FirstName = "Viktoria", LastName = "Ryazhska", Email = "ryazhska@gmail.com" });
                users.Add(new User() { FirstName = "Liubomyr", LastName = "Halamaha", Email = "halamaha@gmail.com" });
                users.Add(new User() { FirstName = "Igor", LastName = "Kohut", Email = "kohut@gmail.com" });
                users.Add(new User() { FirstName = "Andriy", LastName = "Korkuna", Email = "korkuna@gmail.com" });
                users.Add(new User() { FirstName = "Yaroslav", LastName = "Harasym", Email = "harasym@gmail.com" });
                users.Add(new User() { FirstName = "Mykhaylo", LastName = "Plesna", Email = "plesna@gmail.com" });
                users.Add(new User() { FirstName = "Maryana", LastName = "Lopatynska", Email = "lopatynska@gmail.com" });
                #endregion

                #region List of Students
                users.Add(new User() { FirstName = "Roman", LastName = "Maksymyshyn", Email = "maksymyshyn@gmail.com" });
                users.Add(new User() { FirstName = "Yurii-Stefan", LastName = "Zhydetskyi", Email = "zhydetskyi@gmail.com" });
                users.Add(new User() { FirstName = "Oleksandr", LastName = "Isakov", Email = "isakov@gmail.com" });
                users.Add(new User() { FirstName = "Roman", LastName = "Parobii", Email = "parobii@gmail.com" });
                users.Add(new User() { FirstName = "Andrii", LastName = "Lysyi", Email = "lysyi@gmail.com" });
                users.Add(new User() { FirstName = "Andrii", LastName = "Panchyshyn", Email = "panchyshyn@gmail.com" });
                users.Add(new User() { FirstName = "Yulia", LastName = "Pavlyk", Email = "pavlyk@gmail.com" });
                users.Add(new User() { FirstName = "Karim", LastName = "Benkhenni", Email = "benkhenni@gmail.com" });
                users.Add(new User() { FirstName = "Pedro", LastName = "Alvares", Email = "alvares@gmail.com" });
                users.Add(new User() { FirstName = "Dmytro", LastName = "Chalyi", Email = "chalyi@gmail.com" });
                users.Add(new User() { FirstName = "Adriana", LastName = "Prudyvus", Email = "prudyvus@gmail.com" });
                users.Add(new User() { FirstName = "Yaromyr", LastName = "Oryshchyn", Email = "oryshchyn@gmail.com" });
                users.Add(new User() { FirstName = "Andrii", LastName = "Danyliuk", Email = "danyliuk@gmail.com" });
                users.Add(new User() { FirstName = "Maksym", LastName = "Prytyka", Email = "prytyka@gmail.com" });
                users.Add(new User() { FirstName = "Mykhailo", LastName = "Kyzyma", Email = "kyzyma@gmail.com" });
                users.Add(new User() { FirstName = "Dmytro", LastName = "Khomyk", Email = "khomyk@gmail.com" });
                users.Add(new User() { FirstName = "Pavlo", LastName = "Kruk", Email = "kruk@gmail.com" });
                users.Add(new User() { FirstName = "Kateryna", LastName = "Obrizan", Email = "obrizan@gmail.com" });
                users.Add(new User() { FirstName = "Viktor", LastName = "Levak", Email = "levak@gmail.com" });
                users.Add(new User() { FirstName = "Oleksandr", LastName = "Mykhalchuk", Email = "mykhalchuk@gmail.com" });
                #endregion

                //Assigning Id's for users
                for (int i = 0, j = 1; i < users.Count; i++, j++)
                {
                    users[i].Id = j;
                }

                //Assigning custom passwords and  blocked status "false" for users
                foreach (var user in users)
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword("123");
                    user.Blocked = false;
                }

                //Assigning mentor roles for first 10 users
                for (int i = 0; i <= 10; i++)
                {
                    users[i].Role_Id = 1;
                }

                //Assigning student roles for rest of the users
                for (int i = 11; i <= users.Count; i++)
                {
                    users[i].Role_Id = 2;
                }

                context.Users.AddRange(users);
                context.SaveChanges();

            }
        }

        public static void InitializePlans(LearnWithMentor_DBEntities context)
        {
            if (!context.Plans.Any())
            {
                List<Plan> plans = new List<Plan>();
                plans.Add(new Plan()
                {
                    Name = "C# Essential Training",
                    Description =
                        "Takes you through C#'s history, it's core syntax, and the fundamentals of writing strong C# code."
                });
                plans.Add(new Plan()
                {
                    Name = "ASP.NET",
                    Description =
                        "In this practical course, you will learn how to build a line-of-business, enterprise application with ASP.NET Core MVC, including topics such as security, logging, testing, validation, and much more."
                });
                plans.Add(new Plan()
                {
                    Name = "Angular Guide",
                    Description =
                        "Use their gained, deep understanding of the Angular 6 fundamentals to quickly establish themselves as frontend developers"
                });
                plans.Add(new Plan()
                {
                    Name = "Angular Material Guide",
                    Description =
                        "We'll build an entire, realistic app which looks absolutely beautiful, uses Google's Material Design and is extremely fast! Thanks to Firebase and Angularfire, we'll add real-time database functionalities and see our updates almost before we make them!"
                });
                plans.Add(new Plan()
                {
                    Name = "SQL & Database Design: Learn MS SQL Server",
                    Description = "In this course you will learn how to create queries in a MS SQL Management Studio"
                });
                plans.Add(new Plan()
                {
                    Name = "Building Full Stack Applications",
                    Description =
                        "Creating a complete full-stack application requires integrating multiple components.  This plan teaches integration through the perspective of a quiz project, using Angular, ASP.NET Core, and Entity Framework Core to develop a full-stack application."
                });
                plans.Add(new Plan()
                {
                    Name = "The complete React Fullstack course",
                    Description = "You will learn the whole React WebApp building process, from your pc to the server."
                });
                plans.Add(new Plan()
                {
                    Name = "Java Essential Training",
                    Description =
                        " This course provides the foundation for learning Java SE (Standard Edition), so you can build your first apps or start exploring the language on your own."
                });
                plans.Add(new Plan()
                {
                    Name = "The Complete JavaScript Course",
                    Description =
                        "JavaScript and programming fundamentals: variables, boolean logic, if/else, loops, functions, arrays, etc. A true understanding of how JavaScript works behind the scenes."
                });
                plans.Add(new Plan()
                {
                    Name = "C++ Essential Training",
                    Description =
                        "Widely used for both systems and applications development, the C and C++ programming languages are available for virtually every operating system and are often the best choice for performance-critical applications. In this course, Bill Weinman dissects the anatomy of C and C++, from variables to functions and loops, and explores both the C Standard Library and the C++ Standard Template Library. "
                });
                plans.Add(new Plan()
                {
                    Name = "Python 3 Course: Beginner to Advanced",
                    Description =
                        "This course is designed to fully immerse you in the Python language, so it is great for both beginners and veteran programmers! Learn Python through the basics of programming, advanced Python concepts, coding a calculator, essential modules, web scraping, PyMongo, WebPy development, Django web framework, GUI programming, data visualization, machine learning."
                });

                //Assigning Id's for plans
                for (int i = 0, j = 1; i < plans.Count; i++, j++)
                {
                    plans[i].Id = j;
                }

                //Assigning creation date for plans on third of july
                foreach (var plan in plans)
                {
                    plan.Create_Date = new DateTime(2018, 7, 3);
                }

                //Assigning creator of plans on each user by one
                foreach (var plan in plans)
                {
                    plan.Create_Id = plan.Id;
                }

                context.Plans.AddRange(plans);
                context.SaveChanges();
            }
        }

        public static void InitializeGroups(LearnWithMentor_DBEntities context)
        {
            if (!context.Plans.Any())
            {
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

                //Assigning Id's for groups
                for (int i = 0, j = 1; i < groups.Count; i++, j++)
                {
                    groups[i].Id = j;
                }
                //Assigning mentors for groups
                //foreach (var group in groups)
                //{
                //    group.Mentor_Id = group.Id;
                //}

                context.Groups.AddRange(groups);
                context.SaveChanges();
            }
        }

        public static void InitializeTasks(LearnWithMentor_DBEntities context)
        {
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

            context.Tasks.AddRange(tasks);
            context.SaveChanges();
        }

        public static void InitializeSections(LearnWithMentor_DBEntities context)
        {
            List<Section> sections = new List<Section>();
            sections.Add(new Section() { Name = "Base C#" });
            sections.Add(new Section() { Name = "Advaced C#" });
            sections.Add(new Section() { Name = "Full stack app" });
            context.Sections.AddRange(sections);
            context.SaveChanges();
        }

        public static void InitializePlanTask(LearnWithMentor_DBEntities context)
        {
            List<PlanTask> planTasks = new List<PlanTask>();
            //Assigning tasks for plantasks
            //Each plan has 4 tasks
            int count = 0;
            for (int i = 0; i < 10; i++)
            {
                for (int j = count; j < count + 4; j++)
                {
                    planTasks.Add(new PlanTask() { Plan_Id = i, Task_Id = j, Section_Id = j, });
                }

                count = count + 4;
            }

            context.PlanTasks.AddRange(planTasks);
            context.SaveChanges();
        }

        private static void InitializeUserTasks(LearnWithMentor_DBEntities context)
        {
            //Assigning tasks for usertasks
            //Task will have only first seven students
            List<UserTask> userTasks = new List<UserTask>();
            for (int i = 11; i < 17; i++)
            {
                for (int j = 0; j < 24; j++)
                {
                    userTasks.Add(new UserTask()
                    {
                        User_Id = i,
                        PlanTask_Id = j,
                        Mentor_Id = 0,
                        State = "Done",
                        Result = "Ref to GitHub"
                    });
                    userTasks.Add(new UserTask()
                    {
                        User_Id = i,
                        PlanTask_Id = j,
                        Mentor_Id = 0,
                        State = "In progress",
                        Result = "ref to GitHub"
                    });
                    userTasks.Add(new UserTask()
                    {
                        User_Id = i,
                        PlanTask_Id = j,
                        Mentor_Id = 0,
                        State = "In progress",
                        Result = "ref to GitHub"
                    });
                    userTasks.Add(new UserTask()
                    {
                        User_Id = i,
                        PlanTask_Id = j,
                        Mentor_Id = 0,
                        State = "Done",
                        Result = "Ref to GitHub"
                    });
                }
            }

            context.UserTasks.AddRange(userTasks);
        }

        public static void InitializeMessages(LearnWithMentor_DBEntities context){
           
            //Creating messages
            List<Message> messages = new List<Message>();
            messages.Add(new Message() { UserTask_Id = 0, User_Id = 11, Text = "Sorry, i've got cold." });
            messages.Add(
                new Message() { UserTask_Id = 0, User_Id = 12, Text = "Hello. I've done this task. Can you review?" });
            messages.Add(
                new Message() { UserTask_Id = 0, User_Id = 13, Text = "I have problem with this task. Can you help me?" });
            messages.Add(new Message() { UserTask_Id = 0, User_Id = 14, Text = "There some bugs in my code." });
            messages.Add(new Message() { UserTask_Id = 0, User_Id = 0, Text = "Here you have new info about your task." });
            context.Messages.AddRange(messages);
        }

        private static void InitializeComments(LearnWithMentor_DBEntities context)
        {
            //Creating comments
            List<Comment> comments = new List<Comment>();
            comments.Add(new Comment() { PlanTask_Id = 3, Create_Id = 11, Text = "Nice task" });
            comments.Add(new Comment() { PlanTask_Id = 2, Create_Id = 12, Text = "Easy task" });
            comments.Add(new Comment() { PlanTask_Id = 1, Create_Id = 13, Text = "Hard task" });
            context.Comments.AddRange(comments);
        }
    }
}






