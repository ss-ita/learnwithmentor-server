USE master
IF EXISTS(select * from sys.databases where name='LearnWithMentorDB')
DROP DATABASE LearnWithMentorDB

CREATE DATABASE LearnWithMentorDB
Go

use LearnWithMentorDB

CREATE TABLE Roles
(
    Id INT IDENTITY(1,1),
    Name NVARCHAR(MAX) NULL,
    
CONSTRAINT PK_Role_Id PRIMARY KEY CLUSTERED(Id ASC),
)

CREATE TABLE Users
(
    Id INT IDENTITY(1,1),
    FirstName NVARCHAR(MAX) NOT NULL,
	LastName NVARCHAR(MAX) NOT NULL,
    Email NVARCHAR(MAX) NOT NULL,
    Email_Confirmed BIT NOT NULL,
	Password NVARCHAR(MAX) NOT NULL,
	Role_Id INT NOT NULL,
	Blocked BIT NOT NULL,
	Image NVARCHAR(MAX),
	Image_Name NVARCHAR(MAX),

CONSTRAINT PK_Users_Id PRIMARY KEY CLUSTERED(Id),
CONSTRAINT FK_Users_To_Roles FOREIGN KEY (Role_Id)  REFERENCES Roles (Id)
)

CREATE TABLE Groups
(
    Id INT IDENTITY(1,1),
    Name NVARCHAR(MAX) NOT NULL,
    Mentor_Id INT NOT NULL,

CONSTRAINT PK_Group_Id PRIMARY KEY CLUSTERED(Id),
--CONSTRAINT UQ_Group_Name UNIQUE (Name),
CONSTRAINT FK_Groups_To_Users FOREIGN KEY (Mentor_Id)  REFERENCES Users (Id)
)

CREATE TABLE UserGroup
(
    Group_Id INT NOT NULL,
    User_Id INT NOT NULL,  
	
 CONSTRAINT PK_UserGroup PRIMARY KEY CLUSTERED( User_Id ASC, Group_Id ASC),
 CONSTRAINT FK_UserGroups_To_Groups FOREIGN KEY (Group_Id)  REFERENCES Groups (Id),
 CONSTRAINT FK_UserGroups_To_Users FOREIGN KEY (User_Id)  REFERENCES Users (Id)
)

CREATE TABLE Plans
(
    Id INT IDENTITY(1,1),
    Name NVARCHAR(MAX) NOT NULL,
    Description NVARCHAR(MAX),
	Image VARCHAR(MAX),
	Image_Name NVARCHAR(MAX),
    Published BIT  NOT NULL,
    Create_Id INT NOT NULL,
	Mod_Id INT,
	Create_Date DATETIME,
	Mod_Date DATETIME,


CONSTRAINT PK_Plans_Id PRIMARY KEY  CLUSTERED(Id ASC),
CONSTRAINT FK_Plans_To_UsersC FOREIGN KEY (Create_Id)  REFERENCES Users (Id),
CONSTRAINT FK_Plans_To_UsersM FOREIGN KEY (Mod_Id)  REFERENCES Users (Id)
)

CREATE TABLE Sections
(
	Id INT IDENTITY(1,1) NOT NULL,
	Name NVARCHAR(MAX) NOT NULL,

CONSTRAINT PK_Sections_Id PRIMARY KEY (Id ASC),
)

CREATE TABLE GroupPlan
(
    GroupId INT ,
    PlanId INT NOT NULL,    
    
 CONSTRAINT PK_GroupPlan PRIMARY KEY CLUSTERED(GroupId ASC, PlanId ASC),
 CONSTRAINT FK_GroupPlans_To_Groups FOREIGN KEY (GroupId)  REFERENCES Groups (Id) ON DELETE CASCADE,
 CONSTRAINT FK_GroupPlans_To_Plans FOREIGN KEY (PlanId)  REFERENCES Plans (Id) ON DELETE CASCADE
)

CREATE TABLE Tasks
(
    Id INT IDENTITY(1,1) NOT NULL,
    Name NVARCHAR(MAX)  NULL,
    Description NVARCHAR(MAX) NULL ,
	Private BIT NOT NULL,
	Create_Id INT NOT NULL,
	Mod_Id INT,
	Create_Date DATETIME,
	Mod_Date DATETIME,
    
CONSTRAINT PK_Tasks_Id PRIMARY KEY  CLUSTERED(Id ASC),
CONSTRAINT FK_Tasks_To_UsersC FOREIGN KEY (Create_Id)  REFERENCES Users (Id),
CONSTRAINT FK_Tasks_To_UsersM FOREIGN KEY (Mod_Id)  REFERENCES Users (Id)
)

