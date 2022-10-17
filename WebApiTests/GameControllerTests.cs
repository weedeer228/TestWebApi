
namespace WebApiTests
{
    public class GameControllerTests
    {
        private readonly HttpClient _client;

        public GameControllerTests()
        {
            _client = new WebApplicationFactory<Program>().CreateClient();
        }

        [Fact]
        public void Test1()
        {

        }
    }
}