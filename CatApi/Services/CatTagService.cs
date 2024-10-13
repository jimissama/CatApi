using CatApi.Models.Entities;
using CatApi.Repositories.Interfaces;
using CatApi.Services.Database;
using CatApi.Services.Interfaces;

namespace CatApi.Services;

public class CatTagService : ICatTagService
{
    private readonly ICatTagRepo _catTagRepo;
    public CatTagService(IServiceProvider serviceProvider)
    {
        _catTagRepo = serviceProvider.GetRequiredService<ICatTagRepo>();
    }
    public async Task StoreTags(IEnumerable<CatTagEntity> cattags, CatDbContext context)
    {
        await _catTagRepo.StoreTags(cattags, context);
    }

    public async Task StoreTags(IEnumerable<CatTagEntity> cattags)
    {
        await _catTagRepo.StoreTags(cattags);
    }
}
