using TestWebApi.Models;

namespace TestWebApi.Services;

public class GamesDbService : IDbContext<int, Game>
{
    private readonly TestWebApiContext _context;
    private readonly GenreDbService _genreDbService;

    public GamesDbService(TestWebApiContext context)
    {
        _context = context;
        _genreDbService = new(context);
    }

    private async Task<bool> IsGameExistAsync(Game entity) => await Task.Run(async () => (await GetAllAsync()).Any(game => game.Equals(entity)));

    public async Task<Game?> CreateAsync(Game entity)
    {
        if (await IsGameExistAsync(entity)) return null;
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
        return await GetAsync(entity.Id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var game = await GetAsync(id);
        if (game is null) return false;
        _context.Games.Remove(game);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<ICollection<Game>> GetAllAsync() => await _context.Games.Include(game => game.Genres).ToListAsync();

    public async Task<Game?> GetAsync(int id) => await _context.Games.Include(game => game.Genres).FirstOrDefaultAsync(game => game.Id == id);

    public async Task<IList<Game>> GetByAsync(Func<Game, bool> predicate) => await Task.Run(() => _context.Games.Include(game => game.Genres).Where(predicate).ToList());

    public async Task<Game?> UpdateAsync(int id, Game entity)
    {
        var gameFromDb = await GetAsync(id);
        if (gameFromDb is null || await IsGameExistAsync(entity)) return null;
        gameFromDb.Name = entity.Name;
        gameFromDb.Developer = entity.Developer;
        gameFromDb.Genres = entity.Genres;
        _context.Update(gameFromDb);
        await _context.SaveChangesAsync();
        return await GetAsync(id);
    }

    public async Task<IList<Genre>> GetOrCreateGenresAsync(string[] names) => await _genreDbService.GetOrCreateGenresAsync(names);
}
