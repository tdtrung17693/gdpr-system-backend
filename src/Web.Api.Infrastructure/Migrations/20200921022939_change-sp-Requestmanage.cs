using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class changespRequestmanage : Migration
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

    SELECT
        R.Title, R.RequestStatus, R.CreatedAt,
        CONCAT(U.FirstName, ' ', U.LastName) as ApproverFullName,
        U2.Email as RequesterEmail, CONCAT(U2.FirstName, ' ', U2.LastName) as RequesterFullName,
        S.Name
    from Request R
    join [User] U2 on U2.Id = R.CreatedBy
    join Server S on R.ServerId = S.Id
    where R.Id = @RequestId
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
