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

    public async Task CreateAsync(Game entity)
    {
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
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

    public async Task<Game?> UpdateAsync(Game entity)
    {
        try
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return await GetAsync(entity.Id);
        }
        catch (DbUpdateConcurrencyException)
        {
            return null;
        }

    }

    public async Task<IList<Genre>> GetOrCreateGenresAsync(string[] names) => await _genreDbService.GetOrCreateGenresAsync(names);
}
