using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class spGetServerPaging : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(@"
CREATE OR ALTER PROC GetServerPaging
(
	@Page INT,
	@PageSize INT,
	@FilterBy NVARCHAR(100),
	@SortBy NVARCHAR(100),
	@SortOrder BIT
)
AS
BEGIN
	DECLARE @FilterStr NVARCHAR(MAX) = '%' + @FilterBy + '%';
	BEGIN
		SELECT
		[Server].Id,  [Server].CreatedBy,  [Server].UpdatedBy,
		[Server].[Name],  [Server].IpAddress, [Server].StartDate
		,[Server].EndDate,  [Server].[Status],  [Server].IsDeleted, CS.[Name] AS CusName
		FROM dbo.[Server] LEFT OUTER JOIN
		(SELECT Customer.[Name], CustomerServer.ServerId
			FROM dbo.Customer JOIN dbo.CustomerServer ON [Customer].Id = [CustomerServer].CustomerId
		) AS CS ON [Server].Id = CS.ServerId
		WHERE (
			[Server].[Name] LIKE @FilterStr OR
			[CS].[Name] LIKE @FilterStr OR
			[Server].IpAddress  LIKE @FilterStr
		) 
		ORDER BY
		CASE WHEN @SortOrder = 1 THEN 
			CASE WHEN @SortBy = 'ServerName' THEN [Server].[Name] 
				 WHEN @SortBy = 'CustomerName' THEN [CS].[Name]
				 WHEN @SortBy = 'Ip' THEN [Server].[IpAddress]
			END
		END ASC,
		CASE WHEN @SortOrder = 0 THEN 
			CASE WHEN @SortBy = 'ServerName' THEN [Server].[Name] 
				 WHEN @SortBy = 'CustomerName' THEN [CS].[Name]
				 WHEN @SortBy = 'Ip' THEN [Server].[IpAddress]
			END
		END DESC
			
		OFFSET (@Page * @PageSize) ROWS
		FETCH NEXT @PageSize ROWS ONLY;
	END	
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql("drop proc GetServerPaging");
        }
    }
}
