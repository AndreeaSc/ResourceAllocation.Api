using System;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<FashionModelEntity> GetAll()
        {
            var result = _context.FashionModels
                .OrderByDescending(x => x.DateCreated)
                .ToList();

            return result;
        }

        public FashionModelEntity GetById(Guid id)
        {
            var result = _context.FashionModels.FirstOrDefault(x => x.Id == id);
            return result;
        }

        public void Add(FashionModelEntity entity)
        {
            entity.DateCreated = DateTime.Now.ToUniversalTime();
            _context.FashionModels.Add(entity);
            _context.SaveChanges();
        }

        public void Update(FashionModelEntity entity)
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
            dbEntity.Gender = entity.Gender;
            dbEntity.Description = entity.Description;
            _context.FashionModels.Update(dbEntity);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var dbEntity = _context.FashionModels.FirstOrDefault(x => x.Id == id);

            if (dbEntity == null)
                return;

            _context.FashionModels.Remove(dbEntity);
            _context.SaveChanges();
        }
    }
}
