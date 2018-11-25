using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ResourceAllocation.Domain;

namespace ResourceAllocation.Services.Show
{
    public interface IShowsService
    {
        Task Add(ShowEntity entity);
        Task Delete(Guid id);
        Task<IEnumerable<ShowEntity>> GetAll();
        Task<ShowEntity> GetById(Guid id);
        Task Update(ShowEntity entity);
    }
}
