using CatApi.Models.Entities;
using CatApi.Repositories.Interfaces;
using CatApi.Services.Database;
using Microsoft.EntityFrameworkCore;

namespace CatApi.Repositories;

public class CatRepo : ICatRepo
{
    private readonly IServiceProvider _serviceProvider;
    public CatRepo(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public async Task StoreCats(IEnumerable<CatEntity> catEntities)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<CatDbContext>();

            await context.Cats.AddRangeAsync(catEntities);
            await context.SaveChangesAsync();
        }
    }

    public async Task StoreCats(IEnumerable<CatEntity> catEntities, CatDbContext context)
    {
        await context.Cats.AddRangeAsync(catEntities);
    }

    public async Task<CatEntity?> GetCatById(int id)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<CatDbContext>();

            return await context.Cats.AsNoTracking()
                                     .Include(c => c.CatTags)
                                     .ThenInclude(t => t.Tag)
                                     .Where(cd => cd.Id == id)
                                     .FirstOrDefaultAsync();
        }
    }

    public async Task<List<CatEntity>> GetCats(string? tag, int page, int pageSize)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<CatDbContext>();

            var query = context.Cats.AsNoTracking()
                                    .Include(c => c.CatTags)
                                    .ThenInclude(t => t.Tag)
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .AsQueryable();

            if (!string.IsNullOrEmpty(tag))
                query.Where(c => c.CatTags.Select(t => t.Tag).Any(t => t.Name.Equals(tag, StringComparison.InvariantCulture)));

            return await query.ToListAsync();

        }
    }

    public async Task<IEnumerable<CatEntity>> GetExistingCats(IEnumerable<CatEntity> cats)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<CatDbContext>();

            return await context.Cats.AsNoTracking()
                                     .Include(c => c.CatTags)
                                     .ThenInclude(t => t.Tag)
                                     .Where(c => cats.Select(t => t.CatId)
                                     .Contains(c.CatId))
                                     .ToListAsync();
        }
    }

    public async Task<IEnumerable<CatEntity>> GetExistingCats(IEnumerable<CatEntity> cats, CatDbContext context)
    {
        return await context.Cats
                                 .Include(c => c.CatTags)
                                 .ThenInclude(t => t.Tag)
                                 .Where(c => cats.Select(t => t.CatId)
                                 .Contains(c.CatId))
                                 .ToListAsync();
    }

    public async Task<HashSet<string>> GetExistingCatIds(IEnumerable<CatEntity> cats)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<CatDbContext>();

            return new HashSet<string>(await context.Cats.AsNoTracking()
                                                         .Where(c => cats.Select(t => t.CatId)
                                                         .Contains(c.CatId))
                                                         .Select(c => c.CatId)
                                                         .ToListAsync());
        }
    }

    public async Task<HashSet<string>> GetExistingCatIds(IEnumerable<CatEntity> cats, CatDbContext context)
    {
        return new HashSet<string>(await context.Cats.Where(c => cats.Select(t => t.CatId)
                                                     .Contains(c.CatId))
                                                     .Select(c => c.CatId)
                                                     .ToListAsync());
    }

    public async Task<HashSet<string>> GetExistingCatIds(IEnumerable<string> catIds)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<CatDbContext>();

            return new HashSet<string>(await context.Cats.AsNoTracking()
                                                         .Where(c => catIds
                                                         .Contains(c.CatId))
                                                         .Select(c => c.CatId)
                                                         .ToListAsync());
        }
    }

    public async Task<HashSet<string>> GetExistingCatIds(IEnumerable<string> catIds, CatDbContext context)
    {
        return new HashSet<string>(await context.Cats.Where(c => catIds
                                                     .Contains(c.CatId))
                                                     .Select(c => c.CatId)
                                                     .ToListAsync());
    }
}
