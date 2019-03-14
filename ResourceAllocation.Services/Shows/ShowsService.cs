using System;
using System.Collections.Generic;
using ResourceAllocation.DataLayer.Shows;
using ResourceAllocation.Domain;

namespace ResourceAllocation.Services.Shows
{
    public class ShowsService : IShowsService
    {
        private readonly IShowsRepository _showsRepository;

        public ShowsService(IShowsRepository showsRepository)
        {
            _showsRepository = showsRepository;
        }

        public void Add(Show entity)
        {
            _showsRepository.Add(entity);
        }

        public void Delete(Guid id)
        {
            _showsRepository.Delete(id);
        }

        public IEnumerable<Show> GetAll()
        {
            var result = _showsRepository.GetAll();
            return result;
        }

        public Show GetById(Guid id)
        {
            var result = _showsRepository.GetById(id);
            return result;
        }

        public void Update(Show entity)
        {
            _showsRepository.Update(entity);
        }
    }
}
