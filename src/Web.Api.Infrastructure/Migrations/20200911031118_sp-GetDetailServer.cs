using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class spGetDetailServer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"
            CREATE OR ALTER PROCEDURE GetDetailServer (
              @Id UNIQUEIDENTIFIER
            )
            AS 
            BEGIN
              SELECT [Name], IpAddress, StartDate, EndDate, [Status]
              FROM [dbo].[Server]
              WHERE [Server].Id = @Id
            END; 
            ";
            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sp = "drop procedure GetDetailServer";
            migrationBuilder.Sql(sp);
        }
    }
}
