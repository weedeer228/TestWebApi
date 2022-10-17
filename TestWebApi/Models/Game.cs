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

    public IList<Genre> Genres { get; set; } = new List<Genre>();

    public override bool Equals(object? obj) => obj is Game game && game.Name.Equals(Name) && game.Developer.Equals(Developer);

}
