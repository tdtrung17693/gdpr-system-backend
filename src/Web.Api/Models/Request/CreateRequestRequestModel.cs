using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Api.Models.Request
{
    public class CreateRequestRequestModel
    {
        public Guid CreatedBy { get; set; }
        ////public DateTime? CreatedAt { get; set; }
        //public Guid? UpdatedBy { get; set; }
        //public DateTime? UpdatedAt { get; set; }
        //public Guid? DeletedBy { get; set; }
        //public DateTime? DeletedAt { get; set; }
        public string Title { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid ServerId { get; set; }
        public string Description { get; set; }
    }
}
