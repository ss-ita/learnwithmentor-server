﻿CREATE TABLE Sections
(
	Id INT IDENTITY(1,1),
	Name NVARCHAR(50) NOT NULL,

CONSTRAINT PK_Sections_Id PRIMARY KEY CLUSTERED(Id),
)
