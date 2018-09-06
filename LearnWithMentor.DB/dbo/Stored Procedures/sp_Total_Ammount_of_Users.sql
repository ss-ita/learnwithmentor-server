CREATE PROCEDURE sp_Total_Ammount_of_Users
	@Total INT OUTPUT
AS
SELECT @Total = COUNT(*) FROM Users
