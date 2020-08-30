using System;
using System.Collections.Generic;

namespace Web.Api.Infrastructure.Model
{
    public partial class FileInstance
    {
        public FileInstance()
        {
            UserFileInstance = new HashSet<UserFileInstance>();
        }

        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string Path { get; set; }

        public virtual ICollection<UserFileInstance> UserFileInstance { get; set; }
    }
}
