using TestWebApi.Extensions;

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
    [HttpGet("GameExist")]
    [Tags("Get")]
    private async Task<bool> GameExists(int id)
    {
        return await _context.GetAsync(id) != null;
    }
}
