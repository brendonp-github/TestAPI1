using System.ComponentModel.DataAnnotations;

namespace TestAPI1.Data.Models
{
  public class Post
  {
    [Key]
    [Required]
    public long PostID { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
  }
}
