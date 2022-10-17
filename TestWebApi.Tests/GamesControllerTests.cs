namespace TestWebApi.Tests;
[TestFixture]
public class GamesControllerTests
{
    [Test]
    public async Task GetAllTest()
    {
        //Arrange
        var webHost = new WebApplicationFactory<Program>().WithWebHostBuilder(_ => { });
        var client = webHost.CreateClient();
        //Act
        var responce = await client.GetAsync("api/Games/GetAll");
        //Assert
        Assert.That(responce.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task GetByGenrelTest()
    {
        //Arrange
        var webHost = new WebApplicationFactory<Program>().WithWebHostBuilder(_ => { });
        var client = webHost.CreateClient();
        //Act
        await client.PostAsync("api/Games/Create?name=testGame&developer=testDev&genres=testGen", null);
        var responce = await client.GetAsync("api/Games/GetAll");
        //Assert
        Assert.That(responce.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task CreateSuccessTest()
    {
        //Arrange
        var webHost = new WebApplicationFactory<Program>().WithWebHostBuilder(_ => { });
        var client = webHost.CreateClient();
        //Act
        var responce = await client.PostAsync("api/Games/Create?name=testGame&developer=testDev&genres=testGen", null);
        //Assert
        Assert.That(responce.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task DeleteSuccessTest()
    {
        //Arrange
        var webHost = new WebApplicationFactory<Program>().WithWebHostBuilder(_ => { });
        var client = webHost.CreateClient();
        //Act
        await client.PostAsync("api/Games/Create?name=testGame&developer=testDev&genres=testGen", null);
        var responce = await client.DeleteAsync("api/Games/Delete?id=1");
        //Assert
        Assert.That(responce.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task DeleteFailedTest()
    {
        //Arrange
        var webHost = new WebApplicationFactory<Program>().WithWebHostBuilder(_ => { });
        var client = webHost.CreateClient();
        //Act
        var responce = await client.DeleteAsync("api/Games/Delete?id=1");
        //Assert
        Assert.That(responce.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
    [Test]
    public async Task CreateFailedTest()
    {
        //Arrange
        var webHost = new WebApplicationFactory<Program>().WithWebHostBuilder(_ => { });
        var client = webHost.CreateClient();
        //Act
        await client.PostAsync("https://localhost:44351/api/Games/Create?name=1v&developer=1&genres=1", null);
        var responce = await client.PostAsync("https://localhost:44351/api/Games/Create?name=1v&developer=1&genres=1", null);
        //Assert
        Assert.That(responce.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
    [Test]
    public async Task UpdateSuccessTest()
    {
        //Arrange
        var webHost = new WebApplicationFactory<Program>().WithWebHostBuilder(_ => { });
        var client = webHost.CreateClient();
        //Act
        await client.PostAsync("https://localhost:44351/api/Games/Create?name=1v&developer=1&genres=1", null);
        var responce = await client.PutAsync("https://localhost:44351/api/Games/Update?id=1&name=2&developer=1", null);
        //Assert
        Assert.That(responce.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
    [Test]
    public async Task UpdateFailedTest()
    {
        //Arrange
        var webHost = new WebApplicationFactory<Program>().WithWebHostBuilder(_ => { });
        var client = webHost.CreateClient();
        //Act
        await client.PostAsync("https://localhost:44351/api/Games/Create?name=1&developer=1", null);
        await client.PostAsync("https://localhost:44351/api/Games/Create?name=2&developer=1", null);
        var responce = await client.PutAsync("https://localhost:44351/api/Games/Update?id=1&name=2&developer=1", null);
        //Assert
        Assert.That(responce.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

}
