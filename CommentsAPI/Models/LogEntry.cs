using System.ComponentModel.DataAnnotations.Schema;

namespace CommentsAPI.Models
{
    [Table("logs")]
    public class LogEntry
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("timestamp")]
        public DateTime Timestamp { get; set; }

        [Column("level")]
        public string? Level { get; set; }

        [Column("message")]
        public string? Message { get; set; }

        [Column("exception")]
        public string? Exception { get; set; }

        [Column("endpoint")]
        public string? Endpoint { get; set; }
    }
}
