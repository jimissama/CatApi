
namespace CatApi.Models.Response;

/// <summary>
/// Cat Image Record
/// </summary>
public class CatResponse
{
    /// <summary>
    /// Cat Image record Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Cat Image provider Id
    /// </summary>
    public string CatId { get; set; } = string.Empty;

    /// <summary>
    /// Cat Image's width
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// Cat Image's height
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Cat image url
    /// </summary>
    public string Image { get; set; } = string.Empty;

    /// <summary>
    /// The Date and Time the Cat Image created
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    /// Cat temperaments
    /// </summary>
    public ICollection<string> Tags { get; set; } = new List<string>();
}