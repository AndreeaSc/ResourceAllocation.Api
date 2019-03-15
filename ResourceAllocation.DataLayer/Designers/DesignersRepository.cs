using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ResourceAllocation.DataLayer.Artists;
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

        public IEnumerable<Designer> GetAll()
        {
            var result = _context.Designers.Include(designer => designer.FavoriteArtists).ThenInclude(x=>x.Artist).ToList();

            return result;  
        }   

        public Designer GetById(Guid id)
        {
            var result = _context.Designers
                .Include(x => x.FavoriteArtists)
                .ThenInclude(a => a.Artist)
                .FirstOrDefault(x => x.Id == id);
            return result;
        }

        public IEnumerable<Artist> GetResultedModelsById(Guid id)
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
            dbEntity.Password = entity.Password;
            dbEntity.FavoriteArtists = entity.FavoriteArtists;
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
            foreach (var artistId in artistIds)
            {
                designer.FavoriteArtists.Add(new DesignerArtists
                {
                    ArtistId = artistId,    
                    DesignerId = id
                });  
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

        static List<Artist> RemoveFashionModels(List<Artist> models, List<Guid> idsToRemove)
        {
            return models.Where(x => !idsToRemove.Contains(x.Id)).ToList();
        }

        private static int GetDesignerScore(Designer firstDesigner)
        {
            var result = 0;

            //for (int i = 0; i < firstDesigner.AllocatedFashionModels.Count; i++)
            //{
            //    if (firstDesigner.FavoriteFashionModels.Any(x => x.Id == firstDesigner.AllocatedFashionModels[i].Id))
            //        result += firstDesigner.AllocatedFashionModels[i].Prioriy;
            //}

            return result;
        }

        private static List<Designer> ExecuteAlgorithm(List<Designer> designers, List<Artist> fashionModels)
        {
            List<CommonArtistEntity> commonFashionModels = new List<CommonArtistEntity>();

            foreach (var firstDesigner in designers)
            {
                foreach (var secondDesigner in designers)
                {
                    if (firstDesigner.Id != secondDesigner.Id)
                    {
                        GetCommonModels(firstDesigner, secondDesigner, commonFashionModels);
                    }
                }
            }

            foreach (var commonModel in commonFashionModels)    
            {
                var firstDesigner = designers.First(x => x.Id == commonModel.FirstDesigner);
                var firstDesignerModelPosition = GetModelPosition(firstDesigner, commonModel);

                var secondDesigner = designers.First(x => x.Id == commonModel.SecondDesigner);
                var secondDesignerModelPosition = GetModelPosition(secondDesigner, commonModel);

                var commonModelsIds = firstDesigner.FavoriteArtists
                    .Where(x => secondDesigner.FavoriteArtists.Any(y => y.ArtistId == x.ArtistId))
                    .Select(x => x.ArtistId)
                    .ToList();

                //if (firstDesignerModelPosition < secondDesignerModelPosition)
                //{
                //    List<Artist> artists = secondDesigner.FavoriteArtists.Select(x => x.FavoriteArtist).ToList();
                //    secondDesigner.FavoriteArtists = RemoveFashionModels(artists, commonModelsIds);
                //}
                //else if (firstDesignerModelPosition > secondDesignerModelPosition)
                //{
                //    firstDesigner.FavoriteArtists =
                //        RemoveFashionModels(firstDesigner.FavoriteArtists.Select(x=>x.FavoriteArtistId), commonModelsIds);
                //}
                //else if (firstDesignerModelPosition == secondDesignerModelPosition)
                //{
                //    var firstDesignerScore = GetDesignerScore(firstDesigner);
                //    var secondDesignerScore = GetDesignerScore(secondDesigner);

                //    if (firstDesignerScore < secondDesignerScore)
                //    {
                //        firstDesigner.FavoriteArtists =
                //            RemoveFashionModels(firstDesigner.FavoriteArtists, commonModelsIds);
                //    }
                //    else if (firstDesignerScore > secondDesignerScore)
                //    {
                //        secondDesigner.FavoriteArtists =
                //            RemoveFashionModels(secondDesigner.FavoriteArtists, commonModelsIds);
                //    }
                //    else
                //    {
                //        firstDesigner.FavoriteArtists =
                //            RemoveFashionModels(firstDesigner.FavoriteArtists, commonModelsIds);
                //        secondDesigner.FavoriteArtists =
                //            RemoveFashionModels(secondDesigner.FavoriteArtists, commonModelsIds);
                //    }
                //}
            }
            return designers;
        }
    }
}
