using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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

        public List<Designer> GetAll()
        {
            var result = _context.Designers.Include(designer => designer.FavoriteArtists).ThenInclude(x=>x.Artist).ToList();

            foreach (var designer in result)
            {
                designer.FavoriteArtists = designer.FavoriteArtists.OrderBy(x => x.Order).ToList();
            }

            return result;  
        }   

        public Designer GetById(Guid id)
        {
            var result = _context.Designers
                .Include(x => x.FavoriteArtists)
                .ThenInclude(a => a.Artist)
                .FirstOrDefault(x => x.Id == id);

            result.FavoriteArtists = result.FavoriteArtists.OrderBy(x => x.Order).ToList();
            return result;
        }

        public List<Artist> GetResultedModelsById(Guid id)
        {
            List<Designer> designers = new List<Designer>();
            List<Artist> artists = new List<Artist>();
            
            var designersAfterAlgorithm = ExecuteAlgorithm(designers, artists);

            var result = new List<Artist>();

            foreach (var designer in designersAfterAlgorithm)
            {
                if (designer.Id == id)
                    result = designer.FavoriteArtists.Select(x=>x.Artist).ToList();
            }
            
            return result;
        }

        public void Add(Designer entity)
        {
            entity.DateCreated = DateTime.Now.ToUniversalTime();
            _context.Designers.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Designer entity)
        {
            var dbEntity = _context.Designers.First(x => x.Id == entity.Id);
            dbEntity.Name = entity.Name;
            dbEntity.Mail = entity.Mail;
            dbEntity.Surname = entity.Surname;
            dbEntity.nrOfArtistsNeeded = entity.nrOfArtistsNeeded;
          //  dbEntity.FavoriteArtists = entity.FavoriteArtists;
            dbEntity.DateTimeShow = entity.DateTimeShow;
            dbEntity.LocationShow = entity.LocationShow;
            _context.Designers.Update(dbEntity);
            _context.SaveChanges();
        }

        public void SetArtists(Guid id, List<Guid> artistIds)
        {
            var designer = _context.Designers
                .Include(x => x.FavoriteArtists)
                .ThenInclude(a => a.Artist)
                .First(x => x.Id == id);

            designer.FavoriteArtists.Clear();
            var order = 0;
            foreach (var artistId in artistIds)
            {
                designer.FavoriteArtists.Add(new DesignerArtists
                {
                    ArtistId = artistId,    
                    DesignerId = id,
                    Order = order
                });

                order++;
            }
      
            _context.SaveChanges();
            var favoriteAritsts = _context.Designers.Find(id).FavoriteArtists;

        }

        public void Delete(Guid id)
        {
            var dbEntity = _context.Designers.First(x => x.Id == id);
            _context.Designers.Remove(dbEntity);
            _context.SaveChanges();
        }

        private static List<Guid> GetCommonModels(Designer designer, Designer otherDesigner, List<CommonArtistEntity> commonArtists)
        {
            var commonModelsIds = designer.FavoriteArtists
                .Where(x => otherDesigner.FavoriteArtists.Any(y => y.ArtistId == x.ArtistId))
                .Select(x => x.ArtistId)
                .ToList();

            foreach (var commonModelsId in commonModelsIds)
            {
                commonArtists.Add(new CommonArtistEntity()
                {
                    FirstDesigner = designer.Id,
                    SecondDesigner = otherDesigner.Id,
                    ArtistId = commonModelsId
                });
            }

            return commonModelsIds;
        }

        private static int GetModelPosition(Designer firstDesigner, CommonArtistEntity model)
        {
            for (int i = 0; i < firstDesigner.FavoriteArtists.Count; i++)
            {
                if (firstDesigner.FavoriteArtists[i].ArtistId == model.ArtistId)
                    return i;
            }

            return -1;
        }

        static List<DesignerArtists> RemoveFashionModels(List<Artist> models, List<Guid> idsToRemove)
        {
            List<DesignerArtists> result = new List<DesignerArtists>();

            List<Artist> filteredArtists = models.Where(x => !idsToRemove.Contains(x.Id)).ToList();
            foreach (var artist in filteredArtists)
            {
                result.AddRange(artist.FavoriteForDesigners);
            }

            return result;

        }

        private static List<Designer> ExecuteAlgorithm(List<Designer> designers, List<Artist> fashionModels)
        {
            return null;
        }
        }
    }

