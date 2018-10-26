using System;
using System.Collections.Generic;
using System.Linq;
using ResourceAllocation.DataLayer.FashionModels;
using ResourceAllocation.Domain;

namespace ResourceAllocation.Services.FashionModels
{
    public class FashionModelsService : IFashionModelsService
    {
        private readonly IFashionModelsRepository _fashionModelsRepository;
        private static List<FashionModelEntity> _fashionModelEntities = new List<FashionModelEntity>();

        public FashionModelsService(IFashionModelsRepository fashionModelsRepository)
        {
            _fashionModelsRepository = fashionModelsRepository;
        }

        public IEnumerable<FashionModelEntity> GetAll()
        {
            return _fashionModelEntities;
        }

        public FashionModelEntity GetById(Guid id)
        {
            return _fashionModelEntities.FirstOrDefault(x => x.Id == id);
        }

        public void Add(FashionModelEntity entity)
        {
            _fashionModelEntities.Add(entity);
        }

        public void Update(FashionModelEntity entity)
        {
            var item = _fashionModelEntities.FirstOrDefault(x => x.Id == entity.Id);
            if (item != null)
            {
                item.Name = entity.Name;
            }
        }

        public void Delete(Guid id)
        {
            var items = _fashionModelEntities.Where(x => x.Id != id).ToList();
            _fashionModelEntities = items;
        }
    }
}
