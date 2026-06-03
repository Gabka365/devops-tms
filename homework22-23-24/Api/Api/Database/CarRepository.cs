using Api.Database;
using Dapper;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
namespace Api.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly DbOptions _dbOptions;
        private readonly ILogger<CarRepository> _logger;

        public CarRepository(IOptions<DbOptions> dbOptions, ILogger<CarRepository> logger)
        {
            _dbOptions = dbOptions.Value;
            _logger = logger;
        }

        public async Task<bool> CreateCarTableAsync()
        {
            try
            {
                using (var connection = new MySqlConnection(_dbOptions.ConnectionString))
                {
                    var commandText = $@"CREATE DATABASE IF NOT EXISTS `{_dbOptions.DatabaseName}`;";
                    await connection.ExecuteAsync(commandText);

                    commandText = $@"
                        CREATE DATABASE IF NOT EXISTS `{_dbOptions.DatabaseName}`;
                        CREATE TABLE IF NOT EXISTS `{_dbOptions.DatabaseName}`.Cars (
                            id INT AUTO_INCREMENT PRIMARY KEY,
                            mark VARCHAR(100) NOT NULL,
                            model VARCHAR(100) NOT NULL,
                            yearOfRelease INT NOT NULL
                        );";
                    await connection.ExecuteAsync(commandText);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{CarRepository} - {CreateCarTableAsync} error create car table. {ex.Message}",
                    nameof(CarRepository), "CreateCarTableAsync", ex.Message);
                return false;
            }
        }

        public async Task<int?> CreateCarAsync(string mark, string model, int yearOfRelease)
        {
            try
            {
                using (var connection = new MySqlConnection(_dbOptions.ConnectionString))
                {
                    var commandText = $@"
                INSERT INTO `{_dbOptions.DatabaseName}`.Cars (mark, model, yearOfRelease) 
                VALUES (@mark, @model, @yearOfRelease);
                SELECT LAST_INSERT_ID();";

                    var id = await connection.QuerySingleAsync<int>(commandText, new
                    {
                        mark = mark,
                        model = model,
                        yearOfRelease = yearOfRelease
                    });

                    return id;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{CarRepository} - {CreateCarAsync} error create car. {ex.Message}",
                    nameof(CarRepository), "CreateCarAsync", ex.Message);
                return null;
            }
        }

        public async Task<(int id, string mark, string model, int yearOfRelease)?> GetCarAsync(int id)
        {
            try
            {
                using (var connection = new MySqlConnection(_dbOptions.ConnectionString))
                {
                    var commandText = $"SELECT id, mark, model, yearOfRelease FROM `{_dbOptions.DatabaseName}`.Cars WHERE id = @id;";
                    var car = await connection.QuerySingleOrDefaultAsync<(int id, string mark, string model, int yearOfRelease)>(commandText, new { id = id });
                    return car;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{CarRepository} - {GetCarAsync} error create car table. {ex.Message}", 
                    nameof(CarRepository), "GetCarAsync", ex.Message);
                return null;
            }
        }
    }
}
