using Microsoft.EntityFrameworkCore.Migrations;

namespace FlatHunt.Server.Migrations.Extensions
{
    public static class MigrationsExtensions
    {
        internal static MigrationBuilder SqlFile(this MigrationBuilder builder, string path)
        {
            var fullPath = GetScriptFileFullPath(path);
            if (!File.Exists(fullPath))
            {
                throw new ArgumentException($"Incorrect script file path: {fullPath}");
            }

            var script = File.ReadAllText(fullPath);
            builder.Sql(script);

            return builder;
        }

        private static string GetScriptFileFullPath(string path)
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(directory, path);
        }
    }
}
