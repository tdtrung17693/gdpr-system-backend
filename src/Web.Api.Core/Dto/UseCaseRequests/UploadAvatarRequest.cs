using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class UploadAvatarRequest
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public byte[] Content { get; set; }
        public UploadAvatarRequest(Guid id, string fileName, string fileExtension, byte[] content)
        {
            Id = id;
            FileName = fileName;
            FileExtension = fileExtension;
            Content = content;
        }
    }
}
