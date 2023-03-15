using MeetupAPI.Data.Interfaces;
using MeetupAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetupAPI.Data.DbDataSource
{
    public class EventDbRepository : IAsyncRepository<Event>
    {
        private readonly EventContext _eventContext;

        public EventDbRepository(EventContext eventContext)
        {
            _eventContext = eventContext;
        }

        public Event Create(Event entity)
        {
            return _eventContext.Add(entity).Entity;
        }

        public async Task<Event> CreateAsync(Event entity)
        {
            return (await _eventContext.AddAsync(entity)).Entity;
        }

        public void Delete(Event entity)
        {
            _eventContext.Remove(entity);
        }

        public async Task DeleteAsync(Event entity)
        {
            await Task.Factory.StartNew(() =>
            {
                _eventContext.Remove(entity);
            });
        }

        private bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _eventContext.Dispose();
            }
            _disposed = true;
        }

        public IEnumerable<Event> Find(Func<Event, bool> predicate)
        {
            return _eventContext.Events.AsNoTracking().Where(predicate);
        }

        public async Task<IEnumerable<Event>> FindAsync(Func<Event, bool> predicate)
        {
            return await Task.Factory.StartNew(() =>
            {
                return _eventContext.Events.AsNoTracking().Where(predicate);
            });
        }

        public IEnumerable<Event> GetAll()
        {
            return _eventContext.Events.AsNoTracking();
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await Task.Factory.StartNew(() =>
            {
                return _eventContext.Events;
            });
        }

        public Event GetById(long id)
        {
            return _eventContext.Events.AsNoTracking().SingleOrDefault(x=>x.Id == id);
        }

        public async Task<Event> GetByIdAsync(long id)
        {
            return await _eventContext.Events.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
        }

        public int Save()
        {
            return _eventContext.SaveChanges();
        }

        public Task<int> SaveAsync()
        {
            return _eventContext.SaveChangesAsync();
        }

        public Event Update(Event entity)
        {
            return _eventContext.Update(entity).Entity;
        }

        public async Task<Event> UpdateAsync(Event entity)
        {
            return await Task.Factory.StartNew(() =>
            {
                return _eventContext.Update(entity).Entity;
            });
        }
    }
}
