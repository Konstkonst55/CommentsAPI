using System.ComponentModel.DataAnnotations.Schema;

namespace CommentsAPI.Models
{
    [Table("comments")]
    public class Comment
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("post_id")]
        public int PostId { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("body")]
        public string? Body { get; set; }
    }
}
