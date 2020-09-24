using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class spUpdateMultiServerStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = "CREATE TYPE IdList AS TABLE(Id UNIQUEIDENTIFIER)";
            migrationBuilder.Sql(sp);

            sp = @"
            CREATE OR ALTER PROCEDURE UpdateMutilServerStatus(
				@ServerIds IdList READONLY,
				@Updator UNIQUEIDENTIFIER
			)
			AS
			BEGIN
				UPDATE [dbo].[Server]
				SET
					[Status] = ~[Status],
					UpdatedAt = GETUTCDATE(),
					UpdatedBy = @Updator
				WHERE Id IN (SELECT Id FROM @ServerIds)
			END;
            ";

            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sp = "drop procedure UpdateMutilServerStatus";
            migrationBuilder.Sql(sp);
            sp = "drop type IdList";
            migrationBuilder.Sql(sp);

        }
    }
}
