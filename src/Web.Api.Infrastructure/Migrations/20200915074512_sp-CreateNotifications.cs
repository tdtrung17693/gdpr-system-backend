using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class spCreateNotifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = @"
            CREATE TYPE [dbo].[NewNotifications] AS TABLE(
                [Data] nvarchar(max) NULL,
                [FromUserId] uniqueidentifier NULL,
                [ToUserId] uniqueidentifier,
                [NotificationType] nvarchar(max)
            )
            ";
            migrationBuilder.Sql(sql);
            sql = @"
            create or alter proc CreateNotifications
            (
            @Notifications NewNotifications readonly
            )
            as
                begin
                    DECLARE @output TABLE (id uniqueidentifier)
                    insert into Notification(Id, CreatedAt, FromUserId, ToUserId, NotificationType, Data, IsRead)
                        output inserted.Id into @output
                        select newid(), getutcdate(), FromUserId, ToUserId, NotificationType, Data, 0 from @Notifications;

                    select
                         noti.Id, noti.CreatedAt, noti.FromUserId, noti.ToUserId, noti.NotificationType, noti.Data,
                         fromuseraccount.Username as FromUserName, fromuser.Id as FromUserId,
                         touseraccount.Username as ToUserName, touser.Id as ToUserId
                    from Notification noti
                    left join [dbo].[User] fromuser on noti.FromUserId = fromuser.Id
                    join [dbo].[Account] fromuseraccount on fromuser.Id = fromuseraccount.UserId
                    join [dbo].[User] touser on noti.ToUserId = touser.Id
                    join [dbo].[Account] touseraccount on touser.Id = touseraccount.UserId
                    inner join @output o on o.id = noti.Id;
                end
            ";

            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = @"
            drop proc CreateNotifications;
            ";
            migrationBuilder.Sql(sql);

             sql = @"
            drop type [dbo].[NewNotifications];
            ";
            migrationBuilder.Sql(sql);
        }
    }
}
