namespace FlatHunt.Server.Auth
{
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Broker = "Broker";

        public static readonly string[] All = { Admin, Broker };
    }
}