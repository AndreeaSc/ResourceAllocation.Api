using System;
using System.Collections.Generic;
using ResourceAllocation.Domain;

namespace ResourceAllocation.Services.Designers
{
    public interface IDesignersService
    {
        void Add(DesignerEntity entity);
        void Delete(Guid id);
        IEnumerable<DesignerEntity> GetAll();
        DesignerEntity GetById(Guid id);
        IEnumerable<FashionModelEntity> GetResultedModelsById(Guid id);
        void Update(DesignerEntity entity);
    }
}
