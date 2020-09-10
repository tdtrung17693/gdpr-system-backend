using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Domain.Entities
{
  public partial class User : BaseEntity
  {
    public User(
        string firstName, string lastName, string email, Guid roleId,
        Guid? id = null, Guid? createdBy = null, DateTime? createdAt = null, Guid? updatedBy = null, DateTime? updatedAt = null, Guid? deletedBy = null, DateTime? deletedAt = null, bool? isDeleted = false, bool? status = true)
        : base(id, createdAt, createdBy, updatedAt, updatedBy, deletedAt, deletedBy, isDeleted, status)
    {
      FirstName = firstName;
      LastName = lastName;
      Email = email;
      RoleId = roleId;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public Guid RoleId { get; set; }

    public virtual Role Role { get; set; }
    public virtual Account Account { get; set; }

    public IEnumerable<string> GetPermissions()
    {
      List<string> permissions = new List<string>();

      foreach (var pr in this.Role.PermissionRole)
      {
        permissions.Add(pr.Permission.PermissionName);
      }
      return permissions;
    }

    public virtual ICollection<Customer> Customers { get; set; }

    // public virtual ICollection<Comment> CommentCreatedByNavigation { get; set; }
    // public virtual ICollection<Comment> CommentDeletedByNavigation { get; set; }
    // public virtual ICollection<Comment> CommentUpdatedByNavigation { get; set; }
    // public virtual ICollection<Customer> CustomerCreatedByNavigation { get; set; }
    // public virtual ICollection<Customer> CustomerUpdatedByNavigation { get; set; }
    // public virtual ICollection<User> InverseCreatedByNavigation { get; set; }
    // public virtual ICollection<User> InverseDeletedByNavigation { get; set; }
    // public virtual ICollection<User> InverseUpdatedByNavigation { get; set; }
    // public virtual ICollection<Request> RequestApprovedByNavigation { get; set; }
    // public virtual ICollection<Request> RequestCreatedByNavigation { get; set; }
    // public virtual ICollection<Request> RequestDeletedByNavigation { get; set; }
    // public virtual ICollection<Request> RequestUpdatedByNavigation { get; set; }
    public virtual ICollection<UserFileInstance> UserFileInstance { get; set; }
    public virtual ICollection<UserLog> UserLog { get; set; }


    public bool IsContactPoint()
    {
      return Customers.Count > 0;
    }

    public static IEnumerable<string> GetSortableFields()
    {
      return new List<string> { "Username", "FirstName", "LastName", "Email" };
    }

    public static string GetDefaultSortingField()
    {
      return "FirstName";
    }

    public static IEnumerable<string> GetFilterableFields()
    {
      return new List<string> { "FirstName", "LastName", "Email", "Status", "RoleId" };
    }
  }
}
