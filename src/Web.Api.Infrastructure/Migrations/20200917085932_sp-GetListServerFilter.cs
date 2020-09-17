using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class spGetListServerFilter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(@"CREATE OR ALTER PROCEDURE getListServerFilter ( @filterKey NVARCHAR(150))
AS
BEGIN
	SELECT
	[Server].Id,  [Server].CreatedBy,  [Server].UpdatedBy,
	[Server].[Name],  [Server].IpAddress, [Server].StartDate
	,  [Server].EndDate,  [Server].[Status],  [Server].IsDeleted, CS.[Name] AS CusName
	FROM dbo.[Server] LEFT OUTER JOIN
	(SELECT Customer.[Name], CustomerServer.ServerId
	FROM dbo.Customer JOIN dbo.CustomerServer ON [Customer].Id = [CustomerServer].CustomerId
	) AS CS ON [Server].Id = CS.ServerId
        WHERE [Server].IpAddress LIKE CONCAT('%',@filterKey,'%') OR [CS].[Name] LIKE CONCAT('%',@filterKey,'%')
	 OR [Server].[Name] LIKE CONCAT('%',@filterKey,'%')
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql("drop proc getListServerFilter");
        }
    }
}
