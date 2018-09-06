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
