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

    public async Task<HashSet<string>> GetExistingTagNames(IEnumerable<TagEntity> tags)
    {
        return await _tagRepo.GetExistingTagNames(tags);
    }

    public async Task<HashSet<string>> GetExistingTagNames(IEnumerable<TagEntity> tags, CatDbContext context)
    {
        return await _tagRepo.GetExistingTagNames(tags, context);
    }

    public async Task<IEnumerable<TagEntity>> GetExistingTags(IEnumerable<TagEntity> tags)
    {
        return await _tagRepo.GetExistingTags(tags);
    }

    public async Task<IEnumerable<TagEntity>> GetExistingTags(IEnumerable<TagEntity> tags, CatDbContext context)
    {
        return await _tagRepo.GetExistingTags(tags, context);
    }

    public async Task StoreTags(IEnumerable<TagEntity> tags)
    {
        await _tagRepo.StoreTags(tags);
    }

    public async Task StoreTags(IEnumerable<TagEntity> tags, CatDbContext context)
    {
        await _tagRepo.StoreTags(tags, context);
    }
}
