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
        if (ModelState.IsValid && !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(developer))
        {
            var game = new Game()
            {
                Name = name,
                Developer = developer,
                Genres = await ((GamesDbService)_context).GetOrCreateGenresAsync(genres),
            };
            await _context.CreateAsync(game);
            return Ok(game);
        }
        return BadRequest();
    }

    [HttpPut("Update")]
    [Tags("Update")]
    public async Task<IActionResult> Edit([FromQuery] int id, [Bind("Id,Name,Developer")] Game game)
    {
        if (id != game.Id)
            return NotFound();
        if (ModelState.IsValid)
        {
            var editedGame = await _context.UpdateAsync(game);
            if (editedGame is null)
                return NotFound();
            return RedirectToAction(nameof(Index));
        }
        return BadRequest(); ;
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
    [HttpGet]
    [Tags("Get")]
    private bool GameExists(int id)
    {
        return _context.GetAsync(id) != null;
    }
}
