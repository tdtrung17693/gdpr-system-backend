using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class spGetExportBulkRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(@"
CREATE OR ALTER    PROCEDURE [dbo].[GetExportBulkRequest]
(
@uId UNIQUEIDENTIFIER,
@IdList NVARCHAR(MAX)
)
AS
BEGIN 
		DECLARE @Role nvarchar(20)
		SET @Role=(SELECT ROL.[Name]
		FROM [User] as U JOIN [Role] AS ROL ON U.RoleId=ROL.Id
		WHERE @uId = U.Id)
		IF @Role <> 'Employee'
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
		END
		ELSE
		BEGIN
			SELECT *
				INTO #TempRequest1
				FROM [dbo].[Request] AS R
				WHERE R.Id IN (SELECT * FROM string_split(@IdList, ','))

				SELECT R.Title, R.StartDate, R.EndDate, R.CreatedAt, R.RequestStatus, R.UpdatedAt, 
						 S.IpAddress AS ServerIP, S.[Name] AS ServerName, AU.Email AS ApprovedUserEmail, 
						 R.Response, R.[Description]
					
				INTO #RequestInfo1 
				FROM #TempRequest1 AS R
				JOIN [dbo].[Server] AS S ON R.ServerId = S.Id
				JOIN [dbo].[User] AS UC ON R.CreatedBy = UC.Id 
				LEFT JOIN [dbo].[User] AS UU ON R.UpdatedBy = UU.Id 
				LEFT JOIN [dbo].[User] AS AU ON R.ApprovedBy = AU.Id 

				SELECT * FROM #RequestInfo

		END
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql("drop proc GetExportBulkRequest");
        }
    }
}
