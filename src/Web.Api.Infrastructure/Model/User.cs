using System;
using System.Collections.Generic;

namespace Web.Api.Infrastructure.Model
{
    public partial class User
    {
        public User()
        {
            Account = new HashSet<Account>();
            CommentCreatedByNavigation = new HashSet<Comment>();
            CommentDeletedByNavigation = new HashSet<Comment>();
            CommentUpdatedByNavigation = new HashSet<Comment>();
            CustomerCreatedByNavigation = new HashSet<Customer>();
            CustomerUpdatedByNavigation = new HashSet<Customer>();
            InverseCreatedByNavigation = new HashSet<User>();
            InverseDeletedByNavigation = new HashSet<User>();
            InverseUpdatedByNavigation = new HashSet<User>();
            RequestApprovedByNavigation = new HashSet<Request>();
            RequestCreatedByNavigation = new HashSet<Request>();
            RequestDeletedByNavigation = new HashSet<Request>();
            RequestUpdatedByNavigation = new HashSet<Request>();
            UserFileInstance = new HashSet<UserFileInstance>();
            UserLog = new HashSet<UserLog>();
        }

        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Guid? RoleId { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual User DeletedByNavigation { get; set; }
        public virtual Role Role { get; set; }
        public virtual User UpdatedByNavigation { get; set; }
        public virtual ICollection<Account> Account { get; set; }
        public virtual ICollection<Comment> CommentCreatedByNavigation { get; set; }
        public virtual ICollection<Comment> CommentDeletedByNavigation { get; set; }
        public virtual ICollection<Comment> CommentUpdatedByNavigation { get; set; }
        public virtual ICollection<Customer> CustomerCreatedByNavigation { get; set; }
        public virtual ICollection<Customer> CustomerUpdatedByNavigation { get; set; }
        public virtual ICollection<User> InverseCreatedByNavigation { get; set; }
        public virtual ICollection<User> InverseDeletedByNavigation { get; set; }
        public virtual ICollection<User> InverseUpdatedByNavigation { get; set; }
        public virtual ICollection<Request> RequestApprovedByNavigation { get; set; }
        public virtual ICollection<Request> RequestCreatedByNavigation { get; set; }
        public virtual ICollection<Request> RequestDeletedByNavigation { get; set; }
        public virtual ICollection<Request> RequestUpdatedByNavigation { get; set; }
        public virtual ICollection<UserFileInstance> UserFileInstance { get; set; }
        public virtual ICollection<UserLog> UserLog { get; set; }
    }
}
