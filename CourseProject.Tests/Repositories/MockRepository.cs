using CourseProject.DAL.Repositories;

namespace CourseProject.Tests.Repositories;

public class MockRepository<T> : IRepository<T> where T : class
{
    private readonly List<T> _entities = new();
    private int _nextId = 1;

    public List<T> GetAll()
    {
        return new List<T>(_entities);
    }

    public T? GetById(int id)
    {
        var property = typeof(T).GetProperty("Id");
        if (property == null)
            return null;

        return _entities.FirstOrDefault(e =>
            property.GetValue(e)?.Equals(id) == true);
    }

    public void Add(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        var property = typeof(T).GetProperty("Id");
        if (property != null && property.GetValue(entity)?.Equals(0) == true)
        {
            property.SetValue(entity, _nextId++);
        }

        _entities.Add(entity);
    }

    public void Update(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        var property = typeof(T).GetProperty("Id");
        if (property == null)
            throw new InvalidOperationException("Entity must have an Id property.");

        var id = property.GetValue(entity);
        var existing = _entities.FirstOrDefault(e =>
            property.GetValue(e)?.Equals(id) == true);

        if (existing == null)
            throw new InvalidOperationException($"Entity with ID {id} not found.");

        var index = _entities.IndexOf(existing);
        _entities[index] = entity;
    }

    public void Delete(int id)
    {
        var property = typeof(T).GetProperty("Id");
        if (property == null)
            throw new InvalidOperationException("Entity must have an Id property.");

        var entity = _entities.FirstOrDefault(e =>
            property.GetValue(e)?.Equals(id) == true);

        if (entity == null)
            throw new InvalidOperationException($"Entity with ID {id} not found.");

        _entities.Remove(entity);
    }

    public void Save()
    {
        // Mock implementation - no actual file operations
    }
}