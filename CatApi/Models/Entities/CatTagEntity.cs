namespace CatApi.Models.Entities;

public class CatTagEntity
{
    public int CatId { get; set; }
    public CatEntity Cat { get; set; } = new();
    public int TagId { get; set; }
    public TagEntity Tag { get; set; } = new();

}
