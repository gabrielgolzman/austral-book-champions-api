using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Helpers;

public static class DbContextFactory
{
    public static BookDbContext Create(string? databaseName = null)
    {
        var options = new DbContextOptionsBuilder<BookDbContext>()
            .UseInMemoryDatabase(databaseName ?? Guid.NewGuid().ToString())
            .Options;

        return new BookDbContext(options);
    }
}
