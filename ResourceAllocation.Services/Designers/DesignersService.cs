using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task Add(DesignerEntity entity)
        {
            await _designersRepository.Add(entity);
        }

        public async Task Delete(Guid id)
        {
        }

        public async Task<IEnumerable<DesignerEntity>> GetAll()
        {
            return await _designersRepository.GetAll();
        }

        public async Task<DesignerEntity> GetById(Guid id)
        {
            return await _designersRepository.GetById(id);
        }

        public async Task Update(DesignerEntity entity)
        {
            await _designersRepository.Update(entity);
        }
    }
}
