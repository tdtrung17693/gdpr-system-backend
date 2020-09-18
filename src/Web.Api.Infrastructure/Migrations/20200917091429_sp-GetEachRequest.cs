using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class spGetEachRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(@"
CREATE OR ALTER PROCEDURE GetEachRequest
(
	@IdRequest UNIQUEIDENTIFIER
)
AS
BEGIN
	SELECT *
	INTO #TempRequest
	FROM Request AS R
	WHERE R.Id = @IdRequest

	SELECT R.*, UC.FirstName AS CreatedByFName, UC.LastName AS CreatedByLName, UC.Email AS CreatedByEmail,
					UU.FirstName AS UpdatedByFName, UU.LastName AS UpdatedByLName, UU.Email AS UpdatedByEmail,
					S.IpAddress AS ServerIP, S.[Name] AS ServerName
		INTO #RequestInfo
		FROM #TempRequest AS R
		JOIN [dbo].[Server] AS S ON R.ServerId = S.Id
		JOIN [dbo].[User] AS UC ON R.CreatedBy = UC.Id 
		LEFT JOIN [dbo].[User] AS UU ON R.UpdatedBy = UU.Id 
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql("drop proc GetEachRequest");
        }
    }
}
