using TestWebApi.Extensions;

namespace TestWebApi.Services;

public class GenreDbService
{
    private readonly TestWebApiContext _context;

    public GenreDbService(TestWebApiContext context)
    {
        _context = context;
    }

    public async Task<IList<Genre>> GetOrCreateGenresAsync(string[] genreNames)
    {
        var result = new List<Genre>();
        foreach (var genreName in genreNames)
        {
            var name = genreName.ToNormalView();
            var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Name.Equals(name));
            if (genre is null)
            {
                genre = new Genre() { Name = name };
                await CreateGenreAsync(genre);
                await _context.SaveChangesAsync();
            }
            if (!result.Contains(genre))
                result.Add(genre);
        }
        return result;
    }


    private async Task CreateGenreAsync(Genre genre) => await _context.Genres.AddAsync(genre);
}
