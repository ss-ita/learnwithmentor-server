
USE master
GO

IF EXISTS(select * from sys.databases where name='LearnWithMentor_DB')
DROP DATABASE LearnWithMentor_DB
GO

CREATE DATABASE LearnWithMentor_DB
GO

use LearnWithMentor_DB
GO

IF OBJECT_ID('[dbo].[UERS_ROLES]') IS NOT NULL
  DROP VIEW [dbo].[USERS_ROLES]
GO
