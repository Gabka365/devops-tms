namespace Api.Services
{
    public interface ICarService
    {
        public Task<(int id, string mark, string model, int yearOfRelease)?> GetCarAsync(int id);
        public Task<int?> CreateCarAsync(string mark, string model, int yearOfRelease);
    }
}
