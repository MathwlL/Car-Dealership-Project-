using Supabase.Postgrest.Attributes;

namespace CarMark.Models
{
    [Table("Users")]
    public class Users : Supabase.Postgrest.Models.BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("age")]
        public DateTime? BirthDate { get; set; }

        [Column("telephone")]
        public string Phone { get; set; } = string.Empty;

        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column("is_seller")]
        public bool IsSeller { get; set; }

        [Column("is_buyer")]
        public bool IsBuyer { get; set; }
    }
}
