using Supabase.Postgrest.Attributes;

[Table("Cars")]
public class Car : Supabase.Postgrest.Models.BaseModel
{
    [PrimaryKey("id", false)]
    public long Id { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("Company")]
    public string Company { get; set; } = string.Empty;

    [Column("Model")]
    public string Model { get; set; } = string.Empty;

    [Column("Price")]
    public long Price { get; set; }

    [Column("Year")]
    public int Year { get; set; }

    [Column("Used")]
    public bool Used { get; set; }
}
