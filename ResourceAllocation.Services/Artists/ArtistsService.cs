using ResourceAllocation.DataLayer.Artists;
using ResourceAllocation.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ResourceAllocation.Services.Artists
{
    public class ArtistsService : IArtistsService
    {
        private readonly IArtistsRepository _artistsRepository;

        public ArtistsService(IArtistsRepository artistsRepository)
        {
            _artistsRepository = artistsRepository;
        }

        public IEnumerable<Artist> GetAll()
        {
            var result = _artistsRepository.GetAll();
            return result;
        }

        public Artist GetById(Guid id)
        {
            var result = _artistsRepository.GetById(id);
            return result;
        }

        public void Add(Artist entity)
        {
            _artistsRepository.Add(entity);
        }

        public void Update(Artist entity)
        {
            _artistsRepository.Update(entity);
        }

        public void Delete(Guid id)
        {
            _artistsRepository.Delete(id);
        }
    }
}
