using CatApi.Models.Entities;
using CatApi.Services.Database;

namespace CatApi.Repositories.Interfaces;

public interface ITagRepo
{
    public Task<IEnumerable<TagEntity>> GetExistingTags(IEnumerable<TagEntity> tags);

    public Task<IEnumerable<TagEntity>> GetExistingTags(IEnumerable<TagEntity> tags, CatDbContext context);

    public Task<HashSet<string>> GetExistingTagNames(IEnumerable<TagEntity> tags);

    public Task<HashSet<string>> GetExistingTagNames(IEnumerable<TagEntity> tags, CatDbContext context);

    public Task StoreTags(IEnumerable<TagEntity> tags);

    public Task StoreTags(IEnumerable<TagEntity> tags, CatDbContext context);


}
