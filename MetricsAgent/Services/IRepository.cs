namespace MetricsAgent.Services
{
    public interface IRepository<T> where T : class
    {
        IList<T> GetByTimePeriod(TimeSpan fromTime, TimeSpan toTime);
        IList<T> GetAll();
        T GetById(int id);
        int Create(T item);
        int Update(T item);
        int Delete(int id);
    }
}
