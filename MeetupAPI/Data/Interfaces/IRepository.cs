namespace MeetupAPI.Data.Interfaces
{
    public interface IRepository<T> : IDisposable
    {
        IEnumerable<T> GetAll();
        T GetById(long id);
        IEnumerable<T> Find(Func<T, bool> predicate);
        T Create(T entity);
        T Update(T entity);
        void Delete(T entity);
        int Save();
    }
}
