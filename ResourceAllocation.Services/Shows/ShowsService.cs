using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ResourceAllocation.DataLayer.Shows;
using ResourceAllocation.Domain;

namespace ResourceAllocation.Services.Show
{
    public class ShowsService : IShowsService
    {
        private readonly IShowsRepository _showsRepository;

        public ShowsService(IShowsRepository showsRepository)
        {
            _showsRepository = showsRepository;
        }

        public async Task Add(ShowEntity entity)
        {
            await _showsRepository.Add(entity);
        }

        public async Task Delete(Guid id)
        {
        }

        public async Task<IEnumerable<ShowEntity>> GetAll()
        {
            return await _showsRepository.GetAll();
        }

        public async Task<ShowEntity> GetById(Guid id)
        {
            return await _showsRepository.GetById(id);
        }

        public async Task Update(ShowEntity entity)
        {
            await _showsRepository.Update(entity);
        }
    }
}
