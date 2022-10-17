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
    public async Task CreateTest()
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

}
