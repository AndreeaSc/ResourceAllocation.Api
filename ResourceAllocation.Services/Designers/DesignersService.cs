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

        public void Add(DesignerEntity entity)
        {
             _designersRepository.Add(entity);
        }

        public void Delete(Guid id)
        {
            _designersRepository.Delete(id);
        }

        public IEnumerable<DesignerEntity> GetAll()
        {
            var result = _designersRepository.GetAll();
            return result;
        }

        public  DesignerEntity GetById(Guid id)
        {
            var result = _designersRepository.GetById(id);
            return result;
        }

        public IEnumerable<FashionModelEntity> GetResultedModelsById(Guid id)
        {
            var result = _designersRepository.GetResultedModelsById(id);
            return result;
        }
        
        public void Update(DesignerEntity entity)
        {
            _designersRepository.Update(entity);
        }
    }
}
