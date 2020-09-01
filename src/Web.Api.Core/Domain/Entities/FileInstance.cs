using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Api.Core.Domain.Entities
{
    public partial class FileInstance
    {
        public FileInstance(Guid id, string fileName, string extension, string path)
        {
            Id = id;
            FileName = fileName;
            Extension = extension;
            Path = path;
        }
        [Key]
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string Path { get; set; }
        public virtual ICollection<UserFileInstance> UserFileInstance { get; set; }
    }
}
