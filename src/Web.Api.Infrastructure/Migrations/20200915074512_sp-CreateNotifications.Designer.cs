﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Web.Api.Infrastructure.Data.EntityFramework;

namespace Web.Api.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200915074512_sp-CreateNotifications")]
    partial class spCreateNotifications
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-preview2-35157")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Web.Api.Core.Domain.Entities.Account", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<byte[]>("HashedPassword")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<Guid>("UserId");

                    b.Property<string>("Username")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique()
                        .HasName("UQ__Account__536C85E4D807EB86")
                        .HasFilter("[Username] IS NOT NULL");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("Web.Api.Core.Domain.Entities.Comment", b =>
                {
                    b.Property<Guid?>("Id");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(300);

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getutcdate())");

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<bool?>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("((0))");

                    b.Property<Guid?>("ParentId");

                    b.Property<Guid>("RequestId");

                    b.Property<bool?>("Status");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime");

                    b.Property<Guid?>("UpdatedBy");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("ParentId");

                    b.HasIndex("RequestId");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("Web.Api.Core.Domain.Entities.Customer", b =>
                {
                    b.Property<Guid?>("Id");

                    b.Property<Guid?>("ContactPoint");

                    b.Property<DateTime?>("ContractBeginDate")
                        .HasColumnType("date");

                    b.Property<DateTime?>("ContractEndDate")
                        .HasColumnType("date");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getutcdate())");

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<string>("Description")
                        .HasMaxLength(200);

                    b.Property<bool?>("IsDeleted");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool?>("Status")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("((1))");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime");

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<Guid?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ContactPoint");

                    b.HasIndex("UserId");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("Web.Api.Core.Domain.Entities.CustomerServer", b =>
                {
                    b.Property<Guid>("CustomerId");

                    b.Property<Guid>("ServerId");

                    b.HasKey("CustomerId", "ServerId")
                        .HasName("PK__Customer__D8F8C856E0763940");

                    b.HasIndex("ServerId");

                    b.ToTable("CustomerServer");
                });

            modelBuilder.Entity("Web.Api.Core.Domain.Entities.EmailLog", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<string>("Content")
                        .HasMaxLength(300);

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getutcdate())");

                    b.Property<string>("Receiver")
                        .HasMaxLength(256);

                    b.Property<string>("Status")
                        .HasMaxLength(50);

                    b.Property<string>("Subject")
                        .HasMaxLength(300);

                    b.HasKey("Id");

                    b.ToTable("EmailLog");
                });

            modelBuilder.Entity("Web.Api.Core.Domain.Entities.FileInstance", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<string>("Extension")
                        .IsRequired()
                        .HasMaxLength(5);

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.ToTable("FileInstance");
                });

            modelBuilder.Entity("Web.Api.Core.Domain.Entities.HistoryLog", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getutcdate())");

                    b.Property<Guid>("CreatedBy");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("PreviousState")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<Guid>("RequestId");

                    b.Property<string>("UpdatedField")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("UpdatedState")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("RequestId");

                    b.ToTable("HistoryLog");
                });

            modelBuilder.Entity("Web.Api.Core.Domain.Entities.Notification", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CreatedAt");

                    b.Property<Guid?>("CreatedBy");

                    b.Property<string>("Data");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<Guid?>("FromUserId");

                    b.Property<bool?>("IsDeleted");

                    b.Property<bool>("IsRead");

                    b.Property<string>("NotificationType");

                    b.Property<bool?>("Status");

                    b.Property<Guid>("ToUserId");

                    b.Property<DateTime?>("UpdatedAt");

                    b.Property<Guid?>("UpdatedBy");

                    b.HasKey("Id");

                    b.HasIndex("FromUserId");

                    b.HasIndex("ToUserId");

                    b.ToTable("Notification");
                });

            modelBuilder.Entity("Web.Api.Core.Domain.Entities.Permission", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<string>("PermissionName")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.ToTable("Permission");
                });

            modelBuilder.Entity("Web.Api.Core.Domain.Entities.PermissionRole", b =>
                {
                    b.Property<Guid>("RoleId");

                    b.Property<Guid>("PermissionId");

                    b.HasKey("RoleId", "PermissionId")
                        .HasName("PK__Permissi__6400A1A8B1FCF168");

                    b.HasIndex("PermissionId");

                    b.ToTable("PermissionRole");
                });

            modelBuilder.Entity("Web.Api.Core.Domain.Entities.Request", b =>
                {
                    b.Property<Guid?>("Id");

                    b.Property<Guid?>("ApprovedBy");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getutcdate())");

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<string>("Description")
                        .HasMaxLength(100);

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime");

                    b.Property<bool?>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("((0))");

                    b.Property<string>("RequestStatus")
                        .HasMaxLength(50);

                    b.Property<string>("Response")
                        .HasMaxLength(200);

                    b.Property<Guid>("ServerId");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime");

                    b.Property<bool?>("Status")
                        .HasMaxLength(50);

                    b.Property<string>("Title")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime");

                    b.Property<Guid?>("UpdatedBy");

                    b.HasKey("Id");

                    b.HasIndex("ApprovedBy");

                    b.HasIndex("CreatedBy");

                    b.HasIndex("ServerId");

                    b.ToTable("Request");
                });

            modelBuilder.Entity("Web.Api.Core.Domain.Entities.Role", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<string>("Name")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("Web.Api.Core.Domain.Entities.Server", b =>
                {
                    b.Property<Guid?>("Id");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getutcdate())");

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("date");

                    b.Property<string>("IpAddress")
                        .IsRequired()
                        .HasMaxLength(15);

                    b.Property<bool?>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("((0))");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("date");

                    b.Property<bool?>("Status");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime");

                    b.Property<Guid?>("UpdatedBy");

                    b.HasKey("Id");

                    b.ToTable("Server");
                });

            modelBuilder.Entity("Web.Api.Core.Domain.Entities.User", b =>
                {
                    b.Property<Guid?>("Id");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getutcdate())");

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<string>("FirstName")
                        .HasMaxLength(50);

                    b.Property<bool?>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("((0))");

                    b.Property<string>("LastName")
                        .HasMaxLength(50);

                    b.Property<Guid>("RoleId");

                    b.Property<bool?>("Status");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime");

                    b.Property<Guid?>("UpdatedBy");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasName("UQ__User__A9D10534550C08B5")
                        .HasFilter("[Email] IS NOT NULL");

                    b.HasIndex("RoleId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Web.Api.Core.Domain.Entities.UserFileInstance", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("FileInstanceId");

                    b.HasKey("UserId", "FileInstanceId")
                        .HasName("PK__UserFile__6CBD4397C469411F");

                    b.HasAlternateKey("UserId");

                    b.HasIndex("FileInstanceId");

                    b.ToTable("UserFileInstance");
                });

            modelBuilder.Entity("Web.Api.Core.Domain.Entities.UserLog", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<string>("Behavior")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserLog");
                });

            modelBuilder.Entity("Web.Api.Core.Domain.Entities.Account", b =>
                {
                    b.HasOne("Web.Api.Core.Domain.Entities.User", "User")
                        .WithOne("Account")
                        .HasForeignKey("Web.Api.Core.Domain.Entities.Account", "UserId")
                        .HasConstraintName("fk_Account_userId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Web.Api.Core.Domain.Entities.Comment", b =>
                {
                    b.HasOne("Web.Api.Core.Domain.Entities.User", "Author")
                        .WithMany("CommentCreatedByNavigation")
                        .HasForeignKey("CreatedBy")
                        .HasConstraintName("fk_Comment_createdBy");

                    b.HasOne("Web.Api.Core.Domain.Entities.Comment", "Parent")
                        .WithMany("InverseParent")
                        .HasForeignKey("ParentId")
                        .HasConstraintName("fk_Comment_parentId");

                    b.HasOne("Web.Api.Core.Domain.Entities.Request", "Request")
                        .WithMany("Comment")
                        .HasForeignKey("RequestId")
                        .HasConstraintName("fk_Comment_requestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Web.Api.Core.Domain.Entities.Customer", b =>
                {
                    b.HasOne("Web.Api.Core.Domain.Entities.User", "ContactPointNavigation")
                        .WithMany("CustomerContactPointNavigation")
                        .HasForeignKey("ContactPoint")
                        .HasConstraintName("fk_Customer_contactPoint");

                    b.HasOne("Web.Api.Core.Domain.Entities.User")
                        .WithMany("Customers")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Web.Api.Core.Domain.Entities.CustomerServer", b =>
                {
                    b.HasOne("Web.Api.Core.Domain.Entities.Customer", "Customer")
                        .WithMany("CustomerServer")
                        .HasForeignKey("CustomerId")
                        .HasConstraintName("fk_CustomerServer_CustomerId");

                    b.HasOne("Web.Api.Core.Domain.Entities.Server", "Server")
                        .WithMany("CustomerServer")
                        .HasForeignKey("ServerId")
                        .HasConstraintName("fk_CustomerServer_ServerId");
                });

            modelBuilder.Entity("Web.Api.Core.Domain.Entities.HistoryLog", b =>
                {
                    b.HasOne("Web.Api.Core.Domain.Entities.Request", "Request")
                        .WithMany("HistoryLog")
                        .HasForeignKey("RequestId")
                        .HasConstraintName("fk_HistoryLog_requestId");
                });

            modelBuilder.Entity("Web.Api.Core.Domain.Entities.Notification", b =>
                {
                    b.HasOne("Web.Api.Core.Domain.Entities.User", "FromUser")
                        .WithMany()
                        .HasForeignKey("FromUserId");

                    b.HasOne("Web.Api.Core.Domain.Entities.User", "ToUser")
                        .WithMany()
                        .HasForeignKey("ToUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Web.Api.Core.Domain.Entities.PermissionRole", b =>
                {
                    b.HasOne("Web.Api.Core.Domain.Entities.Permission", "Permission")
                        .WithMany("PermissionRole")
                        .HasForeignKey("PermissionId")
                        .HasConstraintName("fk_PermissionRole_permission");

                    b.HasOne("Web.Api.Core.Domain.Entities.Role", "Role")
                        .WithMany("PermissionRole")
                        .HasForeignKey("RoleId")
                        .HasConstraintName("fk_PermissionRole_role");
                });

            modelBuilder.Entity("Web.Api.Core.Domain.Entities.Request", b =>
                {
                    b.HasOne("Web.Api.Core.Domain.Entities.User", "ApprovedByNavigation")
                        .WithMany("RequestApprovedByNavigation")
                        .HasForeignKey("ApprovedBy")
                        .HasConstraintName("fk_Request_approvedBy");

                    b.HasOne("Web.Api.Core.Domain.Entities.User", "CreatedByNavigation")
                        .WithMany("RequestCreatedByNavigation")
                        .HasForeignKey("CreatedBy")
                        .HasConstraintName("fk_Request_createdBy");

                    b.HasOne("Web.Api.Core.Domain.Entities.Server", "Server")
                        .WithMany("Request")
                        .HasForeignKey("ServerId")
                        .HasConstraintName("fk_Request_serverId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Web.Api.Core.Domain.Entities.User", b =>
                {
                    b.HasOne("Web.Api.Core.Domain.Entities.Role", "Role")
                        .WithMany("User")
                        .HasForeignKey("RoleId")
                        .HasConstraintName("fk_User_roleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Web.Api.Core.Domain.Entities.UserFileInstance", b =>
                {
                    b.HasOne("Web.Api.Core.Domain.Entities.FileInstance", "FileInstance")
                        .WithMany("UserFileInstance")
                        .HasForeignKey("FileInstanceId")
                        .HasConstraintName("fk_UserFileInstance_FileInstanceId");

                    b.HasOne("Web.Api.Core.Domain.Entities.User", "User")
                        .WithMany("UserFileInstance")
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_UserFileInstance_UserId");
                });

            modelBuilder.Entity("Web.Api.Core.Domain.Entities.UserLog", b =>
                {
                    b.HasOne("Web.Api.Core.Domain.Entities.User", "User")
                        .WithMany("UserLog")
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_UserLog_userId");
                });
#pragma warning restore 612, 618
        }
    }
}
