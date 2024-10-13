using CatApi.Extensions;
using CatApi.Models.Entities;
using CatApi.Repositories.Interfaces;
using CatApi.Services.Database;
using Microsoft.EntityFrameworkCore;

namespace CatApi.Repositories;

public class TagRepo : ITagRepo
{
    private readonly IServiceProvider _serviceProvider;
    public TagRepo(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<IEnumerable<TagEntity>> GetExistingTags(IEnumerable<TagEntity> tags)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<CatDbContext>();

            return await context.Tags.AsNoTracking()
                                     .Where(tag => tags.Select(t => t.Name)
                                     .Contains(tag.Name))
                                     .ToListAsync();
        }
    }

    public async Task<IEnumerable<TagEntity>> GetExistingTags(IEnumerable<TagEntity> tags, CatDbContext context)
    {
        return await context.Tags.Where(tag => tags.Select(t => t.Name)
                                 .Contains(tag.Name))
                                 .ToListAsync();    
    }

    public async Task<HashSet<string>> GetExistingTagNames(IEnumerable<TagEntity> tags)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<CatDbContext>();

            return new HashSet<string>(await context.Tags
                                     .AsNoTracking()
                                     .Where(tag => tags.Select(t => t.Name)
                                     .Contains(tag.Name))
                                     .Select(tag =>tag.Name)
                                     .ToListAsync());
        }
    }

    public async Task<HashSet<string>> GetExistingTagNames(IEnumerable<TagEntity> tags, CatDbContext context)
    {
            return new HashSet<string>(await context.Tags
                                     .Where(tag => tags.Select(t => t.Name)
                                     .Contains(tag.Name))
                                     .Select(tag =>tag.Name)
                                     .ToListAsync());   
    }

    public async Task StoreTags(IEnumerable<TagEntity> tags)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<CatDbContext>();

            await context.Tags.AddRangeAsync(tags);
            await context.SaveChangesAsync();
        }

    }

    public async Task StoreTags(IEnumerable<TagEntity> tags, CatDbContext context)
    {
        await context.Tags.AddRangeAsync(tags);
    }
}
