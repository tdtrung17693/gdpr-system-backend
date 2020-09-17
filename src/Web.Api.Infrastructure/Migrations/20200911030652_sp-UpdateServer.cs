using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class spUpdateServer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"
              CREATE OR ALTER PROCEDURE UpdateServer (
                @Id UNIQUEIDENTIFIER,
                @IdUpdateBy UNIQUEIDENTIFIER,
                @Name  NVARCHAR(150),
                @IpAddress NVARCHAR(150),
                @StartDate DATETIME = NULL,
                @EndDate DATETIME = NULL,
                @Status BIT
              )
              AS
              BEGIN
                UPDATE dbo.[Server]
                SET	[Name] = @Name, [UpdatedAt] = GETUTCDATE(), [UpdatedBy] = @IdUpdateBy, [Status] = @Status, [IpAddress] = @IpAddress, [StartDate] = @StartDate, [EndDate] = @EndDate
                WHERE Id = @Id;
              END;
              ";
            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sp = "drop procedure UpdateServer";
            migrationBuilder.Sql(sp);
        }
    }
}
