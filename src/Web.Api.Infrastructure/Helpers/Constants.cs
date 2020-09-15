

using System.IO;

namespace Web.Api.Infrastructure.Helpers
{
    public static class Constants
    {
        public static class Strings
        {
            public static class JwtClaimIdentifiers
            {
                public const string Rol = "rol", Id = "id";

                public static string Username = "uname";
            }

            public static class JwtClaims
            {
                public const string ApiAccess = "api_access";
            }

        }
        public static class DefaultValues
        {
            public static class Paging
            {
                public const int PageSize = 10;
                public const int PageNo = 1;
            }

            public const string keyword = "";
            public const string filterStatus = "";
        }
    }
}
