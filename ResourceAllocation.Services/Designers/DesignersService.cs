using System;
using System.Collections.Generic;
using ResourceAllocation.DataLayer.Designers;
using ResourceAllocation.Domain;

namespace ResourceAllocation.Services.Designers
{
    public class DesignersService : IDesignersService
    {
        private readonly IDesignersRepository _designersRepository;

        public DesignersService(IDesignersRepository designersRepository)
        {
            _designersRepository = designersRepository;
        }

        public void Add(Designer entity)
        {
             _designersRepository.Add(entity);
        }

        public void Delete(Guid id)
        {
            _designersRepository.Delete(id);
        }

        public IEnumerable<Designer> GetAll()
        {
            var result = _designersRepository.GetAll();
            return result;
        }

        public  Designer GetById(Guid id)
        {
            var result = _designersRepository.GetById(id);
            return result;
        }
        
        public void Update(Designer entity)
        {
            _designersRepository.Update(entity);
        }

        public void SetFavouriteArtists(Guid id, List<Guid> artistIds)
        {
            _designersRepository.SetArtists(id, artistIds);
        }
    }
}
