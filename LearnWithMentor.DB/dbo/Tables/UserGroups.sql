CREATE TABLE UserGroups
(
    Group_Id INT NOT NULL,
    User_Id INT NOT NULL,    
    
 CONSTRAINT PK_UserGroups PRIMARY KEY CLUSTERED( User_Id ASC, Group_Id ASC),
 CONSTRAINT FK_UserGroups_To_Groups FOREIGN KEY (Group_Id)  REFERENCES Groups (Id),
 CONSTRAINT FK_UserGroups_To_Users FOREIGN KEY (User_Id)  REFERENCES Users (Id)
)
