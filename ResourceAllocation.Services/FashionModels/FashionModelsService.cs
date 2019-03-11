using ResourceAllocation.DataLayer.FashionModels;
using ResourceAllocation.Domain;
using System;
using System.Collections.Generic;
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

        public IEnumerable<FashionModelEntity> GetAll()
        {
            var result = _fashionModelsRepository.GetAll();
            return result;
        }

        public FashionModelEntity GetById(Guid id)
        {
            var result = _fashionModelsRepository.GetById(id);
            return result;
        }

        public void Add(FashionModelEntity entity)
        {
            _fashionModelsRepository.Add(entity);
        }

        public void Update(FashionModelEntity entity)
        {
            _fashionModelsRepository.Update(entity);
        }

        public void Delete(Guid id)
        {
            _fashionModelsRepository.Delete(id);
        }
    }
}