CREATE TABLE PlanTasks
(
	Id INT IDENTITY(1,1),
    Plan_Id INT NOT NULL,
    Task_Id INT NOT NULL,
	Priority INT,
	Section_Id INT,   
    
 CONSTRAINT PK_PlanTasks_Id PRIMARY KEY  CLUSTERED(Id ASC),
 CONSTRAINT FK_PlanTasks_To_Plans FOREIGN KEY (Plan_Id)  REFERENCES Plans (Id),
 CONSTRAINT FK_PlanTasks_To_Tasks FOREIGN KEY (Task_Id)  REFERENCES Tasks (Id),
 CONSTRAINT FK_PlanTasks_To_Sections FOREIGN KEY (Section_Id)  REFERENCES Sections (Id)
)

CREATE TABLE UserTasks
(
    Id INT IDENTITY(1,1),
    User_Id INT NOT NULL,
    PlanTask_Id INT NOT NULL,
	Mentor_Id INT NOT NULL,   
    State NCHAR NULL,
    End_Date DATETIME,
    Result NVARCHAR(MAX) NULL, 
	Propose_End_Date DATETIME,

 CONSTRAINT PK_UserTasks_Id PRIMARY KEY  CLUSTERED(Id ASC),
 CONSTRAINT FK_UserTasks_To_Users FOREIGN KEY (User_Id)  REFERENCES Users (Id),
 CONSTRAINT FK_UserTasks_To_PlanTasks FOREIGN KEY (PlanTask_Id)  REFERENCES PlanTasks (Id),
 CONSTRAINT FK_UserTasks_To_UsersMentor FOREIGN KEY (Mentor_Id)  REFERENCES Users (Id),
 --CONSTRAINT CK_UserTasks_State CHECK(State IN ('P', 'D', 'A', 'R'))
)

CREATE TABLE Messages
(
    Id INT IDENTITY(1,1) NOT NULL ,
	UserTask_Id INT NOT NULL,
    User_Id INT NOT NULL,
    Text NVARCHAR(MAX) NOT NULL,
    Send_Time DATETIME,

CONSTRAINT PK_Masssage_Id PRIMARY KEY  CLUSTERED(Id),
CONSTRAINT FK_Masssage_To_Users FOREIGN KEY (User_Id)  REFERENCES Users (Id),
CONSTRAINT FK_Masssage_To_UserTasks FOREIGN KEY (UserTask_Id)  REFERENCES UserTasks (Id)
)

CREATE TABLE PlanSuggestions
(
	Id  INT IDENTITY(1,1) NOT NULL,
    Plan_Id INT NOT NULL,
    User_Id INT NOT NULL,    
    Mentor_Id INT NOT NULL,    
    Text NVARCHAR(MAX) NOT NULL,
 
 CONSTRAINT PK_PlanSuggestions PRIMARY KEY  CLUSTERED(Id ASC),
 CONSTRAINT FK_PlanSuggestions_To_Plans FOREIGN KEY (Plan_Id)  REFERENCES Plans (Id),
 CONSTRAINT FK_PlanSuggestions_To_Users FOREIGN KEY (User_Id )  REFERENCES Users (Id),
 CONSTRAINT FK_PlanSuggestions_To_Users1 FOREIGN KEY (Mentor_Id)  REFERENCES Users (Id)
)

CREATE TABLE Comments
(
    Id INT IDENTITY (1,1) NOT NULL,
    PlanTask_Id INT NOT NULL,    
    Text NVARCHAR(MAX) NOT NULL,
	Create_Id INT NOT NULL,	
	Create_Date DATETIME,
	Mod_Date DATETIME,

 CONSTRAINT PK_Comments_Id PRIMARY KEY  CLUSTERED(Id),
 CONSTRAINT FK_Comments_To_PlanTasks FOREIGN KEY (PlanTask_Id)  REFERENCES PlanTasks (Id),
 CONSTRAINT FK_Comments_To_UsersC FOREIGN KEY (Create_Id)  REFERENCES Users (Id)
)

