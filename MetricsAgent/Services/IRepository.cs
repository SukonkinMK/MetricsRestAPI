namespace MetricsAgent.Services
{
    public interface IRepository<T> where T : class
    {
        IList<T> GetByTimePeriod(TimeSpan fromTime, TimeSpan toTime);
        IList<T> GetAll();
        T GetById(int id);
        void Create(T item);
        void Update(T item);
        void Delete(int id);
    }
}
