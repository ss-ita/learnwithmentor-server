use LearnWithMentor_DB;
delete PlanSuggestion;
delete UserTasksMessages;
delete Messages;
delete Comments;
delete UserTasks;
delete PlanTasks;
delete Tasks;
delete GroupPlans;
delete Sections;
delete Plans;
delete UserGroups;
delete Groups;
delete Users;
delete Roles;

go

DBCC CHECKIDENT ('[Messages]', RESEED, 0);
DBCC CHECKIDENT ('[Comments]', RESEED, 0);
DBCC CHECKIDENT ('[UserTasks]', RESEED, 0);
DBCC CHECKIDENT ('[Tasks]', RESEED, 0);
DBCC CHECKIDENT ('[Sections]', RESEED, 0);
DBCC CHECKIDENT ('[Plans]', RESEED, 0);
DBCC CHECKIDENT ('[Users]', RESEED, 0);
DBCC CHECKIDENT ('[Roles]', RESEED, 0);

go 
insert into Roles (Name) values ('Mentor'),('Student'),('Admin');

insert into Users ( FirstName, LastName, Email, Password, Role_Id) SELECT 'Volodymyr', 'Savchuk', 'v.v@gmail.com','123', Id FROM Roles WHERE Name = 'Mentor';
insert into Users ( FirstName, LastName, Email, Password, Role_Id) SELECT 'Vyacheslav', 'Koldovsky', 'vyacheslav@gmail.com','123',  Id FROM Roles WHERE Name = 'Mentor';
insert into Users ( FirstName, LastName, Email, Password, Role_Id) SELECT 'Andrew', 'Lysyi', 'andrew.l@gmail.com','123',  Id FROM Roles WHERE Name = 'Student';
insert into Users ( FirstName, LastName, Email, Password, Role_Id) SELECT 'Roman', 'Parobiy', 'roman.p@gmail.com','123', Id FROM Roles WHERE Name = 'Student';
insert into Users ( FirstName, LastName, Email, Password, Role_Id) SELECT 'Andriy', 'Panchyshyn', 'andr@gmail.com','123',  Id FROM Roles WHERE Name = 'Student';
insert into Users ( FirstName, LastName, Email, Password, Role_Id) SELECT 'Roman', 'Maksymyshyn', 'riman@gmail.com','123',  Id FROM Roles WHERE Name = 'Student';
insert into Users ( FirstName, LastName, Email, Password, Role_Id) SELECT 'admin', 'admin', 'admin@gmail.com','123',  Id FROM Roles WHERE Name = 'Admin';


insert into Groups(Name, Mentor_Id) SELECT 'lv.net319', Id FROM Users WHERE Email = 'v.v@gmail.com';
insert into Groups(Name, Mentor_Id) SELECT 'lv.net307', Id FROM Users WHERE Email = 'vyacheslav@gmail.com';
insert into Groups(Name, Mentor_Id) SELECT 'lvjava304', Id FROM Users WHERE Email = 'vyacheslav@gmail.com';
insert into Groups(Name, Mentor_Id) SELECT 'lvdevops318', Id FROM Users WHERE Email = 'vyacheslav@gmail.com';

insert into UserGroups( Group_Id, User_Id) SELECT Groups.Id, Users.Id FROM Groups, Users WHERE Groups.Name = 'lv.net319' AND Users.Email = 'andrew.l@gmail.com';
insert into UserGroups( Group_Id, User_Id) SELECT Groups.Id, Users.Id FROM Groups, Users WHERE Groups.Name = 'lv.net319' AND Users.Email = 'roman.p@gmail.com';
insert into UserGroups( Group_Id, User_Id) SELECT Groups.Id, Users.Id FROM Groups, Users WHERE Groups.Name = 'lv.net319' AND Users.Email = 'riman@gmail.com';
insert into UserGroups( Group_Id, User_Id) SELECT Groups.Id, Users.Id FROM Groups, Users WHERE Groups.Name = 'lv.net319' AND Users.Email = 'andr@gmail.com';
insert into UserGroups( Group_Id, User_Id) SELECT Groups.Id, Users.Id FROM Groups, Users WHERE Groups.Name = 'lv.net307' AND Users.Email = 'andrew.l@gmail.com';

insert into Plans ( Name, Description, Published, Create_Id) SELECT 'C#','Learning C#', 1, Id FROM Users WHERE Email = 'vyacheslav@gmail.com';
insert into Plans ( Name, Description, Published, Create_Id) SELECT 'Java','Learning Java', 0, Id FROM Users WHERE Email = 'vyacheslav@gmail.com';
insert into Plans ( Name, Description, Published, Create_Id) SELECT 'C++','Learning C++', 1, Id FROM Users WHERE Email = 'vyacheslav@gmail.com';
insert into Plans ( Name, Description, Published, Create_Id) SELECT 'devOps','Learning devOps', 1, Id FROM Users WHERE Email = 'v.v@gmail.com';

insert into Sections (Name) SELECT 'First section';

insert into GroupPlans(Group_Id, Plan_Id) SELECT Groups.Id, Plans.Id FROM Groups, Plans WHERE Groups.Name = 'lv.net319' AND Plans.Name = 'C#';
insert into GroupPlans(Group_Id, Plan_Id) SELECT Groups.Id, Plans.Id FROM Groups, Plans WHERE Groups.Name = 'lv.net307' AND Plans.Name = 'C#';
insert into GroupPlans(Group_Id, Plan_Id) SELECT Groups.Id, Plans.Id FROM Groups, Plans WHERE Groups.Name = 'lvjava304' AND Plans.Name = 'Java';
insert into GroupPlans(Group_Id, Plan_Id) SELECT Groups.Id, Plans.Id FROM Groups, Plans WHERE Groups.Name = 'lvdevops318' AND Plans.Name = 'devOps';