GO

CREATE TRIGGER T_Insert_Messages
ON Messages AFTER INSERT
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM INSERTED) OR (TRIGGER_NESTLEVEL() > 1)
		RETURN
	UPDATE Messages
	SET Send_Time = GETDATE()
	WHERE Id IN (SELECT Id FROM INSERTED)
END;
GO

CREATE TRIGGER T_Insert_Plans
ON Plans AFTER INSERT
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM INSERTED) OR (TRIGGER_NESTLEVEL() > 1)
		RETURN
	UPDATE Plans
	SET Create_Date = GETDATE()
	WHERE Id IN (SELECT Id FROM INSERTED)
END;
GO

CREATE TRIGGER T_Update_Plans
ON Plans AFTER UPDATE
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM INSERTED) OR (TRIGGER_NESTLEVEL() > 1)
		RETURN
	UPDATE Plans
	SET Mod_Date = GETDATE()
	WHERE Id IN (SELECT Id FROM INSERTED)
END;
GO

CREATE TRIGGER T_Insert_Tasks
ON Tasks AFTER INSERT
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM INSERTED) OR (TRIGGER_NESTLEVEL() > 1)
		RETURN
	UPDATE Tasks
	SET Create_Date = GETDATE()
	WHERE Id IN (SELECT Id FROM INSERTED)
END;
GO

CREATE TRIGGER T_Update_Tasks
ON Tasks AFTER UPDATE
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM INSERTED) OR (TRIGGER_NESTLEVEL() > 1)
		RETURN
	UPDATE Tasks
	SET Mod_Date = GETDATE()
	WHERE Id IN (SELECT Id FROM INSERTED)
END;
GO

CREATE TRIGGER T_Insert_Comments
ON Comments AFTER INSERT
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM INSERTED) OR (TRIGGER_NESTLEVEL() > 1)
		RETURN
	UPDATE Comments
	SET Create_Date = GETDATE()
	WHERE Id IN (SELECT Id FROM INSERTED)
END;
GO

CREATE TRIGGER T_Update_Comments
ON Comments AFTER UPDATE
AS
BEGIN
	IF NOT EXISTS (SELECT * FROM INSERTED) OR (TRIGGER_NESTLEVEL() > 1)
		RETURN
	UPDATE Comments
	SET Mod_Date = GETDATE()
	WHERE Id IN (SELECT Id FROM INSERTED)
END;

IF OBJECT_ID('[dbo].[GROUPS_-PLANS-TASKS]') IS NOT NULL
  DROP VIEW [dbo].[GROUPS-PLANS-TASKS]
GO
CREATE VIEW [dbo].[GROUPS-PLANS-TASKS]
AS
SELECT        dbo.Plans.Name, dbo.Plans.Description, dbo.Plans.Create_Date, dbo.Plans.Mod_Date, dbo.Plans.Published, dbo.PlanTasks.Priority, dbo.Sections.Name AS Section_Name, dbo.Tasks.Name AS Task_Name, dbo.Tasks.Description AS Task_Description, 
                         dbo.Tasks.Create_Date AS Tasks_Create_Date, dbo.Tasks.Mod_Date AS Task_Mod_Date, dbo.Tasks.Private
FROM            dbo.Plans INNER JOIN
                         dbo.PlanTasks ON dbo.Plans.Id = dbo.PlanTasks.Plan_Id INNER JOIN
                         dbo.Sections ON dbo.PlanTasks.Section_Id = dbo.Sections.Id INNER JOIN
                         dbo.Tasks ON dbo.PlanTasks.Task_Id = dbo.Tasks.Id
GO

IF OBJECT_ID('[dbo].[UERS_ROLES]') IS NOT NULL
  DROP VIEW [dbo].[USERS_ROLES]
GO
CREATE VIEW [dbo].[USERS_ROLES]
AS
SELECT        dbo.Users.FirstName, dbo.Users.LastName, dbo.Roles.Name AS Roles_Name, dbo.Users.Email
FROM            dbo.Roles INNER JOIN
                         dbo.Users ON dbo.Roles.Id = dbo.Users.Role_Id
GO

CREATE PROCEDURE sp_Total_Ammount_of_Users
	@Total INT OUTPUT
AS
SELECT @Total = COUNT(*) FROM Users