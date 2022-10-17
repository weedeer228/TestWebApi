namespace TestWebApi.Data
{
    public class DbInitializer
    {
        public static void Initialize(TestWebApiContext context)
        {
            context.Database.EnsureCreated();
        }

        public static void RecreateDb(TestWebApiContext context)
        {
            context.Database.EnsureDeleted();
            Initialize(context);
        }
    }

}
