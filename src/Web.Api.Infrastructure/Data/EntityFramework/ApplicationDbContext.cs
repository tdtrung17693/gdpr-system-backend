using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Interfaces.Services;

namespace Web.Api.Infrastructure.Data.EntityFramework
{
  public class ApplicationDbContext : DbContext
  {
    private IHttpContextAccessor _httpContext;
    public ApplicationDbContext() { }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
    {
      _httpContext = httpContextAccessor;
    }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<CustomerServer> CustomerServer { get; set; }
        public virtual DbSet<EmailLog> EmailLog { get; set; }
        public virtual DbSet<FileInstance> FileInstance { get; set; }
        public virtual DbSet<HistoryLog> HistoryLog { get; set; }
        public virtual DbSet<Permission> Permission { get; set; }
        public virtual DbSet<PermissionRole> PermissionRole { get; set; }
        public virtual DbSet<Request> Request { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Server> Server { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserFileInstance> UserFileInstance { get; set; }
        public virtual DbSet<UserLog> UserLog { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        public DbQuery<SPRequestResultView> SPRequestResultView { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      foreach (var entityType in modelBuilder.Model.GetEntityTypes())
      {
        // Use the entity name instead of the Context.DbSet<T> name
        // refs https://docs.microsoft.com/en-us/ef/core/modeling/entity-types?tabs=fluent-api#table-name
        // modelBuilder.Entity(entityType.ClrType).ToTable(entityType.ClrType.Name);
      }

      modelBuilder.Entity<Account>(entity =>
      {
        entity.HasIndex(e => e.Username)
                  .HasName("UQ__Account__536C85E4D807EB86")
                  .IsUnique();

        entity.Property(e => e.Id).ValueGeneratedNever();

        entity.Property(e => e.HashedPassword)
                  .IsRequired()
                  .HasMaxLength(256);

        entity.Property(e => e.Salt)
                  .IsRequired()
                  .HasMaxLength(20);

        entity.Property(e => e.Username).HasMaxLength(20);

        entity.HasOne(d => d.User)
                  .WithOne(p => p.Account)
                  .HasConstraintName("fk_Account_userId");
      });

      modelBuilder.Entity<Comment>(entity =>
      {
        entity.Property(e => e.Id).ValueGeneratedNever();

        entity.Property(e => e.Content)
                  .IsRequired()
                  .HasMaxLength(300);

        entity.Property(e => e.CreatedAt)
                  .HasColumnType("datetime")
                  .HasDefaultValueSql("(getutcdate())");

        entity.Property(e => e.DeletedAt).HasColumnType("datetime");

        entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

        entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

        entity.HasOne(d => d.Author)
          .WithMany(p => p.CommentCreatedByNavigation)
          .HasForeignKey(d => d.CreatedBy)
          .HasConstraintName("fk_Comment_createdBy");

              // entity.HasOne(d => d.DeletedByNavigation)
              //     .WithMany(p => p.CommentDeletedByNavigation)
              //     .HasForeignKey(d => d.DeletedBy)
              //     .HasConstraintName("fk_Comment_deletedBy");

              entity.HasOne(d => d.Parent)
                  .WithMany(p => p.InverseParent)
                  .HasForeignKey(d => d.ParentId)
                  .HasConstraintName("fk_Comment_parentId");

        entity.HasOne(d => d.Request)
                  .WithMany(p => p.Comment)
                  .HasForeignKey(d => d.RequestId)
                  .HasConstraintName("fk_Comment_requestId");

              // entity.HasOne(d => d.UpdatedByNavigation)
              //     .WithMany(p => p.CommentUpdatedByNavigation)
              //     .HasForeignKey(d => d.UpdatedBy)
              //     .HasConstraintName("fk_Comment_updatedBy");
            });

      modelBuilder.Entity<Customer>(entity =>
      {
        entity.Property(e => e.Id).ValueGeneratedNever();

        entity.Property(e => e.ContractBeginDate).HasColumnType("date");

        entity.Property(e => e.ContractEndDate).HasColumnType("date");

        entity.Property(e => e.CreatedAt)
                  .HasColumnType("datetime")
                  .HasDefaultValueSql("(getutcdate())");

        entity.Property(e => e.Description).HasMaxLength(200);

        entity.Property(e => e.Name)
                  .IsRequired()
                  .HasMaxLength(50);

        entity.Property(e => e.Status).HasDefaultValueSql("((1))");

        entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        entity.HasOne(d => d.ContactPointNavigation)
             .WithMany(p => p.CustomerContactPointNavigation)
             .HasForeignKey(d => d.ContactPoint)
             .HasConstraintName("fk_Customer_contactPoint");

        // entity.HasOne(d => d.CreatedByNavigation)
        //     .WithMany(p => p.CustomerCreatedByNavigation)
        //     .HasForeignKey(d => d.CreatedBy)
        //     .HasConstraintName("fk_Customer_createdBy");

        // entity.HasOne(d => d.UpdatedByNavigation)
        //     .WithMany(p => p.CustomerUpdatedByNavigation)
        //     .HasForeignKey(d => d.UpdatedBy)
        //     .HasConstraintName("fk_Customer_updatedBy");
      });

      modelBuilder.Entity<CustomerServer>(entity =>
      {
        entity.HasKey(e => new { e.CustomerId, e.ServerId })
                  .HasName("PK__Customer__D8F8C856E0763940");

        entity.HasOne(d => d.Customer)
                  .WithMany(p => p.CustomerServer)
                  .HasForeignKey(d => d.CustomerId)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("fk_CustomerServer_CustomerId");

        entity.HasOne(d => d.Server)
                  .WithMany(p => p.CustomerServer)
                  .HasForeignKey(d => d.ServerId)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("fk_CustomerServer_ServerId");
      });

      modelBuilder.Entity<EmailLog>(entity =>
      {
        entity.Property(e => e.Id).ValueGeneratedNever();

        entity.Property(e => e.Content).HasMaxLength(300);

        entity.Property(e => e.CreatedAt)
                  .HasColumnType("datetime")
                  .HasDefaultValueSql("(getutcdate())");

        entity.Property(e => e.Receiver).HasMaxLength(256);

        entity.Property(e => e.Status).HasMaxLength(50);

        entity.Property(e => e.Subject).HasMaxLength(300);
      });

      modelBuilder.Entity<FileInstance>(entity =>
      {
        entity.Property(e => e.Id).ValueGeneratedNever();

        entity.Property(e => e.Extension)
                  .IsRequired()
                  .HasMaxLength(5);

        entity.Property(e => e.FileName)
                  .IsRequired()
                  .HasMaxLength(100);

        entity.Property(e => e.Path)
                  .IsRequired()
                  .HasMaxLength(200);
      });

      modelBuilder.Entity<HistoryLog>(entity =>
      {
        entity.Property(e => e.Id).ValueGeneratedNever();

        entity.Property(e => e.CreatedAt)
                  .HasColumnType("datetime")
                  .HasDefaultValueSql("(getutcdate())");

        entity.Property(e => e.Message)
                  .IsRequired()
                  .HasMaxLength(100);

        entity.Property(e => e.PreviousState)
                  .IsRequired()
                  .HasMaxLength(50);

        entity.Property(e => e.UpdatedField)
                  .IsRequired()
                  .HasMaxLength(50);

        entity.Property(e => e.UpdatedState)
                  .IsRequired()
                  .HasMaxLength(50);

        entity.HasOne(d => d.Request)
                  .WithMany(p => p.HistoryLog)
                  .HasForeignKey(d => d.RequestId)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("fk_HistoryLog_requestId");
      });

      modelBuilder.Entity<Permission>(entity =>
      {
        entity.Property(e => e.Id).ValueGeneratedNever();

        entity.Property(e => e.PermissionName).HasMaxLength(20);
      });

      modelBuilder.Entity<PermissionRole>(entity =>
      {
        entity.HasKey(e => new { e.RoleId, e.PermissionId })
                  .HasName("PK__Permissi__6400A1A8B1FCF168");

        entity.HasOne(d => d.Permission)
                  .WithMany(p => p.PermissionRole)
                  .HasForeignKey(d => d.PermissionId)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("fk_PermissionRole_permission");

        entity.HasOne(d => d.Role)
                  .WithMany(p => p.PermissionRole)
                  .HasForeignKey(d => d.RoleId)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("fk_PermissionRole_role");
      });

      modelBuilder.Entity<Request>(entity =>
      {
        entity.Property(e => e.Id).ValueGeneratedNever();

        entity.Property(e => e.CreatedAt)
                  .HasColumnType("datetime")
                  .HasDefaultValueSql("(getutcdate())");

        entity.Property(e => e.DeletedAt).HasColumnType("datetime");

        entity.Property(e => e.Description).HasMaxLength(100);

        entity.Property(e => e.EndDate).HasColumnType("datetime");

        entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

        entity.Property(e => e.Response).HasMaxLength(200);

        entity.Property(e => e.StartDate).HasColumnType("datetime");

        entity.Property(e => e.Status).HasMaxLength(50);
                entity.Property(e => e.RequestStatus).HasMaxLength(50);

        entity.Property(e => e.Title).HasMaxLength(50);

        entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

        entity.HasOne(d => d.ApprovedByNavigation)
            .WithMany(p => p.RequestApprovedByNavigation)
            .HasForeignKey(d => d.ApprovedBy)
            .HasConstraintName("fk_Request_approvedBy");

        entity.HasOne(d => d.CreatedByNavigation)
            .WithMany(p => p.RequestCreatedByNavigation)
            .HasForeignKey(d => d.CreatedBy)
            .HasConstraintName("fk_Request_createdBy");

        entity.HasOne(d => d.Server)
            .WithMany(p => p.Request)
            .HasForeignKey(d => d.ServerId)
            .HasConstraintName("fk_Request_serverId");

                // entity.HasOne(d => d.UpdatedByNavigation)
                //     .WithMany(p => p.RequestUpdatedByNavigation)
                //     .HasForeignKey(d => d.UpdatedBy)
                //     .HasConstraintName("fk_Request_updatedBy");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(20);
            });

            modelBuilder.Entity<Server>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.IpAddress)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email)
                    .HasName("UQ__User__A9D10534550C08B5")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                // entity.HasOne(d => d.CreatedByNavigation)
                //     .WithMany(p => p.InverseCreatedByNavigation)
                //     .HasForeignKey(d => d.CreatedBy)
                //     .OnDelete(DeleteBehavior.ClientSetNull)
                //     .HasConstraintName("fk_User_createdBy");

                // entity.HasOne(d => d.DeletedByNavigation)
                //     .WithMany(p => p.InverseDeletedByNavigation)
                //     .HasForeignKey(d => d.DeletedBy)
                //     .HasConstraintName("fk_User_deletedBy");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("fk_User_roleId");

                // entity.HasOne(d => d.UpdatedByNavigation)
                //     .WithMany(p => p.InverseUpdatedByNavigation)
                //     .HasForeignKey(d => d.UpdatedBy)
                //     .HasConstraintName("fk_User_updatedBy");
            });

