namespace CarMark.Models;

public class CarImage
{
    public long Id { get; set; }
    public string? ImgUrl { get; set; }
    public long CarId { get; set; }
    public Car? Car { get; set; }
}
