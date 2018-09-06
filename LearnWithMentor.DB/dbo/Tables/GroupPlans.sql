CREATE TABLE GroupPlans
(
    Group_Id INT NOT NULL,
    Plan_Id INT NOT NULL,    
    
 CONSTRAINT FK_GroupPlans_To_Groups FOREIGN KEY (Group_Id)  REFERENCES Groups (Id),
 CONSTRAINT FK_GroupPlans_To_Plans FOREIGN KEY (Plan_Id)  REFERENCES Plans (Id)
)
