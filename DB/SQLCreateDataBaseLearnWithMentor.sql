USE master
IF EXISTS(select * from sys.databases where name='LearnWithMentor_DB')
DROP DATABASE LearnWithMentor_DB

CREATE DATABASE LearnWithMentor_DB
Go 
use LearnWithMentor_DB

CREATE TABLE Users
(
    Id INT IDENTITY,
    Name NVARCHAR(50) NOT NULL,
    Email NVARCHAR(50) NOT NULL,
    Password NVARCHAR(50) NOT NULL,

CONSTRAINT PK_User_Id PRIMARY KEY (Id),
CONSTRAINT UQ_User_Email UNIQUE (Email)
)

CREATE TABLE Roles
(
    Id INT IDENTITY,
    Name NVARCHAR(50) NOT NULL,
    
CONSTRAINT PK_Role_Id PRIMARY KEY (Id),
)

CREATE TABLE UserRoles
(
    Role_Id INT NOT NULL,
    User_Id INT NOT NULL,    
    
 CONSTRAINT FK_UserRoles_To_Roles FOREIGN KEY (Role_Id)  REFERENCES Roles (Id),
 CONSTRAINT FK_UserRoles_To_Users FOREIGN KEY (User_Id)  REFERENCES Users (Id)
)

CREATE TABLE Groups
(
    Id INT IDENTITY,
    Name NVARCHAR(50) NOT NULL,
    Description NVARCHAR(500) NOT NULL,
    Mentor_Id INT,

CONSTRAINT PK_Group_Id PRIMARY KEY (Id),
CONSTRAINT UQ_Group_Name UNIQUE (Name),
CONSTRAINT FK_Groups_To_Users FOREIGN KEY (Mentor_Id)  REFERENCES Users (Id)
)

CREATE TABLE UserGroups
(
    Group_Id INT NOT NULL,
    User_Id INT NOT NULL,    
    
 CONSTRAINT FK_UserGroups_To_Groups FOREIGN KEY (Group_Id)  REFERENCES Groups (Id),
 CONSTRAINT FK_UserGroups_To_Users FOREIGN KEY (User_Id)  REFERENCES Users (Id)
)

CREATE TABLE Plans
(
    Id INT IDENTITY,
    Name NVARCHAR(50) NOT NULL,
    Description NVARCHAR(500),
    Published BIT  NOT NULL CONSTRAINT DF_Plans_Published  DEFAULT  0,
    Owner_Id INT NOT NULL,

CONSTRAINT PK_Plans_Id PRIMARY KEY (Id),
CONSTRAINT FK_Plans_To_Users FOREIGN KEY (Owner_Id)  REFERENCES Users (Id)
)

CREATE TABLE GroupPlans
(
    Group_Id INT NOT NULL,
    Plan_Id INT NOT NULL,    
    
 CONSTRAINT FK_GroupPlans_To_Groups FOREIGN KEY (Group_Id)  REFERENCES Groups (Id),
 CONSTRAINT FK_GroupPlans_To_Plans FOREIGN KEY (Plan_Id)  REFERENCES Plans (Id)
)

CREATE TABLE Tasks
(
    Id INT IDENTITY,
    Name NVARCHAR(50) NOT NULL,
    Description NVARCHAR(MAX),
    
CONSTRAINT PK_Tasks_Id PRIMARY KEY (Id),
)

CREATE TABLE PlanTasks
(
    Plan_Id INT NOT NULL,
    Task_Id INT NOT NULL,    
    
 CONSTRAINT FK_PlanTasks_To_Plans FOREIGN KEY (Plan_Id)  REFERENCES Plans (Id),
 CONSTRAINT FK_PlanTasks_To_Tasks FOREIGN KEY (Task_Id)  REFERENCES Tasks (Id)
)

CREATE TABLE Activities
(
    Id INT IDENTITY,
    Group_Id INT NOT NULL,
    Task_Id INT NOT NULL,
    End_Date DATETIME,

CONSTRAINT PK_Activities_Id PRIMARY KEY (Id),
CONSTRAINT FK_Activities_To_Groups FOREIGN KEY (Group_Id)  REFERENCES Groups (Id),
CONSTRAINT FK_Activities_To_Tasks FOREIGN KEY (Task_Id)  REFERENCES Tasks (Id)
)

CREATE TABLE UserTasks
(
    Id INT IDENTITY,
    User_Id INT NOT NULL,
    Activity_Id INT NOT NULL,    
    State NCHAR NOT NULL CONSTRAINT DF_UserTasks_State DEFAULT 'P',
    End_Date DATETIME,
    Result NVARCHAR(MAX) NOT NULL,    

 CONSTRAINT PK_UserTasks_Id PRIMARY KEY (Id),
 CONSTRAINT FK_UserTasks_To_Users FOREIGN KEY (User_Id)  REFERENCES Users (Id),
 CONSTRAINT FK_UserTasks_To_Activities FOREIGN KEY (Activity_Id)  REFERENCES Activities (Id),
 CONSTRAINT CK_UserTasks_State CHECK(State IN ('P', 'D', 'A', 'R'))
)

CREATE TABLE Messages
(
    Id INT IDENTITY,
    User_Id INT NOT NULL,
    Text NVARCHAR(1000) NOT NULL,
    Send_Time DATETIME NOT NULL,

CONSTRAINT PK_Masssage_Id PRIMARY KEY (Id),
CONSTRAINT FK_Masssage_To_Users FOREIGN KEY (User_Id)  REFERENCES Users (Id),
)

CREATE TABLE UserTasksMessages
(
    Message_Id INT NOT NULL,
    Usertask_Id INT NOT NULL,    
    
 CONSTRAINT FK_UserTasksMessages_To_Messsages FOREIGN KEY (Message_Id)  REFERENCES Messages (Id),
 CONSTRAINT FK_UserTasksMessages_To_UserTasks FOREIGN KEY (Usertask_Id)  REFERENCES UserTasks (Id)
)

CREATE TABLE EndDateSuggestions
(
    Usertask_Id INT NOT NULL,
    End_Date DATETIME NOT NULL,
    
 CONSTRAINT FK_EndDateSuggestions_To_UserTasks FOREIGN KEY (Usertask_Id)  REFERENCES UserTasks (Id)
)

CREATE TABLE PlanSuggestion
(
    Plan_Id INT NOT NULL,
    User_Id INT NOT NULL,    
    Mentor_Id INT NOT NULL,    
    Text NVARCHAR(1000) NOT NULL,

 CONSTRAINT FK_PlanSuggestion_To_Plans FOREIGN KEY (Plan_Id)  REFERENCES Plans (Id),
 CONSTRAINT FK_PlanSuggestion_To_Users FOREIGN KEY (User_Id )  REFERENCES Users (Id),
 CONSTRAINT FK_PlanSuggestion_To_Users1 FOREIGN KEY (Mentor_Id)  REFERENCES Users (Id)
)

CREATE TABLE Comment
(
    Id INT IDENTITY,
    User_Id INT,
    Task_Id INT NOT NULL,    
    Text NVARCHAR(2000) NOT NULL,
    Publish_Date DATETIME NOT NULL,
    Edit_Date DATETIME ,

 CONSTRAINT PK_Comment_Id PRIMARY KEY (Id),
 CONSTRAINT FK_Comment_To_Users FOREIGN KEY (User_Id)  REFERENCES Users (Id),
 CONSTRAINT FK_Comment_To_Tasks FOREIGN KEY (Task_Id)  REFERENCES Tasks (Id)
)