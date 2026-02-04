using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace UserCrud.EntityFrameworkCore;

public static class UserCrudDbContextConfigurer
{
    public static void Configure(DbContextOptionsBuilder<UserCrudDbContext> builder, string connectionString)
    {
        builder.UseSqlServer(connectionString);
    }

    public static void Configure(DbContextOptionsBuilder<UserCrudDbContext> builder, DbConnection connection)
    {
        builder.UseSqlServer(connection);
    }
}
