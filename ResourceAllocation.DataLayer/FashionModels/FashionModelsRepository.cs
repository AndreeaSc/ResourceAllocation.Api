using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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

        public IEnumerable<Artist> GetAll()
        {
            var result = _context.Artists.Include(x=>x.FavoriteForDesigners).ThenInclude(x=>x.Designer)
                .OrderByDescending(x => x.DateCreated)
                .ToList();

            return result;
        }

        public Artist GetById(Guid id)
        {
            var result = _context.Artists.FirstOrDefault(x => x.Id == id);
            return result;
        }

        public void Add(Artist entity)
        {
            entity.DateCreated = DateTime.Now.ToUniversalTime();
            _context.Artists.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Artist entity)
        { 
            _context.Set<Artist>().Update(entity); 
            _context.SaveChanges();
        }

       

        public void Delete(Guid id)
        {
            var dbEntity = _context.Artists.FirstOrDefault(x => x.Id == id);

            if (dbEntity == null)
                return;

            _context.Artists.Remove(dbEntity);
            _context.SaveChanges();
        }
    }
}
