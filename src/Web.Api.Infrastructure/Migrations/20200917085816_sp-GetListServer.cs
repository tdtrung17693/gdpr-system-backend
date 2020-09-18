using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class spGetListServer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        var sql = @"
CREATE OR ALTER PROCEDURE getListServer
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
END";
	        migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql("drop procedure getListServer");
        }
    }
}
