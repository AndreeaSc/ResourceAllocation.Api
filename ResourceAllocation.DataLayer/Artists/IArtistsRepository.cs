using System;
using System.Collections.Generic;
using ResourceAllocation.Domain;

namespace ResourceAllocation.DataLayer.Artists
{
    public interface IArtistsRepository
    {
        void Add(Artist entity);
        void Delete(Guid id);
        List<Artist> GetAll();
        Artist GetById(Guid id);
        void Update(Artist entity);
    }
}