using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Core.Domain
{
    public static class PermissionConstants
    {
        public const string VIEW_USER = "user:view";
        public const string EDIT_USER = "user:edit";
        public const string CREATE_USER = "user:create";
        public const string DELETE_USER = "user:delete";

        public const string VIEW_REQUEST = "request:view";
        public const string EDIT_REQUEST = "request:edit";
        public const string CREATE_REQUEST = "request:create";
        public const string DELETE_REQUEST = "request:delete";

        public const string VIEW_CUSTOMER = "customer:view";
        public const string EDIT_CUSTOMER = "customer:edit";
        public const string CREATE_CUSTOMER = "customer:create";
        public const string DELETE_CUSTOMER = "customer:delete";

        public const string VIEW_SERVER = "server:view";
        public const string EDIT_SERVER = "server:edit";
        public const string CREATE_SERVER = "server:create";
        public const string DELETE_SERVER = "server:delete";
    }
}
