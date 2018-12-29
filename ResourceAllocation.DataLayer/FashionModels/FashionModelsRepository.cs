using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResourceAllocation.Domain;

namespace ResourceAllocation.DataLayer.FashionModels
{
    public class FashionModelsRepository : IFashionModelsRepository
    {
        private readonly ResourceAllocationDbContext _context;

        public FashionModelsRepository(ResourceAllocationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FashionModelEntity>> GetAll()
        {
            var result = _context.FashionModels
                .OrderByDescending(x => x.DateCreated)
                .ToList();

            return result;
        }

        public async Task<FashionModelEntity> GetById(Guid id)
        {
            var result = _context.FashionModels.FirstOrDefault(x => x.Id == id);
            return result;
        }

        public async Task Add(FashionModelEntity entity)
        {
            entity.DateCreated = DateTime.Now.ToUniversalTime();
            _context.FashionModels.Add(entity);
            _context.SaveChanges();
        }

        public async Task Update(FashionModelEntity entity)
        {
            var dbEntity = _context.FashionModels.First(x => x.Id == entity.Id);
            dbEntity.Name = entity.Name;
            dbEntity.Height = entity.Height;
            dbEntity.Weight = entity.Weight;
            dbEntity.WaistSize = entity.WaistSize;
            dbEntity.HipsSize = entity.HipsSize;
            dbEntity.BreastSize = entity.BreastSize;
            dbEntity.EyesColor = entity.EyesColor;
            dbEntity.HairColor = entity.HairColor;
            dbEntity.Facebook = entity.Facebook;
            dbEntity.Instagram = entity.Instagram;
            dbEntity.Photo = entity.Photo;
            dbEntity.Description = entity.Description;
            _context.FashionModels.Update(dbEntity);
            _context.SaveChanges();
        }

        public async Task Delete(Guid id)
        {
        }
    }
}
