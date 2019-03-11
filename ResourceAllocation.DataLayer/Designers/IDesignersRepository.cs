using System;
using System.Collections.Generic;
using ResourceAllocation.Domain;

namespace ResourceAllocation.DataLayer.Designers
{
   public interface IDesignersRepository
    {
        void Add(DesignerEntity entity);
        void Delete(Guid id);
        IEnumerable<DesignerEntity> GetAll();
        DesignerEntity GetById(Guid id);
        IEnumerable<FashionModelEntity> GetResultedModelsById(Guid idGuid);
        void Update(DesignerEntity entity);
    }
}
