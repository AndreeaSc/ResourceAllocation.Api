using System;
using System.Collections.Generic;
using ResourceAllocation.Domain;

namespace ResourceAllocation.Services.FashionModels
{
    public interface IFashionModelsService
    {
        void Add(Artist entity);
        void Delete(Guid id);
        IEnumerable<Artist> GetAll();
        Artist GetById(Guid id);
        void Update(Artist entity);
    }
}