            modelBuilder.Entity<UserFileInstance>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.FileInstanceId })
                    .HasName("PK__UserFile__6CBD4397C469411F");

                entity.HasOne(d => d.FileInstance)
                    .WithMany(p => p.UserFileInstance)
                    .HasForeignKey(d => d.FileInstanceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_UserFileInstance_FileInstanceId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserFileInstance)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_UserFileInstance_UserId");
            });

            modelBuilder.Entity<UserLog>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Behavior)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserLog)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_UserLog_userId");
            });
        }

        public override int SaveChanges()
        {
            AddAuitInfo();
            return base.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            AddAuitInfo();
            return await base.SaveChangesAsync();
        }

    private void AddAuitInfo()
    {
      var entries = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
      var authService = (IAuthService)_httpContext.HttpContext.RequestServices.GetService(typeof(IAuthService));
      var currentUser = authService.GetCurrentUser();

      foreach (var entry in entries)
      {
        if (entry.State == EntityState.Added)
        {
          ((BaseEntity)entry.Entity).CreatedAt = DateTime.UtcNow;
          ((BaseEntity)entry.Entity).CreatedBy = currentUser.Id;
        }
        ((BaseEntity)entry.Entity).UpdatedAt = DateTime.UtcNow;
        ((BaseEntity)entry.Entity).UpdatedBy = currentUser.Id;

        ((BaseEntity)entry.Entity).Status = ((BaseEntity)entry.Entity).Status == null ? false : ((BaseEntity)entry.Entity).Status;
      }
    }
  }
}
