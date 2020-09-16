using System;

namespace Web.Api.Models.Request.Comment
{
    public class CreateCommentRequest
    {
        public string RequestId { get; set; }
        public string Content { get; set; }
        public Guid? ParentId { get; set; }
    }
}