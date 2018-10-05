CREATE TABLE PlanSuggestion
(
	Id  INT IDENTITY(1,1),
    Plan_Id INT NOT NULL,
    User_Id INT NOT NULL,    
    Mentor_Id INT NOT NULL,    
    Text NVARCHAR(1000) NOT NULL,
 
 CONSTRAINT PK_PlanSuggestions PRIMARY KEY  CLUSTERED(Id),
 CONSTRAINT FK_PlanSuggestion_To_Plans FOREIGN KEY (Plan_Id)  REFERENCES Plans (Id),
 CONSTRAINT FK_PlanSuggestion_To_Users FOREIGN KEY (User_Id )  REFERENCES Users (Id),
 CONSTRAINT FK_PlanSuggestion_To_Users1 FOREIGN KEY (Mentor_Id)  REFERENCES Users (Id)
)
