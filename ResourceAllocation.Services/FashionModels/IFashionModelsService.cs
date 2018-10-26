using System;
using System.Collections.Generic;
using ResourceAllocation.Domain;

namespace ResourceAllocation.Services.FashionModels
{
    public interface IFashionModelsService
    {
        void Add(FashionModelEntity entity);
        void Delete(Guid id);
        IEnumerable<FashionModelEntity> GetAll();
        FashionModelEntity GetById(Guid id);
        void Update(FashionModelEntity entity);
    }
}