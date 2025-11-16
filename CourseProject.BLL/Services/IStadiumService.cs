using CourseProject.BLL.Entities;

namespace CourseProject.BLL.Services;

public interface IStadiumService : IService<Stadium>
{
    List<Stadium> SearchStadiums(string name);
}