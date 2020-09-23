using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class changespGetRequestCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          migrationBuilder.Sql(@"CREATE OR ALTER    PROC [dbo].[GetRequestCount] (@SearchKey NVARCHAR(MAX))
AS
BEGIN
	SELECT *
			INTO #TempRequest
			FROM [dbo].[Request] AS R

			SELECT R.*, UC.FirstName AS CreatedByFName, UC.LastName AS CreatedByLName, UC.Email AS CreatedByEmail,
						UU.FirstName AS UpdatedByFName, UU.LastName AS UpdatedByLName, UU.Email AS UpdatedByEmail,
						S.IpAddress AS ServerIP, S.[Name] AS ServerName
			INTO #RequestInfo 
			FROM #TempRequest AS R
			JOIN [dbo].[Server] AS S ON R.ServerId = S.Id
			JOIN [dbo].[User] AS UC ON R.CreatedBy = UC.Id 
			LEFT JOIN [dbo].[User] AS UU ON R.UpdatedBy = UU.Id 
		
			SELECT COUNT(*) FROM #RequestInfo AS RI
			WHERE 
				(RI.Title LIKE '%' + @SearchKey + '%' OR
				RI.ServerIP LIKE '%' + @SearchKey + '%' OR
				RI.ServerName LIKE '%' + @SearchKey + '%' OR
				RI.CreatedByEmail LIKE '%' + @SearchKey + '%' OR
				RI.CreatedByFName LIKE '%' + @SearchKey + '%' OR
				RI.CreatedByLName LIKE '%' + @SearchKey + '%' OR
				RI.UpdatedByEmail LIKE '%' + @SearchKey + '%' OR
				RI.UpdatedByFName LIKE '%' + @SearchKey + '%' OR
				RI.UpdatedByLName LIKE '%' + @SearchKey + '%')

END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
