# learnwithmentor-server(Local)
LearnWithMentor is a helper service with plan-based system of learning
# Requirements
Microsoft SQL Server 2017 and Visual Studio 2017 are required.
# Installation
1. Clone this repository to your computer.
2. Install Entity Framework with NuGet Packages Manager.
3. Create an empty DB with name LearnWithMentor.DB.
4. Add conection strings to LearnWithMentor/Web.config and LearnWithMentorDAL/app.config.
5. If you have installed express edition of sql server, add \SQLEXPRESS to Data Source section in connection strings in     LearnWithMentor/Web.config and LearnWithMentorDAL/app.config.
6. Publish script for creating and initializing database to your local DB:
    * right click on LearnWithMentor.DB(SQL project) and choose publish.
    * choose your DB in Target database connection.
    * click Generate Script;
    * execute script that has been generated.
7. Start learnwithmentor.sln.
