using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ResourceAllocation.Domain;

namespace ResourceAllocation.DataLayer.Shows
{
    public interface IShowsRepository
    {
        Task Add(ShowEntity entity);
        Task Delete(Guid id);
        Task<IEnumerable<ShowEntity>> GetAll();
        Task<ShowEntity> GetById(Guid id);
        Task Update(ShowEntity entity);
    }
}
