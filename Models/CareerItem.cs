namespace BaltaDataAcess.Models;
public class CareerItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public Course Course { get; set; } = new();
}