insert into Tasks ( Name, Description, Create_Id) SELECT 'OOP', 'learning oop', Id FROM Users WHERE Email = 'vyacheslav@gmail.com';
insert into Tasks ( Name, Description, Create_Id) SELECT 'LINQ', 'select, orderby', Id FROM Users WHERE Email = 'v.v@gmail.com';
insert into Tasks ( Name, Description, Create_Id) SELECT 'Collections', 'generic, list', Id FROM Users WHERE Email = 'vyacheslav@gmail.com';
insert into Tasks ( Name, Description, Create_Id) SELECT 'Exceptions', 'try, catch, finally', Id FROM Users WHERE Email = 'v.v@gmail.com';

insert into PlanTasks(Plan_Id, Task_Id) SELECT Plans.Id, Tasks.Id FROM Plans, Tasks WHERE Plans.Name = 'C#' AND Tasks.Name = 'OOP';
insert into PlanTasks(Plan_Id, Task_Id, Section_Id) SELECT Plans.Id, Tasks.Id, Sections.Id FROM Plans, Tasks, Sections WHERE Plans.Name = 'Java' AND Tasks.Name = 'OOP' AND Sections.Name = 'First section';
insert into PlanTasks(Plan_Id, Task_Id) SELECT Plans.Id, Tasks.Id FROM Plans, Tasks WHERE Plans.Name = 'C#' AND Tasks.Name = 'Collections';
insert into PlanTasks(Plan_Id, Task_Id) SELECT Plans.Id, Tasks.Id FROM Plans, Tasks WHERE Plans.Name = 'C#' AND Tasks.Name = 'Exceptions';

insert into UserTasks(User_Id,Task_Id,State,End_Date,Result) SELECT Users.Id, Tasks.Id, 'p','2018-08-10','link msdn' FROM Users, Tasks WHERE Users.Email = 'roman.p@gmail.com' AND Tasks.Name = 'OOP';
insert into UserTasks(User_Id,Task_Id,State,End_Date,Result) SELECT Users.Id, Tasks.Id, 'p','2018-09-08','link stackoverflow' FROM Users, Tasks WHERE Users.Email = 'riman@gmail.com' AND Tasks.Name = 'Collections';
insert into UserTasks(User_Id,Task_Id,State,End_Date,Result) SELECT Users.Id, Tasks.Id, 'p','2018-10-09','link github' FROM Users, Tasks WHERE Users.Email = 'andrew.l@gmail.com' AND Tasks.Name = 'Exceptions';
insert into UserTasks(User_Id,Task_Id,State,End_Date,Result, Propose_End_Date) SELECT Users.Id, Tasks.Id, 'p','2018-08-03','link google.com', '2018-08-13T14:25:10' FROM Users, Tasks WHERE Users.Email = 'andrew.l@gmail.com' AND Tasks.Name = 'Exceptions';

insert into Comments(Create_Id, Task_Id, Text) SELECT Users.Id, Tasks.Id, 'first comment' FROM Users, Tasks WHERE Users.Email = 'roman.p@gmail.com' AND Tasks.Name = 'OOP';
insert into Comments(Create_Id, Task_Id, Text) SELECT Users.Id, Tasks.Id, 'second comment' FROM Users, Tasks WHERE Users.Email = 'andrew.l@gmail.com' AND Tasks.Name = 'Collections';
insert into Comments(Create_Id, Task_Id, Text) SELECT Users.Id, Tasks.Id, 'third comment' FROM Users, Tasks WHERE Users.Email = 'riman@gmail.com' AND Tasks.Name = 'OOP';
insert into Comments(Create_Id, Task_Id, Text) SELECT Users.Id, Tasks.Id, 'another comment' FROM Users, Tasks WHERE Users.Email = 'riman@gmail.com' AND Tasks.Name = 'Collections';

insert into Messages(User_Id, Text, Send_Time) SELECT Id, 'Hello', '2018-08-13T14:25:15' FROM Users WHERE Email = 'vyacheslav@gmail.com';
insert into Messages(User_Id, Text, Send_Time) SELECT Id, 'Hello guys', '2018-08-13T14:15:10' FROM Users WHERE Email = 'vyacheslav@gmail.com';
insert into Messages(User_Id, Text, Send_Time) SELECT Id, 'Hello world', '2018-08-13T14:15:10' FROM Users WHERE Email = 'riman@gmail.com';

insert into UserTasksMessages(Message_Id, Usertask_Id) SELECT Messages.Id, UserTasks.Id FROM Messages, UserTasks WHERE Messages.Text = 'Hello' AND UserTasks.Result = 'link msdn';
insert into UserTasksMessages(Message_Id, Usertask_Id) SELECT Messages.Id, UserTasks.Id FROM Messages, UserTasks WHERE Messages.Text = 'Hello guys' AND UserTasks.Result = 'link google.com';
insert into UserTasksMessages(Message_Id, Usertask_Id) SELECT Messages.Id, UserTasks.Id FROM Messages, UserTasks WHERE Messages.Text = 'Hello world' AND UserTasks.Result = 'link stackoverflow';

insert into PlanSuggestion( Plan_Id, User_Id, Mentor_Id,Text) SELECT Plans.Id, U1.Id, U2.Id, 'first new task' FROM Plans, Users AS U1, Users AS U2 WHERE Plans.Name = 'C#' AND U1.Email = 'roman.p@gmail.com' AND U2.Email = 'vyacheslav@gmail.com';
