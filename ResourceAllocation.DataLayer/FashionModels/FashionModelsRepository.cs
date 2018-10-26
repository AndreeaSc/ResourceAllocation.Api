using System;
using System.Collections.Generic;
using ResourceAllocation.Domain;

namespace ResourceAllocation.DataLayer.FashionModels
{
    public class FashionModelsRepository : IFashionModelsRepository
    {
        public IEnumerable<FashionModelEntity> GetAll()
        {
            return null;
        }

        public FashionModelEntity GetById(Guid id)
        {
            return null;
        }

        public void Add(FashionModelEntity entity)
        {

        }

        public void Update(FashionModelEntity entity)
        {

        }

        public void Delete(Guid id)
        {

        }
    }
}
