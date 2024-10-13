using CatApi.Models.Entities;
using CatApi.Repositories.Interfaces;
using CatApi.Services.Database;

namespace CatApi.Repositories;

public class CatTagRepo : ICatTagRepo
{
    private readonly IServiceProvider _serviceProvider;
    public CatTagRepo(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StoreTags(IEnumerable<CatTagEntity> cattags)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<CatDbContext>();

            await context.CatTag.AddRangeAsync(cattags);
            await context.SaveChangesAsync();
        }

    }

    public async Task StoreTags(IEnumerable<CatTagEntity> cattags, CatDbContext context)
    {
        await context.CatTag.AddRangeAsync(cattags);
    }
}

