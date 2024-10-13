using CatApi.Models.Entities;

namespace CatApi.Helpers;

public static class CatBreedTemperamentHelper
{
    public static IEnumerable<TagEntity> GetTagEntitiesFromTemperamentsList(List<string> temperaments)
    {
        var temperamentsResult = new List<string>();
        var tagEntities = new List<TagEntity>();

        foreach (var temperament in temperaments)
        {
            temperamentsResult.AddRange(temperament.Replace(" ", "").Split(',').Distinct().ToList());
        }

        foreach (var tag in temperamentsResult)
        {
            tagEntities.Add(new TagEntity{Name = tag});
        }

        return tagEntities;

    }

        public static IEnumerable<string> GetTagTemperamentsList(IEnumerable<string> temperaments)
    {
        var temperamentsResult = new List<string>();

        foreach (var temperament in temperaments)
        {
            temperamentsResult.AddRange(temperament.Replace(" ", "").Split(',').Distinct().ToList());
        }

        return temperamentsResult;

    }
}
