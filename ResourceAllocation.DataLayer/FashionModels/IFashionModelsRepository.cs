using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ResourceAllocation.Domain;

namespace ResourceAllocation.DataLayer.FashionModels
{
    public interface IFashionModelsRepository
    {
        Task Add(FashionModelEntity entity);
        Task Delete(Guid id);
        Task<IEnumerable<FashionModelEntity>> GetAll();
        Task<FashionModelEntity> GetById(Guid id);
        Task Update(FashionModelEntity entity);
    }
}