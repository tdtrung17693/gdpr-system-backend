using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class changespRequestManage2 : Migration
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
  declare @OldStatus nvarchar(max);
  select @OldStatus = RequestStatus from Request where Id = @RequestId;

	IF @Status = 'Open'
	BEGIN
		UPDATE Request
		SET UpdatedAt = GETUTCDATE(), UpdatedBy = @UserId, Response = @Response, RequestStatus = @Status, ApprovedBy = @UserId
		WHERE Id = @RequestId
	END
	ELSE
	BEGIN
		UPDATE Request
		SET UpdatedAt = GETUTCDATE(), UpdatedBy = @UserId, Response = @Response, RequestStatus = @Status
		WHERE Id = @RequestId
	END

    SELECT
        R.Title, R.RequestStatus, R.CreatedAt, @OldStatus as OldStatus,
        CONCAT(U.FirstName, ' ', U.LastName) as ApproverFullName, A.Username as ApproverUsername,
        U2.Id as RequesterId, U2.Email as RequesterEmail, CONCAT(U2.FirstName, ' ', U2.LastName) as RequesterFullName, A2.Username as RequesterUsername,
        S.Name
    from Request R
    join [User] U on U.Id = R.UpdatedBy
    join Account A on U.Id = A.UserId
    join [User] U2 on U2.Id = R.CreatedBy
    join Account A2 on U2.Id = A2.UserId
    join Server S on R.ServerId = S.Id
    where R.Id = @RequestId
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
