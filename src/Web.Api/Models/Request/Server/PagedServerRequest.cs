using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Api.Models.Request
{
    public class PagedServerRequest
    {
        public PagedServerRequest()
        {
            page = 1;
            pageSize = 10;
            filterBy = "";
            sortedBy = "ServerName";
            sortOrder = true;
        }
        public int page { get; set; }
        public int pageSize { get; set; }
        public string filterBy { get; set; }
        public string sortedBy { get; set; }
        public bool sortOrder { get; set; }
    }
}
