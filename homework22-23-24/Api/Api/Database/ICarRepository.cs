namespace Api.Database
{
    public interface ICarRepository
    {
        public Task<bool> CreateCarTableAsync();
        public Task<(int id, string mark, string model, int yearOfRelease)?> GetCarAsync(int id);
        public Task<int?> CreateCarAsync(string mark, string model, int yearOfRelease);
    }
}
