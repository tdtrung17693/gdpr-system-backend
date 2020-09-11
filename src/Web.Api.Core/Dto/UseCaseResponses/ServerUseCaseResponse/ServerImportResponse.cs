using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Core.Dto.UseCaseResponses.ServerUseCaseResponse
{
    public class ServerImportResponse<T>
    {
        public int Code { get; set; }

        public string Msg { get; set; }

        public T Data { get; set; }

        public static ServerImportResponse<T> GetResult(int code, string msg, T data = default(T))
        {
            return new ServerImportResponse<T>
            {
                Code = code,
                Msg = msg,
                Data = data
            };
        }
    }
}
