using System;
using System.Collections.Generic;
using ResourceAllocation.Domain;

namespace ResourceAllocation.DataLayer.FashionModels
{
    public interface IFashionModelsRepository
    {
        void Add(Artist entity);
        void Delete(Guid id);
        IEnumerable<Artist> GetAll();
        Artist GetById(Guid id);
        void Update(Artist entity);
    }
}