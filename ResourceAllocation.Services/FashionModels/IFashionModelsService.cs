using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ResourceAllocation.Domain;

namespace ResourceAllocation.Services.FashionModels
{
    public interface IFashionModelsService
    {
        Task Add(FashionModelEntity entity);
        Task Delete(Guid id);
        Task<IEnumerable<FashionModelEntity>> GetAll();
        Task<FashionModelEntity> GetById(Guid id);
        Task Update(FashionModelEntity entity);
    }
}