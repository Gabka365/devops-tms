using Api.Database;

namespace Api.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;

        public CarService(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public async Task<int?> CreateCarAsync(string mark, string model, int yearOfRelease)
        {
            if (!await _carRepository.CreateCarTableAsync()) {
                return null;
            }

            return await _carRepository.CreateCarAsync(mark, model, yearOfRelease);
        }

        public async Task<(int id, string mark, string model, int yearOfRelease)?> GetCarAsync(int id)
        {
            return await _carRepository.GetCarAsync(id);
        }
    }
}
