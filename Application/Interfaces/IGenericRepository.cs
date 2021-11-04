using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        void AddAsync(T obj);
        void Update(T obj);
        void Delete(T obj);
        Task<T> GetEntityWithSpecAsync(ISpecification<T> spec);
        Task<T> GetLastEntityWithSpecAsync(ISpecification<T> spec);
        Task<IReadOnlyList<T>> ListAllWithSpecAsync(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> spec);
    }
}