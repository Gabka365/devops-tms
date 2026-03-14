namespace test_unit_service.Managers
{
    public interface IAppManager
    {
        Task<string?> GetByIdAsync(int id);
    }
}
