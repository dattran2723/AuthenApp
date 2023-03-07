using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Entities
{
    [Table("tokens")]
    public class Token : IEntity
    {
        public int Id { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("refresh_token")]
        public string RefreshToken { get; set; } = string.Empty;

        [Column("expires_in")]
        public string ExpiresIn { get; set; } = string.Empty;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
