use LearnWithMentor_DB;
delete UserRoles;
delete Roles;
delete PlanSuggestion;
delete EndDateSuggestions;
delete UserTasksMessages;
delete Messages;
delete Comment;
delete UserTasks;
delete Activities;
delete PlanTasks;
delete Tasks;
delete GroupPlans;
delete Plans;
delete UserGroups;
delete Groups;
delete Users;

go 
insert into Users ( Name, Email,Password) values ('Volodymyr', 'v.v@gmail.com','123');
insert into Users ( Name, Email,Password) values ('Vyacheslav', 'vyacheslav@gmail.com','123');
insert into Users ( Name, Email,Password) values ('Andrew', 'andrew.l@gmail.com','123');
insert into Users ( Name, Email,Password) values ('Roman', 'roman.p@gmail.com','123');
insert into Users ( Name, Email,Password) values ('Andriy', 'andr@gmail.com','123');
insert into Users ( Name, Email,Password) values ('Roman', 'riman@gmail.com','123');


insert into Roles (Name) values ('Mentor'),('Student'),('Admin');

insert into Groups(Name, Description,Mentor_Id) values 
( 'lv.net319', '.net group',null),
( 'lv.net307', 'pokemons group',null),
( 'lvjava304', 'java group',null),
( 'lvdevops318', 'devOps group',null);

insert into UserGroups( Group_Id, User_Id) values ( 1,2),
( 1,1),
( 1,3),
( 1,4),
( 2,1);

insert into UserRoles ( Role_Id,User_Id) values ( 1,2),
( 3,1),
( 2,3),
( 2,4),
( 2,5),
( 2,6);



insert  into   Plans ( Name, Description, Published,Owner_Id) values ('C#','Learning C#' ,1, 2),
('Java','Learning Java' ,0, 2),
('C++','Learning C++' ,0, 1),
('devOps','Learning devOps' ,1, 2);


insert into GroupPlans (Group_Id, Plan_Id) values (1,1),
(2,2),
(3,3),
(4,4);

insert into Tasks ( Name, Description) values 
('OOP', 'learning oop'),
('LINQ', 'select, orderby'),
('Collections', 'generic, list'),
('Exceptions', 'try, catch, finally');


insert into PlanTasks(Plan_Id,Task_Id) values ( 1,1),
(2,2),
(3,3),
(4,4);

insert into Activities( Group_Id, Task_Id, End_Date) values ( 1,1,'2018-06-11'),
( 1,1,'2018-06-11'),
( 2,2,'2018-08-10'),
( 3,4,'2018-11-08');

insert into UserTasks(User_Id,Activity_Id,State,End_Date,Result) values (3,1,'p','2018-08-03','link google.com'),
(3,3,'p','2018-08-10','link msdn'),
(4,2,'p','2018-09-08','link stackoverflow'),
(5,4,'p','2018-10-09','link github ');

insert into Comment( Publish_Date, User_Id, Task_Id, Edit_Date, Text) values ( '2018-05-12T14:25:10', 3,2,Null, 'first comment '),
( '2018-04-13T10:05:10', 4,4,Null, 'second comment '),
( '2018-04-23T11:05:10', 5,3,Null, ' comment after second'),
( '2018-05-15T12:15:10', 3,1,Null, 'last comment ');

insert into Messages(User_Id, Text, Send_Time) values (1, 'Hello', '2018-08-13T14:25:10'),
(2, 'Hello guys', '2018-08-13T14:15:10'),
(3, 'Hello everyone', '2018-08-13T15:25:10'),
(4, 'Hello rabbit', '2018-08-13T14:20:10');



insert into UserTasksMessages(Message_Id, Usertask_Id) values (1,1),(2,2),(3,3),(4,4);
insert into EndDateSuggestions (Usertask_Id, End_Date) values (3, '2018-08-12T14:15:10'),
(2, '2018-08-12T14:15:10'),
(3, '2018-08-12T14:15:10'),
(4, '2018-08-11T14:15:10');

insert into PlanSuggestion( Plan_Id, User_Id, Mentor_Id,Text) values ( 1, 3,1,'first new plan '),
( 2, 4,1,'second new plan '),
( 3, 5,2,'after second new plan '),
( 4, 5,1,'last new plan ');