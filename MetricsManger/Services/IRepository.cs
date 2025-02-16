namespace MetricsManager.Services
{
    public interface IRepository<T> where T : class
    {
        int Add(T value);
        IList<T> Get();
        int Update(T value);
    }
}
