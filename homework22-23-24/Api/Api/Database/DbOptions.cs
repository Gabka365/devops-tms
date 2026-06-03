namespace Api.Database
{
    public class DbOptions
    {
        public required string ConnectionString { get; set; }

        public required string DatabaseName { get; set; }
    }
}
