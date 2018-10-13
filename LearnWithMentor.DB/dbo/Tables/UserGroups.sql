CREATE TABLE UserGroups
(
    UserId INT NOT NULL,
    GroupId INT NOT NULL,    
    
 CONSTRAINT PK_UserGroups PRIMARY KEY CLUSTERED( UserId ASC, GroupId ASC),
 CONSTRAINT FK_UserGroups_To_Users FOREIGN KEY (UserId)  REFERENCES Users (Id),
 CONSTRAINT FK_UserGroups_To_Groups FOREIGN KEY (GroupId)  REFERENCES Groups (Id)
)
