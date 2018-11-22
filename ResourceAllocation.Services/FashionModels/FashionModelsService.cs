using ResourceAllocation.DataLayer.FashionModels;
using ResourceAllocation.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceAllocation.Services.FashionModels
{
    public class FashionModelsService : IFashionModelsService
    {
        private readonly IFashionModelsRepository _fashionModelsRepository;

        public FashionModelsService(IFashionModelsRepository fashionModelsRepository)
        {
            _fashionModelsRepository = fashionModelsRepository;
        }

        public async Task<IEnumerable<FashionModelEntity>> GetAll()
        {
            return await _fashionModelsRepository.GetAll();
        }

        public async Task<FashionModelEntity> GetById(Guid id)
        {
            return await _fashionModelsRepository.GetById(id);
        }

        public async Task Add(FashionModelEntity entity)
        {
            await _fashionModelsRepository.Add(entity);
        }

        public async Task Update(FashionModelEntity entity)
        {
            await _fashionModelsRepository.Update(entity);
        }

        public async Task Delete(Guid id)
        {

        }
    }
}
