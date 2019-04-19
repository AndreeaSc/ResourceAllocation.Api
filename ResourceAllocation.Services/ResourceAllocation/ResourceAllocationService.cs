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
        
        public List<Designer> ExecuteAlgorithm(List<Designer> designers, List<Artist> fashionModel)
        {
            {
                List<CommonArtistEntity> commonFashionModels = new List<CommonArtistEntity>();

                foreach (var designer in designers)
                {
                    designer.AllocatedArtists = designer.FavoriteArtists;
                }

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
                        secondDesigner.AllocatedArtists = RemoveFashionModels(artists, commonModelsIds);
                        secondDesigner.Score = secondDesigner.Score + commonModelsIds.Count;
                    }
                    else if (firstDesignerModelPosition > secondDesignerModelPosition)
                    {
                        List<Artist> artists = firstDesigner.FavoriteArtists.Select(x => x.Artist).ToList();
                        firstDesigner.AllocatedArtists = RemoveFashionModels(artists, commonModelsIds);
                        firstDesigner.Score = secondDesigner.Score + commonModelsIds.Count;
                    }
                    else if (firstDesignerModelPosition == secondDesignerModelPosition)
                    {
                        if (firstDesigner.Score < secondDesigner.Score)
                        {
                            List<Artist> artistsFirstDesigner = new List<Artist>();

                            List<DesignerArtists> allocatedArtists = firstDesigner.AllocatedArtists;
                            foreach (var artist in allocatedArtists)
                            {
                                artistsFirstDesigner.Add(artist.Artist);
                            }
                            secondDesigner.AllocatedArtists =
                                RemoveFashionModels(artistsFirstDesigner, commonModelsIds);
                            firstDesigner.Score = secondDesigner.Score + commonModelsIds.Count;
                        }
                        else if (firstDesigner.Score > secondDesigner.Score)
                        {
                            List<Artist> artistsSecondDesigner = new List<Artist>();

                            List<DesignerArtists> allocatedArtists = secondDesigner.AllocatedArtists;
                            foreach (var artist in allocatedArtists)
                            {
                                artistsSecondDesigner.Add(artist.Artist);
                            }

                            firstDesigner.FavoriteArtists =
                                RemoveFashionModels(artistsSecondDesigner, commonModelsIds);
                            firstDesigner.Score = secondDesigner.Score + commonModelsIds.Count;
                        }
                        else
                        {
                            List<Artist> artistsFirstDesigner = new List<Artist>();

                            List<DesignerArtists> allocatedArtistsFirstDesinger = firstDesigner.AllocatedArtists;
                            foreach (var artist in allocatedArtistsFirstDesinger)
                            {
                                artistsFirstDesigner.Add(artist.Artist);
                            }

                            List<Artist> artistsSecondDesigner = new List<Artist>();

                            List<DesignerArtists> allocatedArtistsSeconDesigner = secondDesigner.AllocatedArtists;
                            foreach (var artist in allocatedArtistsSeconDesigner)
                            {
                                artistsSecondDesigner.Add(artist.Artist);
                            }

                            firstDesigner.FavoriteArtists =
                                RemoveFashionModels(artistsFirstDesigner, commonModelsIds);
                            firstDesigner.Score = secondDesigner.Score + commonModelsIds.Count;

                            secondDesigner.FavoriteArtists =
                                RemoveFashionModels(artistsSecondDesigner, commonModelsIds);
                            secondDesigner.Score = secondDesigner.Score + commonModelsIds.Count;
                        }
                    }
                }
                return designers;
            }
        }
    }
}

