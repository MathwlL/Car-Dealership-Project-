using Supabase.Postgrest.Attributes;

namespace CarMark.Models
{
    [Table("CarImages")]
    public class CarImage : Supabase.Postgrest.Models.BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("Img_Url")]
        public string? ImgUrl { get; set; }

        [Column("CarID")]
        public long CarId { get; set; }
    }
}