using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.BLL.Services
{
    public interface IService<T>
    {
        List<T> GetAllEntity();
        T GetByIdEntity(int id);
        void AddEntity(T entity);
        void UpdateEntity(T entity);
        void DeleteEntity(int id);
    }
}
