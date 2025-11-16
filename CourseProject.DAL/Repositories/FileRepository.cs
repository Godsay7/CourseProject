using System.Text.Json;

namespace CourseProject.DAL.Repositories;

public class FileRepository<T> : IRepository<T> where T : class
{
    private readonly string _filePath;
    private List<T> _entities;

    public FileRepository(string filePath)
    {
        _filePath = filePath;
        _entities = LoadFromFile();
    }

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
        try
        {
            var directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var json = JsonSerializer.Serialize(_entities, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(_filePath, json);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to save data to file: {ex.Message}", ex);
        }
    }

    private List<T> LoadFromFile()
    {
        try
        {
            if (!File.Exists(_filePath))
                return new List<T>();

            var json = File.ReadAllText(_filePath);
            if (string.IsNullOrWhiteSpace(json))
                return new List<T>();

            var entities = JsonSerializer.Deserialize<List<T>>(json);
            return entities ?? new List<T>();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to load data from file: {ex.Message}", ex);
        }
    }
}