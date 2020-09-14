 

namespace Web.Api.Core.Dto
{
    public sealed class Error
    {
        public string Code { get; }
        public string Description { get; }

        public Error(string code, string description)
        {
            Code = code;
            Description = description;
        }
        public static class Codes
        {
            public const string ENTITY_NOT_FOUND = "entity_not_found";
            public const string UNKNOWN = "unknown_error";
            public const string UNIQUE_CONSTRAINT_VIOLATED = "unique_constraint_error";
            public const string UNAUTHORIZED_ACCESS = "unauthorized_access";
        }

        public static class Messages
        {
            public const string ENTITY_NOT_FOUND = "Entity not found.";
            public const string UNKNOWN = "Unexpected error happened.";
            public const string UNIQUE_CONSTRAINT_VIOLATED = "Unique constraint violated";
            public const string UNAUTHORIZED_ACCESS = "Unauthorized access";
        }
    }
}
