using System.Diagnostics.CodeAnalysis;
using CatApi.Models.Entities;

namespace CatApi.Extensions;

public class TagEqualityExtension : IEqualityComparer<TagEntity>
{
    public bool Equals(TagEntity? x, TagEntity? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;
        return x.Name.ToLower() == y.Name.ToLower();

    }

    public int GetHashCode([DisallowNull] TagEntity obj)
    {
        if (obj is null) return 0;
        int hashName = obj.Name == null ? 0 : obj.Name.GetHashCode();
        return hashName;
    }
}
