using System.Collections.Generic;

namespace AspNetCoreStarterKit.Domain.StaticData.Authorization
{
    public static class Permissions
    {
        public static List<string> GetAll()
        {
            return new List<string>
            {
                Users.Read, Users.Create, Users.Update, Users.Delete,
                Roles.Read, Roles.Create, Roles.Update, Roles.Delete
            };
        }

        public static class Users
        {
            public const string Read = "Permissions.Users.Read";
            public const string Create = "Permissions.Users.Create";
            public const string Update = "Permissions.Users.Update";
            public const string Delete = "Permissions.Users.Delete";
        }

        public static class Roles
        {
            public const string Read = "Permissions.Roles.Read";
            public const string Create = "Permissions.Roles.Create";
            public const string Update = "Permissions.Roles.Update";
            public const string Delete = "Permissions.Roles.Delete";
        }
    }
}
