using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class spCreateServer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"
            CREATE OR ALTER PROCEDURE CreateServer (
              @Name  NVARCHAR(150),
              @IpAddress NVARCHAR(150),
              @CreatedBy UNIQUEIDENTIFIER,
              @StartDate DATETIME,
              @EndDate DATETIME
            )
            AS
            BEGIN
              INSERT INTO dbo.[Server](Id, CreatedAt, CreatedBy, DeletedAt, DeletedBy, EndDate, IpAddress, IsDeleted,[Name], StartDate, [Status], UpdatedAt, UpdatedBy )
              VALUES (NEWID(), GETUTCDATE(), @CreatedBy,null,null, @EndDate, @IpAddress, 0, @Name, @StartDate, 1, null, null)
            END;
            ";


            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sp = @"drop procedure CreateServer;";
            migrationBuilder.Sql(sp);
        }
    }
}
