using System;
using System.Collections.Generic;
using ResourceAllocation.Domain;

namespace ResourceAllocation.DataLayer.FashionModels
{
    public interface IFashionModelsRepository
    {
        void Add(FashionModelEntity entity);
        void Delete(Guid id);
        IEnumerable<FashionModelEntity> GetAll();
        FashionModelEntity GetById(Guid id);
        void Update(FashionModelEntity entity);
    }
}