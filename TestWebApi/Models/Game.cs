using System.ComponentModel.DataAnnotations;

namespace TestWebApi.Models;

public class Game
{
    public int Id { get; set; }

    [Required]
    [MaxLength(92)]
    public string Name { get; set; }

    [Required]
    [MaxLength(50)]
    public string Developer { get; set; }

    public IList<Genre> Genres { get; init; } = new List<Genre>();
}
