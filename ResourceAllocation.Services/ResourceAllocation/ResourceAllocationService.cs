using System;
using System.Collections.Generic;
using System.Linq;
using ResourceAllocation.DataLayer.Designers;
using ResourceAllocation.Domain;

namespace ResourceAllocation.Services.ResourceAllocation
{
    public class ResourceAllocationService : IResourceAllocationService
    {
        private readonly IDesignersRepository _designersRepository;

        public ResourceAllocationService(IDesignersRepository designersRepository)
        {
            _designersRepository = designersRepository;
        }

        public List<Designer> ExecuteAlgorithm()
        {

            var designers = _designersRepository.GetAll();
            
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

                if (firstDesignerModelPosition < secondDesignerModelPosition)
                {
                    List<Artist> artists = secondDesigner.FavoriteArtists.Select(x => x.Artist).ToList();
                    secondDesigner.AllocatedArtists = RemoveFashionModels(artists, commonModelsIds, secondDesigner);
                }
                else if (firstDesignerModelPosition > secondDesignerModelPosition)
                {
                    List<Artist> artists = firstDesigner.FavoriteArtists.Select(x => x.Artist).ToList();
                    firstDesigner.AllocatedArtists = RemoveFashionModels(artists, commonModelsIds, firstDesigner);
                }
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
            return designers as List<Designer>;
        }

        private List<Guid> GetCommonModels(Designer firstDesigner, Designer secondDesigner, List<CommonArtistEntity> commonFashionModels)
        {
            var commonModelsIds = firstDesigner.FavoriteArtists
                            .Where(x => secondDesigner.FavoriteArtists.Any(y => y.ArtistId == x.ArtistId))
                            .Select(x => x.ArtistId)
                            .ToList();

            foreach (var commonModelsId in commonModelsIds)
            {
                commonFashionModels.Add(new CommonArtistEntity()
                {
                    FirstDesigner = firstDesigner.Id,
                    SecondDesigner = secondDesigner.Id,
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

        static List<DesignerArtists> RemoveFashionModels(List<Artist> models, List<Guid> idsToRemove, Designer designer)
        {
            return designer.FavoriteArtists.Where(x => !idsToRemove.Contains(x.ArtistId)).ToList();
        }

        //private static int GetDesignerScore(Designer designer)
        //{
        //    var result = 0;

        //    for (int i = 0; i < designer.FavoriteArtists.Count; i++)
        //    {
        //        if (designer.FavoriteArtists.Any(x => x.ArtistId == designer.FavoriteArtists[i].ArtistId))
        //            result += designer.FavoriteArtists[i].;
        //    }

        //    return result;
        //}

    }
}
