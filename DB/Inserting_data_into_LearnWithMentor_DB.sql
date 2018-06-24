use LearnWithMentor_DB;
delete PlanSuggestion;
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

insert into Users ( FirstName, LastName, Email, Password, Role_Id) SELECT 'Volodymyr', 'Savchuk', 'v.v@gmail.com','$2a$10$moX0C/KhFxJmqLWut5TMgOZUhzIg5ClWXyYNIxFZmXQaWM.JQuxVi', Id FROM Roles WHERE Name = 'Mentor';
insert into Users ( FirstName, LastName, Email, Password, Role_Id) SELECT 'Vyacheslav', 'Koldovsky', 'vyacheslav@gmail.com','$2a$10$0ihRdQTYu4INTnqkMolHe.ojX6S6VvEy.rrl2WPYsZPibK0aMOKWC',  Id FROM Roles WHERE Name = 'Mentor';
insert into Users ( FirstName, LastName, Email, Password, Role_Id) SELECT 'Andrew', 'Lysyi', 'andrew.l@gmail.com','$2a$10$jv7BGvoO6/ElJ1/uvQdsfuCcMoln4YSbnqT5CbOZtogC6t.SMcO3y',  Id FROM Roles WHERE Name = 'Student';
insert into Users ( FirstName, LastName, Email, Password, Role_Id) SELECT 'Roman', 'Parobiy', 'roman.p@gmail.com','$2a$10$mnELDFRFHEvUfKI102FR2OQTMIUw6j6iR5IZ7n9Pu.h03/Oylot.i', Id FROM Roles WHERE Name = 'Student';
insert into Users ( FirstName, LastName, Email, Password, Role_Id) SELECT 'Andriy', 'Panchyshyn', 'andr@gmail.com','$2a$10$vSnSmWSaVpxvUKffDJB7meuYxQFF9t4PgLoASwIEdQB3mvNBz/lBS',  Id FROM Roles WHERE Name = 'Student';
insert into Users ( FirstName, LastName, Email, Password, Role_Id) SELECT 'Roman', 'Maksymyshyn', 'riman@gmail.com','$2a$10$eo9yp6BBzj/zt.nOroe8Iu5hO843ljcHG6xFqx8flEofJDvfbFzMK',  Id FROM Roles WHERE Name = 'Student';
insert into Users ( FirstName, LastName, Email, Password, Role_Id) SELECT 'admin', 'admin', 'admin@gmail.com','$2a$10$mTJzuP0fjy4elrx4rwJ4eezmk70QGN9F4.gJp32ImX69caOSEwVlW',  Id FROM Roles WHERE Name = 'Admin';


insert into Groups(Name, Mentor_Id) SELECT 'lv.net319', Id FROM Users WHERE Email = 'vyacheslav@gmail.com';
insert into Groups(Name, Mentor_Id) SELECT 'lv.net307', Id FROM Users WHERE Email = 'vyacheslav@gmail.com';
insert into Groups(Name, Mentor_Id) SELECT 'lvjava304', Id FROM Users WHERE Email = 'vyacheslav@gmail.com';
insert into Groups(Name, Mentor_Id) SELECT 'lvdevops318', Id FROM Users WHERE Email = 'vyacheslav@gmail.com';

insert into UserGroups( Group_Id, User_Id) SELECT Groups.Id, Users.Id FROM Groups, Users WHERE Groups.Name = 'lv.net319' AND Users.Email = 'andrew.l@gmail.com';
insert into UserGroups( Group_Id, User_Id) SELECT Groups.Id, Users.Id FROM Groups, Users WHERE Groups.Name = 'lv.net319' AND Users.Email = 'roman.p@gmail.com';
insert into UserGroups( Group_Id, User_Id) SELECT Groups.Id, Users.Id FROM Groups, Users WHERE Groups.Name = 'lv.net319' AND Users.Email = 'riman@gmail.com';
insert into UserGroups( Group_Id, User_Id) SELECT Groups.Id, Users.Id FROM Groups, Users WHERE Groups.Name = 'lv.net319' AND Users.Email = 'andr@gmail.com';

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

