using System;
using System.Collections.Generic;
using ResourceAllocation.Domain;

namespace ResourceAllocation.Services.Shows
{
    public interface IShowsService
    {
        void Add(Show entity);
        void Delete(Guid id);
        IEnumerable<Show> GetAll();
        Show GetById(Guid id);
        void Update(Show entity);
    }
}
