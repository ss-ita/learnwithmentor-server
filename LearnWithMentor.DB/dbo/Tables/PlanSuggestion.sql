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
