using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class spRequestManage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql(@"
CREATE OR ALTER PROCEDURE RequestManage
(
	@RequestId UNIQUEIDENTIFIER,
	@UserId UNIQUEIDENTIFIER,
	@Response NVARCHAR(500),
	@Status NVARCHAR(6)
)
AS
BEGIN
	IF @Status = 'Open'
	BEGIN
		UPDATE Request
		SET UpdatedAt = GETDATE(), UpdatedBy = @UserId, Response = @Response, RequestStatus = @Status, ApprovedBy = @UserId
		WHERE Id = @RequestId
	END
	ELSE
	BEGIN
		UPDATE Request
		SET UpdatedAt = GETDATE(), UpdatedBy = @UserId, Response = @Response, RequestStatus = @Status
		WHERE Id = @RequestId
	END


	--IF @Status = 3
	--BEGIN
	--	INSERT INTO HistoryLog
	--	VALUES (NEWID(), GETDATE(), @UserId, @RequestId, 'Approved')
	--END
	--IF @Status = 4
	--BEGIN
	--	INSERT INTO HistoryLog
	--	VALUES (NEWID(), GETDATE(), @UserId, @RequestId, 'Closed')
	--END
	--IF @Status = 5
	--BEGIN
	--	INSERT INTO HistoryLog
	--	VALUES (NEWID(), GETDATE(), @UserId, @RequestId, 'Declined')
	--END

END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.Sql("drop proc RequestManage");
        }
    }
}
