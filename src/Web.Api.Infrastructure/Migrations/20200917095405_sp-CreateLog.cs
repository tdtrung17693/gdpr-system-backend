using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class spCreateLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE OR ALTER PROC CreateLog (
    @RequestId UNIQUEIDENTIFIER,
    @UpdatedField NVARCHAR(150),
    @UpdatedState NVARCHAR(150),
    @PreviousState NVARCHAR(150),
    @Message NVARCHAR(150),
    @CreatedBy NVARCHAR(150)
)
AS
BEGIN
    INSERT INTO dbo.[HistoryLog](Id, RequestId, CreatedAt, CreatedBy, UpdatedField, UpdatedState, PreviousState, [Message])
    VALUES (NEWID(), @RequestId, GETUTCDATE(), @CreatedBy, @UpdatedField, @UpdatedState, @PreviousState, @Message)
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("drop proc CreateLog");
        }
    }
}
