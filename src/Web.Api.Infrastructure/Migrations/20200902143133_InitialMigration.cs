using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Api.Infrastructure.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Receiver = table.Column<string>(maxLength: 256, nullable: true),
                    Subject = table.Column<string>(maxLength: 300, nullable: true),
                    Content = table.Column<string>(maxLength: 300, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getutcdate())"),
                    Status = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileInstance",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FileName = table.Column<string>(maxLength: 20, nullable: false),
                    Extension = table.Column<string>(maxLength: 5, nullable: false),
                    Path = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileInstance", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PermissionName = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionRole",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(nullable: false),
                    PermissionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Permissi__6400A1A8B1FCF168", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "fk_PermissionRole_permission",
                        column: x => x.PermissionId,
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_PermissionRole_role",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getutcdate())"),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<Guid>(nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<Guid>(nullable: true),
                    Status = table.Column<bool>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true, defaultValueSql: "((0))"),
                    FirstName = table.Column<string>(maxLength: 50, nullable: true),
                    LastName = table.Column<string>(maxLength: 50, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "fk_User_createdBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_User_deletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_User_roleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_User_updatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Username = table.Column<string>(maxLength: 20, nullable: true),
                    HashedPassword = table.Column<byte[]>(maxLength: 256, nullable: false),
                    Salt = table.Column<string>(maxLength: 20, nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                    table.ForeignKey(
                        name: "fk_Account_userId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getutcdate())"),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<Guid>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<Guid>(nullable: true),
                    Status = table.Column<bool>(nullable: true, defaultValueSql: "((1))"),
                    IsDeleted = table.Column<bool>(nullable: true),
                    DeletedByNavigationId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    ContractBeginDate = table.Column<DateTime>(type: "date", nullable: true),
                    ContractEndDate = table.Column<DateTime>(type: "date", nullable: true),
                    ContactPoint = table.Column<Guid>(nullable: true),
                    Description = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                    table.ForeignKey(
                        name: "fk_Customer_createdBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customer_User_DeletedByNavigationId",
                        column: x => x.DeletedByNavigationId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_Customer_updatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Server",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getutcdate())"),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<Guid>(nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<Guid>(nullable: true),
                    Status = table.Column<bool>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true, defaultValueSql: "((0))"),
                    //CreatedByNavigationId = table.Column<Guid>(nullable: true),
                    //DeletedByNavigationId = table.Column<Guid>(nullable: true),
                    //UpdatedByNavigationId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(maxLength: 150, nullable: false),
                    IpAddress = table.Column<string>(maxLength: 15, nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: true),
                    EndDate = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Server", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Server_User_CreatedByNavigationId",
                        column: x => x.CreatedByNavigationId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Server_User_DeletedByNavigationId",
                        column: x => x.DeletedByNavigationId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Server_User_UpdatedByNavigationId",
                        column: x => x.UpdatedByNavigationId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserFileInstance",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    FileInstanceId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserFile__6CBD4397C469411F", x => new { x.UserId, x.FileInstanceId });
                    table.UniqueConstraint("AK_UserFileInstance_UserId", x => x.UserId);
                    table.ForeignKey(
                        name: "fk_UserFileInstance_FileInstanceId",
                        column: x => x.FileInstanceId,
                        principalTable: "FileInstance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_UserFileInstance_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Behavior = table.Column<string>(maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLog", x => x.Id);
                    table.ForeignKey(
                        name: "fk_UserLog_userId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerServer",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(nullable: false),
                    ServerId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Customer__D8F8C856E0763940", x => new { x.CustomerId, x.ServerId });
                    table.ForeignKey(
                        name: "fk_CustomerServer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_CustomerServer_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Server",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Request",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getutcdate())"),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<Guid>(nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<Guid>(nullable: true),
                    Status = table.Column<bool>(maxLength: 50, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true, defaultValueSql: "((0))"),
                    RequestStatus = table.Column<string>(nullable: true),
                    Title = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 100, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ServerId = table.Column<Guid>(nullable: true),
                    Response = table.Column<string>(maxLength: 200, nullable: true),
                    ApprovedBy = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Request", x => x.Id);
                    table.ForeignKey(
                        name: "fk_Request_approvedBy",
                        column: x => x.ApprovedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_Request_createdBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_Request_deletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_Request_serverId",
                        column: x => x.ServerId,
                        principalTable: "Server",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_Request_updatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getutcdate())"),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<Guid>(nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<Guid>(nullable: true),
                    Status = table.Column<bool>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true, defaultValueSql: "((0))"),
                    RequestId = table.Column<Guid>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: true),
                    Content = table.Column<string>(maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Id);
                    table.ForeignKey(
                        name: "fk_Comment_createdBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_Comment_deletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_Comment_parentId",
                        column: x => x.ParentId,
                        principalTable: "Comment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_Comment_requestId",
                        column: x => x.RequestId,
                        principalTable: "Request",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_Comment_updatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HistoryLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RequestId = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getutcdate())"),
                    CreatedBy = table.Column<Guid>(nullable: false),
                    UpdatedField = table.Column<string>(maxLength: 50, nullable: false),
                    UpdatedState = table.Column<string>(maxLength: 50, nullable: false),
                    PreviousState = table.Column<string>(maxLength: 50, nullable: false),
                    Message = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryLog", x => x.Id);
                    table.ForeignKey(
                        name: "fk_HistoryLog_requestId",
                        column: x => x.RequestId,
                        principalTable: "Request",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_UserId",
                table: "Account",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Account__536C85E4D807EB86",
                table: "Account",
                column: "Username",
                unique: true,
                filter: "[Username] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_CreatedBy",
                table: "Comment",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_DeletedBy",
                table: "Comment",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ParentId",
                table: "Comment",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_RequestId",
                table: "Comment",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_UpdatedBy",
                table: "Comment",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CreatedBy",
                table: "Customer",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_DeletedByNavigationId",
                table: "Customer",
                column: "DeletedByNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_UpdatedBy",
                table: "Customer",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerServer_ServerId",
                table: "CustomerServer",
                column: "ServerId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryLog_RequestId",
                table: "HistoryLog",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionRole_PermissionId",
                table: "PermissionRole",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Request_ApprovedBy",
                table: "Request",
                column: "ApprovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Request_CreatedBy",
                table: "Request",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Request_DeletedBy",
                table: "Request",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Request_ServerId",
                table: "Request",
                column: "ServerId");

            migrationBuilder.CreateIndex(
                name: "IX_Request_UpdatedBy",
                table: "Request",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Server_CreatedByNavigationId",
                table: "Server",
                column: "CreatedByNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_Server_DeletedByNavigationId",
                table: "Server",
                column: "DeletedByNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_Server_UpdatedByNavigationId",
                table: "Server",
                column: "UpdatedByNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_User_CreatedBy",
                table: "User",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_User_DeletedBy",
                table: "User",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "UQ__User__A9D10534550C08B5",
                table: "User",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleId",
                table: "User",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_User_UpdatedBy",
                table: "User",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserFileInstance_FileInstanceId",
                table: "UserFileInstance",
                column: "FileInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLog_UserId",
                table: "UserLog",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "CustomerServer");

            migrationBuilder.DropTable(
                name: "EmailLog");

            migrationBuilder.DropTable(
                name: "HistoryLog");

            migrationBuilder.DropTable(
                name: "PermissionRole");

            migrationBuilder.DropTable(
                name: "UserFileInstance");

            migrationBuilder.DropTable(
                name: "UserLog");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Request");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "FileInstance");

            migrationBuilder.DropTable(
                name: "Server");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
