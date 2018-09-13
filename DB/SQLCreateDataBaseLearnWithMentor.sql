--USE master
--IF EXISTS(select * from sys.databases where name='LearnWithMentor_DB')
--DROP DATABASE LearnWithMentor_DB

--CREATE DATABASE LearnWithMentor_DB
--Go 
use LearnWithMentor_DB

CREATE TABLE Roles
(
    Id INT IDENTITY,
    Name NVARCHAR(50) NOT NULL,
    
CONSTRAINT PK_Role_Id PRIMARY KEY (Id),
)

CREATE TABLE Users
(
    Id INT IDENTITY,
    FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(50) NOT NULL,
    Email_Confirmed BIT NOT NULL CONSTRAINT DF_User_Email_Confirmed DEFAULT 0,
	Password NVARCHAR(100) NOT NULL,
	Role_Id INT NOT NULL,
	Blocked BIT NOT NULL CONSTRAINT DF_User_Blocked DEFAULT 0,
	Image VARCHAR(MAX),
	Image_Name NVARCHAR(1000),

CONSTRAINT PK_Users_Id PRIMARY KEY (Id),
CONSTRAINT UQ_Users_Email UNIQUE (Email),
CONSTRAINT FK_Users_To_Roles FOREIGN KEY (Role_Id)  REFERENCES Roles (Id)
)

CREATE TABLE Groups
(
    Id INT IDENTITY,
    Name NVARCHAR(50) NOT NULL,
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
	Image VARCHAR(MAX),
	Image_Name NVARCHAR(1000),
    Published BIT  NOT NULL CONSTRAINT DF_Plans_Published  DEFAULT  0,
    Create_Id INT NOT NULL,
	Mod_Id INT,
	Create_Date DATETIME,
	Mod_Date DATETIME,


CONSTRAINT PK_Plans_Id PRIMARY KEY (Id),
CONSTRAINT FK_Plans_To_UsersC FOREIGN KEY (Create_Id)  REFERENCES Users (Id),
CONSTRAINT FK_Plans_To_UsersM FOREIGN KEY (Mod_Id)  REFERENCES Users (Id)
)

CREATE TABLE Sections
(
	Id INT IDENTITY,
	Name NVARCHAR(50) NOT NULL,

CONSTRAINT PK_Sections_Id PRIMARY KEY (Id),
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
	Private BIT NOT NULL CONSTRAINT DF_Tasks_Private DEFAULT 1,
	Create_Id INT NOT NULL,
	Mod_Id INT,
	Create_Date DATETIME,
	Mod_Date DATETIME,
    
CONSTRAINT PK_Tasks_Id PRIMARY KEY (Id),
CONSTRAINT FK_Tasks_To_UsersC FOREIGN KEY (Create_Id)  REFERENCES Users (Id),
CONSTRAINT FK_Tasks_To_UsersM FOREIGN KEY (Mod_Id)  REFERENCES Users (Id)
)

CREATE TABLE PlanTasks
(
	Id INT IDENTITY,
    Plan_Id INT NOT NULL,
    Task_Id INT NOT NULL,
	Priority INT,
	Section_Id INT,   
    
 CONSTRAINT PK_PlanTasks_Id PRIMARY KEY (Id),
 CONSTRAINT FK_PlanTasks_To_Plans FOREIGN KEY (Plan_Id)  REFERENCES Plans (Id),
 CONSTRAINT FK_PlanTasks_To_Tasks FOREIGN KEY (Task_Id)  REFERENCES Tasks (Id),
 CONSTRAINT FK_PlanTasks_To_Sections FOREIGN KEY (Section_Id)  REFERENCES Sections (Id)
)

CREATE TABLE UserTasks
(
    Id INT IDENTITY,
    User_Id INT NOT NULL,
    PlanTask_Id INT NOT NULL,
	Mentor_Id INT NOT NULL,   
    State NCHAR NOT NULL CONSTRAINT DF_UserTasks_State DEFAULT 'P',
    End_Date DATETIME,
    Result NVARCHAR(MAX) NOT NULL, 
	Propose_End_Date DATETIME,

 CONSTRAINT PK_UserTasks_Id PRIMARY KEY (Id),
 CONSTRAINT FK_UserTasks_To_Users FOREIGN KEY (User_Id)  REFERENCES Users (Id),
 CONSTRAINT FK_UserTasks_To_PlanTasks FOREIGN KEY (PlanTask_Id)  REFERENCES PlanTasks (Id),
 CONSTRAINT FK_UserTasks_To_UsersMentor FOREIGN KEY (Mentor_Id)  REFERENCES Users (Id),
 CONSTRAINT CK_UserTasks_State CHECK(State IN ('P', 'D', 'A', 'R'))
)

CREATE TABLE Messages
(
    Id INT IDENTITY,
	UserTask_Id INT NOT NULL,
    User_Id INT NOT NULL,
    Text NVARCHAR(1000) NOT NULL,
    Send_Time DATETIME,

CONSTRAINT PK_Masssage_Id PRIMARY KEY (Id),
CONSTRAINT FK_Masssage_To_Users FOREIGN KEY (User_Id)  REFERENCES Users (Id),
CONSTRAINT FK_Masssage_To_UserTasks FOREIGN KEY (UserTask_Id)  REFERENCES UserTasks (Id)
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

CREATE TABLE Comments
(
    Id INT IDENTITY,
    PlanTask_Id INT NOT NULL,    
    Text NVARCHAR(2000) NOT NULL,
	Create_Id INT NOT NULL,	
	Create_Date DATETIME,
	Mod_Date DATETIME,

 CONSTRAINT PK_Comments_Id PRIMARY KEY (Id),
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