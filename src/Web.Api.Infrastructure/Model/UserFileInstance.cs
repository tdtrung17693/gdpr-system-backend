using System;
using System.Collections.Generic;

namespace Web.Api.Infrastructure.Model
{
    public partial class UserFileInstance
    {
        public Guid UserId { get; set; }
        public Guid FileInstanceId { get; set; }

        public virtual FileInstance FileInstance { get; set; }
        public virtual User User { get; set; }
    }
}
