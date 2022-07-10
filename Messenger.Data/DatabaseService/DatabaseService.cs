using Messenger.Domain.Constants;
using Npgsql;

namespace Messenger.Data.DatabaseService;

public static class DatabaseService
{
    public static string GetConnectionString()
    {
        var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
        
        if (databaseUrl == null)
        {
            return DatabaseConstants.ConnectionString;
        }
        
        var databaseUri = new Uri(databaseUrl);
        var userInfo = databaseUri.UserInfo.Split(':');

        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = databaseUri.Host,
            Port = databaseUri.Port,
            Username = userInfo[0],
            Password = userInfo[1],
            Database = databaseUri.LocalPath.TrimStart('/')
        };

        return builder.ToString();
    }
}