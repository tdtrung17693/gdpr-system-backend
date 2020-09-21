using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class spGetRequestExport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(@"
CREATE OR ALTER PROC GetRequestExport
(
	@FromDate DATETIME,
	@ToDate DATETIME
)
AS
BEGIN
	 SELECT *
		INTO #TempRequest
		FROM [dbo].[Request] AS R

		SELECT R.Title, R.StartDate, R.EndDate, R.CreatedAt, UC.Email AS CreatedUserEmail, R.RequestStatus, R.UpdatedAt, 
				 UU.Email AS UpdatedUserEmail, S.IpAddress AS ServerIP, S.[Name] AS ServerName, AU.Email AS ApprovedUserEmail, 
				 R.Response, R.[Description]
					
		INTO #RequestInfo 
		FROM #TempRequest AS R
		JOIN [dbo].[Server] AS S ON R.ServerId = S.Id
		JOIN [dbo].[User] AS UC ON R.CreatedBy = UC.Id 
		LEFT JOIN [dbo].[User] AS UU ON R.UpdatedBy = UU.Id 
		LEFT JOIN [dbo].[User] AS AU ON R.ApprovedBy = AU.Id 
		SELECT * FROM #RequestInfo AS RI
        WHERE RI.CreatedAt BETWEEN @FromDate AND @ToDate

		SELECT * FROM #RequestInfo
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(@"drop proc GetRequestExport");
        }
    }
}
