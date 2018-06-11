# learnwithmentor-server
LearnWithMentor is a helper service with plan-based system of learning
# Requirements
Microsoft SQL Server 2017 and Visual Studio 2017 are required.
# Installation
1. Clone this repository to your computer.
2. Run SQLCreateDataBaseLearnWithMentor.sql script from DB folder.
3. Run Inserting data into LearnWithMentor_DB .sql script from DB folder.
4. If you have installed not express edition of sql server, open learnwithmentor.sln in Visual Studio, remove \SQLEXPRESS from Data Source section in connection strings in LearnWithMentor/Web.config and LearnWithMentorDAL/app.config.
5. Start learnwithmentor.sln.