insert into UserTasks(User_Id,PlanTask_Id,Mentor_Id,State,End_Date,Result) SELECT Users.Id, PlanTasks.Id, Groups.Mentor_Id, 'p','2018-08-10','my userTask description' FROM Users, PlanTasks, Groups WHERE Groups.Name = 'lv.net319' AND Users.Email = 'roman.p@gmail.com' AND PlanTasks.Plan_Id = (SELECT Plans.Id FROM Plans WHERE Plans.Name = 'C#');
insert into UserTasks(User_Id,PlanTask_Id,Mentor_Id,State,End_Date,Result) SELECT Users.Id, PlanTasks.Id, Groups.Mentor_Id, 'p','2018-09-08','my work' FROM Users, PlanTasks, Groups WHERE Groups.Name = 'lv.net319' AND Users.Email = 'riman@gmail.com' AND PlanTasks.Plan_Id = (SELECT Plans.Id FROM Plans WHERE Plans.Name = 'C#');
insert into UserTasks(User_Id,PlanTask_Id,Mentor_Id,State,End_Date,Result) SELECT Users.Id, PlanTasks.Id, Groups.Mentor_Id, 'p','2018-10-09','link github' FROM Users, PlanTasks, Groups WHERE Groups.Name = 'lv.net319' AND Users.Email = 'andrew.l@gmail.com' AND PlanTasks.Plan_Id = (SELECT Plans.Id FROM Plans WHERE Plans.Name = 'C#');
insert into UserTasks(User_Id,PlanTask_Id,Mentor_Id,State,End_Date,Result) SELECT Users.Id, PlanTasks.Id, Groups.Mentor_Id, 'p','2018-08-03','link google.com' FROM Users, PlanTasks, Groups WHERE Groups.Name = 'lv.net319' AND Users.Email = 'andr@gmail.com' AND PlanTasks.Plan_Id = (SELECT Plans.Id FROM Plans WHERE Plans.Name = 'C#');

insert into Comments(Create_Id, PlanTask_Id, Text) SELECT Users.Id, PlanTasks.Id, 'first comment' FROM Users, PlanTasks WHERE Users.Email = 'roman.p@gmail.com' AND PlanTasks.Id = 1;
insert into Comments(Create_Id, PlanTask_Id, Text) SELECT Users.Id, PlanTasks.Id, 'second comment' FROM Users, PlanTasks WHERE Users.Email = 'andrew.l@gmail.com' AND PlanTasks.Id = 1;
insert into Comments(Create_Id, PlanTask_Id, Text) SELECT Users.Id, PlanTasks.Id, 'third comment' FROM Users, PlanTasks WHERE Users.Email = 'riman@gmail.com' AND PlanTasks.Id = 2;
insert into Comments(Create_Id, PlanTask_Id, Text) SELECT Users.Id, PlanTasks.Id, 'another comment' FROM Users, PlanTasks WHERE Users.Email = 'riman@gmail.com' AND PlanTasks.Id = 2;

insert into Messages(User_Id, UserTask_Id, Text) SELECT Users.Id, UserTasks.Id, 'Hello' FROM Users, UserTasks WHERE Email = 'vyacheslav@gmail.com' AND UserTasks.Id = 1;
insert into Messages(User_Id, UserTask_Id, Text) SELECT Users.Id, UserTasks.Id, 'Hello guys' FROM Users, UserTasks WHERE Email = 'vyacheslav@gmail.com' AND UserTasks.Id = 2;
insert into Messages(User_Id, UserTask_Id, Text) SELECT Users.Id, UserTasks.Id, 'Hello world' FROM Users, UserTasks WHERE Email = 'roman.p@gmail.com' AND UserTasks.Id = 0;

insert into PlanSuggestion( Plan_Id, User_Id, Mentor_Id,Text) SELECT Plans.Id, U1.Id, U2.Id, 'first new task' FROM Plans, Users AS U1, Users AS U2 WHERE Plans.Name = 'C#' AND U1.Email = 'roman.p@gmail.com' AND U2.Email = 'vyacheslav@gmail.com';
