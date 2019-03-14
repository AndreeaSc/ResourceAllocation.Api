using System;
using System.Collections.Generic;
using ResourceAllocation.Domain;

namespace ResourceAllocation.Services.Designers
{
    public interface IDesignersService
    {
        void Add(Designer entity);
        void Delete(Guid id);
        IEnumerable<Designer> GetAll();
        Designer GetById(Guid id);
        void Update(Designer entity);
        void SetFavouriteModels(Guid id, List<Guid> fashionModelIds);
    }
}
