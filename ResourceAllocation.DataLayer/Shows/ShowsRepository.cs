using System;
using System.Collections.Generic;
using System.Linq;
using ResourceAllocation.Domain;

namespace ResourceAllocation.DataLayer.Shows
{
    public class ShowsRepository : IShowsRepository
    {
        private readonly ResourceAllocationDbContext _context;

        public ShowsRepository(ResourceAllocationDbContext context)
        {
            _context = context;
        }

        public void Add(ShowEntity entity)
        {
            entity.DateCreated = DateTime.Now.ToUniversalTime();
            _context.Shows.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var dbEntity = _context.Shows.First(x => x.Id == id);
            _context.Shows.Remove(dbEntity);
            _context.SaveChanges();
        }

        public IEnumerable<ShowEntity> GetAll()
        {
            var result = _context.Shows.ToList();

            return result;
        }

        public ShowEntity GetById(Guid id)
        {
            var result = _context.Shows.FirstOrDefault(x => x.Id == id);
            return result;
        }

        public void Update(ShowEntity entity)
        {
            var dbEntity = _context.Shows.First(x => x.Id == entity.Id);
            dbEntity.Designer = entity.Designer;
            dbEntity.Date = entity.Date;
            dbEntity.FashionModelsName = entity.FashionModelsName;
            dbEntity.Location = entity.Location;
            _context.Shows.Update(dbEntity);
            _context.SaveChanges();
        }
    }
}