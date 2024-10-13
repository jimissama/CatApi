namespace CatApi.Models.SourceApi;

public class CatSourceResponse
{
    public string Id { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public int Width { get; set; }
    public int Height { get; set; }    

    public List<CatBreedSourceResponse> Breeds { get; set; } = new();
}
