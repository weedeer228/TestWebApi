using TestWebApi.Extensions;
using TestWebApi.Models;

namespace TestWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GamesController : Controller
{
    private readonly IDbContext<int, Game> _context;

    public GamesController(TestWebApiContext context)
    {
        _context = new GamesDbService(context);
    }

    private GamesDbService GamesDbServiceContext => (GamesDbService)_context;

    // GET: Games
    [HttpGet("GetAll")]
    [Tags("Get")]
    public async Task<IActionResult> GetAll() => Ok(await _context.GetAllAsync());


    [HttpGet("GetByGenre")]
    [Tags("Get")]
    public async Task<ICollection<Game>> GetGamesByGenre([FromQuery] string[] genres) => await _context.GetByAsync(game => game.Genres.Any(genre => genres.Contains(genre.Name)));

    // POST: Games/Create
    [HttpPost("Create")]
    [Tags("Create")]
    public async Task<IActionResult> Create([FromQuery] string name, [FromQuery] string developer, [FromQuery] string[] genres)
    {
        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(developer))
        {
            var game = new Game()
            {
                Name = name.ToNormalView(),
                Developer = developer.ToNormalView(),
                Genres = await GamesDbServiceContext.GetOrCreateGenresAsync(genres),
            };
            var result = await _context.CreateAsync(game);
            if (result is null) return BadRequest($"Entity with same parameters already exist\nname: {name}\ndeveloper: {developer}");
            return Ok(game);
        }
        return BadRequest();
    }

    [HttpGet("GetById")]
    [Tags("Get")]
    public async Task<IActionResult> GetById([FromQuery] int id)
    {
        var entity = await _context.GetAsync(id);
        if (entity is null)
            return NotFound();
        return Ok(entity);
    }

    [HttpPut("Update")]
    [Tags("Update")]
    public async Task<IActionResult> Edit([FromQuery] int id, [FromQuery] string name, [FromQuery] string developer, [FromQuery] string[] genres)
    {
        if (await _context.GetAsync(id) is null) return NotFound();
        var newGameData = new Game()
        {
            Name = name,
            Developer = developer,
            Genres = await GamesDbServiceContext.GetOrCreateGenresAsync(genres),
        };
        var editedGame = await _context.UpdateAsync(id, newGameData);
        if (editedGame is null) return BadRequest($"Entity with same parameters already exist\nname: {name}\ndeveloper: {developer}");
        return Ok(editedGame);
    }

    [HttpPut("UpdateGenres")]
    [Tags("Update")]
    public async Task<IActionResult> EditGameGenres([FromQuery] int id, [FromQuery] string[] genres)
    {
        var gameFromDb = await _context.GetAsync(id);
        if (gameFromDb is null) return NotFound();
        var newGameData = new Game()
        {
            Name = gameFromDb.Name,
            Developer = gameFromDb.Developer,
            Genres = await GamesDbServiceContext.GetOrCreateGenresAsync(genres),
        };
        var editedGame = await _context.UpdateAsync(id, newGameData, false);
        if (editedGame is null) return NotFound();
        return Ok(editedGame);
    }

    [HttpDelete("Delete")]
    [Tags("Delete")]
    public async Task<IActionResult> Delete([FromQuery] int id)
    {
        if (_context == null)
        {
            return Problem("Entity set 'TestWebApiContext.Game'  is null.");
        }
        if (await _context.DeleteAsync(id))
            return Ok();
        return NotFound();
    }
}
