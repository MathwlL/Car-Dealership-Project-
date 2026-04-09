using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

[Table("Users")]
public class Users : Supabase.Postgrest.Models.BaseModel
{
    [PrimaryKey("id", false)]
    public long Id { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("age")]
    public DateTime BirthDate { get; set; }

    [Column("telephone")]
    public int? Telephone { get; set; }

    [Column("email")]
    public string Email { get; set; }
}
