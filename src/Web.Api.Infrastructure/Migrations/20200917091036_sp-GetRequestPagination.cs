using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class spGetRequestPagination : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(@"
CREATE OR ALTER PROCEDURE GetRequestPagination
(
  @SearchKey NVARCHAR(MAX) = '',
  @PageNo INT = 1,
  @PageSize INT = 10,
  @SortColumn NVARCHAR(20) = 'CreatedDate',
  @SortOrder NVARCHAR(4) = 'DESC',
  @FilterStatusString NVARCHAR(6) = ''
  --@FromDate DATETIME = 'Jan 01 1800 00:00AM',
  --@ToDate DATETIME = 'Jan 01 2800 00:00AM'
)
AS
BEGIN
		--IF (@FromDate IS NULL OR @ToDate IS NULL)
		--	BEGIN
		--		SET @FromDate = 'Jan 01 1800 00:00AM'
		--		SET @ToDate = 'Jan 01 2800 00:00AM'
		--	END
		--SET @PageSize = (SELECT COUNT(Id) FROM Request) 
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
		
		SELECT * FROM #RequestInfo AS RI
        WHERE (@FilterStatusString = (CASE @FilterStatusString WHEN '' THEN '' ELSE RI.RequestStatus END)) AND
			(RI.Title LIKE '%' + @SearchKey + '%' OR
			RI.ServerIP LIKE '%' + @SearchKey + '%' OR
			RI.ServerName LIKE '%' + @SearchKey + '%' OR
			RI.CreatedByEmail LIKE '%' + @SearchKey + '%' OR
			RI.CreatedByFName LIKE '%' + @SearchKey + '%' OR
			RI.CreatedByLName LIKE '%' + @SearchKey + '%' OR
			RI.UpdatedByEmail LIKE '%' + @SearchKey + '%' OR
			RI.UpdatedByFName LIKE '%' + @SearchKey + '%' OR
			RI.UpdatedByLName LIKE '%' + @SearchKey + '%') --AND
			--(RI.CreatedAt BETWEEN @FromDate AND @ToDate)
  
        ORDER BY  
			CASE @SortOrder
				WHEN 'ASC' THEN 
					CASE @SortColumn
						WHEN 'RequestTitle' THEN Title
						WHEN 'CreatedDate' THEN CreatedAt
					END
			END ASC,
			CASE @SortOrder
				WHEN 'DESC' THEN 
					CASE @SortColumn
						WHEN 'RequestTitle' THEN Title
						WHEN 'CreatedDate' THEN CreatedAt
					END
			END DESC

        OFFSET @PageSize * (@PageNo - 1) ROWS  
        FETCH NEXT @PageSize ROWS ONLY
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql("drop proc GetRequestPagination");
        }
    }
}
