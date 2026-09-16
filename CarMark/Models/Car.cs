namespace CarMark.Models;

public class Car
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Company { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public long Price { get; set; }
    public int Year { get; set; }
    public bool Used { get; set; }
    public string? Details { get; set; }
    public string? Image { get; set; }
    public long? SellerId { get; set; }
    public User? Seller { get; set; }
    public List<CarImage> Images { get; set; } = new();
}
