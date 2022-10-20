using System.ComponentModel.DataAnnotations;

namespace TestWebApi.Models;

public class Genre
{
    public int Id { get; set; }

    [Required]
    [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
    public string Name { get; set; }
}
