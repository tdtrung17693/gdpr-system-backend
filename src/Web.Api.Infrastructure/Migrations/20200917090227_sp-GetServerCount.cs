using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class spGetServerCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE OR ALTER PROC GetServerCount @FilterBy NVARCHAR(MAX)
AS
BEGIN
    DECLARE @FilterStr NVARCHAR(MAX) = '%' + @FilterBy + '%';
	SELECT Count([Server].Id) AS ServerCount
	FROM dbo.[Server] LEFT OUTER JOIN
		(
	    SELECT dbo.CustomerServer.ServerId,dbo.Customer.Name
	    FROM dbo.Customer JOIN dbo.CustomerServer ON [Customer].Id = [CustomerServer].CustomerId
		) AS CS ON [Server].Id = CS.ServerId
		WHERE (
			[Server].[Name] LIKE @FilterStr OR
			[CS].[Name] LIKE @FilterStr OR
			[Server].IpAddress  LIKE @FilterStr
		)
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("drop proc GetServerCount");
        }
    }
}
