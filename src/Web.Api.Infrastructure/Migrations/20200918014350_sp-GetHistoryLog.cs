using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class spGetHistoryLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE OR ALTER PROC GetHistoryLog @requestId UNIQUEIDENTIFIER
            AS
            BEGIN
                SELECT CreatedAt, CreatedBy, UpdatedField, UpdatedState, PreviousState, [Message]
                FROM dbo.HistoryLog
                WHERE RequestId = @requestId
            END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("drop proc GetHistoryLog");
        }
    }
}
