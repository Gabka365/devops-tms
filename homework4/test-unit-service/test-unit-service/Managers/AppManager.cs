namespace test_unit_service.Managers
{
    public class AppManager : IAppManager
    {
        private readonly List<string> _mockEntities = new List<string>()
        {
            "Entity 0",
            "Entity 1",
            "Entity 2",
            "Entity 3",
            "Entity 4",
            "Entity 5",
            "Entity 6"
        };

        public async Task<string?> GetByIdAsync(int id)
        {
           if (id >= _mockEntities.Count)
                return "Not found";    

           await Task.Delay(TimeSpan.FromSeconds(1)); 

           return _mockEntities[id];
        }
    }
}
