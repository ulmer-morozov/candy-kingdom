using CandyKingdom.MarcyCms.Sample.Content;

using Microsoft.EntityFrameworkCore;

namespace CandyKingdom.MarcyCms.Sample.Data;

public sealed class InitialDataFiller
{
    private readonly IDbContextFactory<CmsSampleDbContext> _contextFactory;

    public InitialDataFiller(IDbContextFactory<CmsSampleDbContext> dbContextFactory)
    {
        _contextFactory = dbContextFactory;
    }

    public async Task InitializeIfNecessary()
    {
        await using (var context = await _contextFactory.CreateDbContextAsync())
        {
            await context.Database.EnsureCreatedAsync();
            await context.Database.MigrateAsync();

            // await Add
        }
    }
}
