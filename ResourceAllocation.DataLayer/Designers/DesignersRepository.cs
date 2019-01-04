using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResourceAllocation.DataLayer.FashionModels;
using ResourceAllocation.Domain;

namespace ResourceAllocation.DataLayer.Designers
{
   public class DesignersRepository : IDesignersRepository
    {
        private readonly ResourceAllocationDbContext _context;

        public DesignersRepository(ResourceAllocationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DesignerEntity>> GetAll()
        {
            var result = _context.Designers.ToList();

            return result;
        }

        public async Task<DesignerEntity> GetById(Guid id)
        {
            var result = _context.Designers.FirstOrDefault(x => x.Id == id);
            return result;
        }

        public async Task Add(DesignerEntity entity)
        {
            entity.DateCreated = DateTime.Now.ToUniversalTime();
            _context.Designers.Add(entity);
            _context.SaveChanges();
        }

        public async Task Update(DesignerEntity entity)
        {
            var dbEntity = _context.Designers.First(x => x.Id == entity.Id);
            dbEntity.Name = entity.Name;
            dbEntity.Mail = entity.Mail;
            dbEntity.Surname = entity.Surname;
            _context.Designers.Update(dbEntity);
            _context.SaveChanges();
        }

        public async Task Delete(Guid id)
        {
            var dbEntity = _context.Designers.First(x => x.Id == id);
            _context.Designers.Remove(dbEntity);
            _context.SaveChanges();
        }
    }
}
