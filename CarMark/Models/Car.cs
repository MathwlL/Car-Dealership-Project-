using Supabase.Postgrest.Attributes;
namespace CarMark.Models
{
    [Table("Cars")]
    public class Car : Supabase.Postgrest.Models.BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        public string Company { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public long Price { get; set; }

        public int Year { get; set; }

        public bool Used { get; set; }

        public string? Details { get; set; }

        [Column("Imagem")]
        public string? Image { get; set; }

        [Column("SellerID")]
        public long? SellerId { get; set; }
    }

}
