using CourseProject.BLL.Entities;
using CourseProject.BLL.Exceptions;
using CourseProject.DAL.Repositories;

namespace CourseProject.BLL.Services;

public class StadiumService : IStadiumService
{
    private readonly IRepository<Stadium> _repository;

    public StadiumService(IRepository<Stadium> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public List<Stadium> GetAllEntity()
    {
        try
        {
            return _repository.GetAll();
        }
        catch (Exception ex)
        {
            throw new BusinessLogicException($"Failed to retrieve stadiums: {ex.Message}", ex);
        }
    }

    public Stadium GetByIdEntity(int id)
    {
        try
        {
            var stadium = _repository.GetById(id);
            if (stadium == null)
                throw new StadiumNotFoundException(id);
            return stadium;
        }
        catch (StadiumNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessLogicException($"Failed to retrieve stadium: {ex.Message}", ex);
        }
    }

    public void AddEntity(Stadium stadium)
    {
        if (stadium == null)
            throw new ArgumentNullException(nameof(stadium));

        try
        {
            // Generate ID if not set
            if (stadium.Id == 0)
            {
                var allStadiums = _repository.GetAll();
                stadium.Id = allStadiums.Count > 0 ? allStadiums.Max(s => s.Id) + 1 : 1;
            }

            _repository.Add(stadium);
            _repository.Save();
        }
        catch (Exception ex)
        {
            throw new BusinessLogicException($"Failed to add stadium: {ex.Message}", ex);
        }
    }

    public void UpdateEntity(Stadium stadium)
    {
        if (stadium == null)
            throw new ArgumentNullException(nameof(stadium));

        try
        {
            var existing = _repository.GetById(stadium.Id);
            if (existing == null)
                throw new StadiumNotFoundException(stadium.Id);

            _repository.Update(stadium);
            _repository.Save();
        }
        catch (StadiumNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessLogicException($"Failed to update stadium: {ex.Message}", ex);
        }
    }

    public void DeleteEntity(int id)
    {
        try
        {
            var stadium = _repository.GetById(id);
            if (stadium == null)
                throw new StadiumNotFoundException(id);

            _repository.Delete(id);
            _repository.Save();
        }
        catch (StadiumNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessLogicException($"Failed to delete stadium: {ex.Message}", ex);
        }
    }

    public List<Stadium> SearchStadiums(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return GetAllEntity();

        try
        {
            var allStadiums = GetAllEntity();
            var searchTerm = name.ToLowerInvariant();
            return allStadiums.Where(s =>
                s.Name.ToLowerInvariant().Contains(searchTerm)).ToList();
        }
        catch (Exception ex)
        {
            throw new BusinessLogicException($"Failed to search stadiums: {ex.Message}", ex);
        }
    }
}