using System;
using System.Collections.Generic;
using ResourceAllocation.Domain;

namespace ResourceAllocation.DataLayer.Shows
{
    public interface IShowsRepository
    {
        void Add(Show entity);
        void Delete(Guid id);
        IEnumerable<Show> GetAll();
        Show GetById(Guid id);
        void Update(Show entity);
    }
}
