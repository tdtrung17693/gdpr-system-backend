using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class spGetExportBulkRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(@"
CREATE OR ALTER  PROCEDURE [dbo].[GetExportBulkRequest]
(
@IdList NVARCHAR(MAX)
)
AS
BEGIN	
	
	SELECT *
		INTO #TempRequest
		FROM [dbo].[Request] AS R
		WHERE R.Id IN (SELECT * FROM string_split(@IdList, ','))

		SELECT R.Title, R.StartDate, R.EndDate, R.CreatedAt, UC.Email AS CreatedUserEmail, R.RequestStatus, R.UpdatedAt, 
				 UU.Email AS UpdatedUserEmail, S.IpAddress AS ServerIP, S.[Name] AS ServerName, AU.Email AS ApprovedUserEmail, 
				 R.Response, R.[Description]
					
		INTO #RequestInfo 
		FROM #TempRequest AS R
		JOIN [dbo].[Server] AS S ON R.ServerId = S.Id
		JOIN [dbo].[User] AS UC ON R.CreatedBy = UC.Id 
		LEFT JOIN [dbo].[User] AS UU ON R.UpdatedBy = UU.Id 
		LEFT JOIN [dbo].[User] AS AU ON R.ApprovedBy = AU.Id 

		SELECT * FROM #RequestInfo
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql("drop proc GetExportBulkRequest");
        }
    }
}
