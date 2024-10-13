using System.Diagnostics.CodeAnalysis;
using CatApi.Models.Entities;

namespace CatApi.Extensions;

public class StringEqualityExtension : IEqualityComparer<string>
{
    public bool Equals(string? x, string? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;
        return x.Equals(y, StringComparison.InvariantCultureIgnoreCase);

    }

    public int GetHashCode([DisallowNull] string obj)
    {
        if (obj is null) return 0;
        return obj.GetHashCode();
    }
}
