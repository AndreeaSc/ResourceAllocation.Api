using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResourceAllocation.DataLayer.FashionModels;
using ResourceAllocation.DataLayer.Shows;
using ResourceAllocation.Domain;

namespace ResourceAllocation.DataLayer.Show
{
    public class ShowsRepository : IShowsRepository
    {
        private readonly ResourceAllocationDbContext _context;

        public ShowsRepository(ResourceAllocationDbContext context)
        {
            _context = context;
        }

        public async Task Add(ShowEntity entity)
        {
            entity.DateCreated = DateTime.Now.ToUniversalTime();
            _context.Shows.Add(entity);
            _context.SaveChanges();
        }

        public async Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ShowEntity>> GetAll()
        {
            var result = _context.Shows.ToList();

            return result;
        }

        public async Task<ShowEntity> GetById(Guid id)
        {
            var result = _context.Shows.FirstOrDefault(x => x.Id == id);
            return result;
        }

        public async Task Update(ShowEntity entity)
        {
            var dbEntity = _context.Shows.First(x => x.Id == entity.Id);
            dbEntity.Designer = entity.Designer;
            dbEntity.Date = entity.Date;
            _context.Shows.Update(dbEntity);
            _context.SaveChanges();
        }
    }
}