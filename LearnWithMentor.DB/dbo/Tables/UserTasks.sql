CREATE TABLE UserTasks
(
    Id INT IDENTITY(1,1),
    User_Id INT NOT NULL,
    PlanTask_Id INT NOT NULL,
	Mentor_Id INT NOT NULL,   
    State NCHAR NOT NULL CONSTRAINT DF_UserTasks_State DEFAULT 'P',
    End_Date DATETIME,
    Result NVARCHAR(MAX) NOT NULL, 
	Propose_End_Date DATETIME,

 CONSTRAINT PK_UserTasks_Id PRIMARY KEY CLUSTERED(Id),
 CONSTRAINT FK_UserTasks_To_Users FOREIGN KEY (User_Id)  REFERENCES Users (Id),
 CONSTRAINT FK_UserTasks_To_PlanTasks FOREIGN KEY (PlanTask_Id)  REFERENCES PlanTasks (Id),
 CONSTRAINT FK_UserTasks_To_UsersMentor FOREIGN KEY (Mentor_Id)  REFERENCES Users (Id),
 --CONSTRAINT CK_UserTasks_State CHECK(State IN ('P', 'D', 'A', 'R'))
)
