using AutoMapper;
using CatApi.Models.Entities;
using CatApi.Repositories.Interfaces;
using CatApi.Services.Database;
using CatApi.Services.Interfaces;

namespace CatApi.Services;

public class TagService : ITagService
{
    private readonly ITagRepo _tagRepo;

    public TagService(IServiceProvider serviceProvider)
    {
        _tagRepo = serviceProvider.GetRequiredService<ITagRepo>();
    }

    public async Task<HashSet<string>> GetExistingTagNamesAsync(IEnumerable<TagEntity> tags)
    {
        return await _tagRepo.GetExistingTagNamesAsync(tags);
    }

    public async Task<HashSet<string>> GetExistingTagNamesAsync(IEnumerable<TagEntity> tags, CatDbContext context)
    {
        return await _tagRepo.GetExistingTagNamesAsync(tags, context);
    }

    public async Task<IEnumerable<TagEntity>> GetExistingTagsAsync(IEnumerable<TagEntity> tags)
    {
        return await _tagRepo.GetExistingTagsAsync(tags);
    }

    public async Task<IEnumerable<TagEntity>> GetExistingTagsAsync(IEnumerable<TagEntity> tags, CatDbContext context)
    {
        return await _tagRepo.GetExistingTagsAsync(tags, context);
    }

    public async Task StoreTagsAsync(IEnumerable<TagEntity> tags)
    {
        await _tagRepo.StoreTagsAsync(tags);
    }

    public async Task StoreTagsAsync(IEnumerable<TagEntity> tags, CatDbContext context)
    {
        await _tagRepo.StoreTagsAsync(tags, context);
    }
}
