using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryBase.Repository
{
    interface IRepository<T> where T: class
    {
        Task<List<T>> getAll();
        Task<T> getById(int id);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(int id);


    }
}
