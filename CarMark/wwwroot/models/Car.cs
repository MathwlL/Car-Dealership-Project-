using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace CarMark;

[Table("Cars")]
public class Carro : BaseModel
{
    [PrimaryKey("id", false)]
    public long Id { get; set; }

    [Column("Company")]
    public string Company { get; set; }

    [Column("Model")]
    public string Model { get; set; }

    [Column("Price")]
    public long Price { get; set; }

    [Column("Year")]
    public int Year { get; set; }

    [Column("Used")]
    public bool Used { get; set; }
}