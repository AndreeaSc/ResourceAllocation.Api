using System;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<DesignerEntity> GetAll()
        {
            var result = _context.Designers.ToList();

            return result;
        }

        public DesignerEntity GetById(Guid id)
        {
            var result = _context.Designers.FirstOrDefault(x => x.Id == id);
            return result;
        }

        public IEnumerable<FashionModelEntity> GetResultedModelsById(Guid id)
        {
            List<DesignerEntity> designers = new List<DesignerEntity>();
            List<FashionModelEntity> fashionModels = new List<FashionModelEntity>();
            
            var designersAfterAlgorithm = ExecuteAlgorithm(designers, fashionModels);

            List<FashionModelEntity> result = new List<FashionModelEntity>();

            foreach (var designer in designersAfterAlgorithm)
            {
                if (designer.Id == id)
                    result = designer.AllocatedFashionModels;
            }
            
            return result;
        }

        public void Add(DesignerEntity entity)
        {
            entity.DateCreated = DateTime.Now.ToUniversalTime();
            _context.Designers.Add(entity);
            _context.SaveChanges();
        }

        public void Update(DesignerEntity entity)
        {
            var dbEntity = _context.Designers.First(x => x.Id == entity.Id);
            dbEntity.Name = entity.Name;
            dbEntity.Mail = entity.Mail;
            dbEntity.Surname = entity.Surname;
            dbEntity.Password = entity.Password;
            dbEntity.FavoriteFashionModels = entity.FavoriteFashionModels;
            _context.Designers.Update(dbEntity);
            _context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var dbEntity = _context.Designers.First(x => x.Id == id);
            _context.Designers.Remove(dbEntity);
            _context.SaveChanges();
        }

        private static List<Guid> GetCommonModels(DesignerEntity designer, DesignerEntity otherDesigner, List<CommonFashionModelEntity> commonFashionModels)
        {
            var commonModelsIds = designer.FavoriteFashionModels
                .Where(x => otherDesigner.AllocatedFashionModels.Any(y => y.Id == x.Id))
                .Select(x => x.Id)
                .ToList();

            foreach (var commonModelsId in commonModelsIds)
            {
                commonFashionModels.Add(new CommonFashionModelEntity
                {
                    FirstDesigner = designer.Id,
                    SecondDesigner = otherDesigner.Id,
                    FashionModelId = commonModelsId
                });
            }

            return commonModelsIds;
        }

        private static int GetModelPosition(DesignerEntity firstDesigner, CommonFashionModelEntity model)
        {
            for (int i = 0; i < firstDesigner.FavoriteFashionModels.Count; i++)
            {
                if (firstDesigner.FavoriteFashionModels[i].Id == model.FashionModelId)
                    return i;
            }

            return -1;
        }

        static List<FashionModelEntity> RemoveFashionModels(List<FashionModelEntity> models, List<Guid> idsToRemove)
        {
            return models.Where(x => !idsToRemove.Contains(x.Id)).ToList();
        }

        private static int GetDesignerScore(DesignerEntity firstDesigner)
        {
            var result = 0;

            for (int i = 0; i < firstDesigner.AllocatedFashionModels.Count; i++)
            {
                if (firstDesigner.FavoriteFashionModels.Any(x => x.Id == firstDesigner.AllocatedFashionModels[i].Id))
                    result += firstDesigner.AllocatedFashionModels[i].Prioriy;
            }

            return result;
        }

        private static List<DesignerEntity> ExecuteAlgorithm(List<DesignerEntity> designers, List<FashionModelEntity> fashionModels)
        {
            List<CommonFashionModelEntity> commonFashionModels = new List<CommonFashionModelEntity>();

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

                var commonModelsIds = firstDesigner.AllocatedFashionModels
                    .Where(x => secondDesigner.AllocatedFashionModels.Any(y => y.Id == x.Id))
                    .Select(x => x.Id)
                    .ToList();

                if (firstDesignerModelPosition < secondDesignerModelPosition)
                {
                    secondDesigner.AllocatedFashionModels =
                        RemoveFashionModels(secondDesigner.AllocatedFashionModels, commonModelsIds);
                }
                else if (firstDesignerModelPosition > secondDesignerModelPosition)
                {
                    firstDesigner.AllocatedFashionModels =
                        RemoveFashionModels(firstDesigner.AllocatedFashionModels, commonModelsIds);
                }
                else if (firstDesignerModelPosition == secondDesignerModelPosition)
                {
                    var firstDesignerScore = GetDesignerScore(firstDesigner);
                    var secondDesignerScore = GetDesignerScore(secondDesigner);

                    if (firstDesignerScore < secondDesignerScore)
                    {
                        firstDesigner.AllocatedFashionModels =
                            RemoveFashionModels(firstDesigner.AllocatedFashionModels, commonModelsIds);
                    }
                    else if (firstDesignerScore > secondDesignerScore)
                    {
                        secondDesigner.AllocatedFashionModels =
                            RemoveFashionModels(secondDesigner.AllocatedFashionModels, commonModelsIds);
                    }
                    else
                    {
                        firstDesigner.AllocatedFashionModels =
                            RemoveFashionModels(firstDesigner.AllocatedFashionModels, commonModelsIds);
                        secondDesigner.AllocatedFashionModels =
                            RemoveFashionModels(secondDesigner.AllocatedFashionModels, commonModelsIds);
                    }
                }
            }
            return designers;
        }
    }
}
