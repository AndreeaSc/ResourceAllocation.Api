using System;
using System.Collections.Generic;
using ResourceAllocation.Domain;

namespace ResourceAllocation.DataLayer.Designers
{
   public interface IDesignersRepository
    {
        void Add(Designer entity);
        void Delete(Guid id);
        IEnumerable<Designer> GetAll();
        Designer GetById(Guid id);
        void Update(Designer entity);
        void SetArtists(Guid id, List<Guid> artistIds);
    }
}
