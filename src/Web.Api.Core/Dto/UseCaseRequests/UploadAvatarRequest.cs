using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Core.Dto.UseCaseRequests { 
    public class UploadAvatarRequest
    {
        public Guid UserId { get; set; }
        public Guid? FileId { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string Content { get; set; }
        public UploadAvatarRequest(Guid userId, string fileName, string fileExtension, string content, Guid? fileId)
        {
            UserId = userId;
            FileName = fileName;
            FileExtension = fileExtension;
            Content = content;
            FileId = fileId;
        }
    }
}
