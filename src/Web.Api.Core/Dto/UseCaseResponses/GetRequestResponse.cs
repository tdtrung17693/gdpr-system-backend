using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseResponses
{
    public class GetRequestResponse : UseCaseResponseMessage
    {
        public int NoPages { get; }
        public IList<RequestDetail> RequestList { get; }
        public IEnumerable<Error> Errors { get; }
        //public GetRequestResponse(IEnumerable<Error> errors, bool success = false, string message = null) : base(success, message)
        //{
        //    Errors = errors;
        //}

        public GetRequestResponse(int noPages, IList<RequestDetail> requestList, bool success = false, string message = null) : base(success, message)
        {
            NoPages = noPages;
            RequestList = requestList;
        }
    }
}
