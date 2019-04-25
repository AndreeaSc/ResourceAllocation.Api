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

        public Designer RemoveFashionModelsDD (List<Artist> models, Guid idToRemove, Designer designer)
        {
            foreach (var artist in designer.AllocatedArtists)
            {
                if (idToRemove.Equals(designer.AllocatedArtists))
                {
                    designer.AllocatedArtists.Remove(artist);
                }
            }

            return designer;
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

        private static int getScore(Designer designer)
        {
            if (designer.FavoriteArtists != designer.AllocatedArtists)
            {
                designer.Score = designer.Score + designer.FavoriteArtists.Count - designer.AllocatedArtists.Count;
            }

            return designer.Score;
        }

        public List<Designer> AdjustedWinner(List<Designer> designers, List<CommonArtistEntity> commonArtists)
        {

            foreach (var commonArtist in commonArtists)
            {
                var firstDesigner = designers.First(x => x.Id == commonArtist.FirstDesigner);
                var firstDesignerArtistPosition = GetModelPosition(firstDesigner, commonArtist);

                var secondDesigner = designers.First(x => x.Id == commonArtist.SecondDesigner);
                var secondDesignerArtistPosition = GetModelPosition(secondDesigner, commonArtist);

                if (firstDesigner.DateTimeShow.Date.Equals(secondDesigner.DateTimeShow.Date))
                {
                    var commonModelsIds = firstDesigner.FavoriteArtists
                        .Where(x => secondDesigner.FavoriteArtists.Any(y => y.ArtistId == x.ArtistId))
                        .Select(x => x.ArtistId)
                        .ToList();

                    if (firstDesignerArtistPosition < secondDesignerArtistPosition)
                    {
                        List<Artist> artists = secondDesigner.FavoriteArtists.Select(x => x.Artist).ToList();
                        secondDesigner.AllocatedArtists = RemoveFashionModels(artists, commonModelsIds);
                    }
                    else if (firstDesignerArtistPosition > secondDesignerArtistPosition)
                    {
                        List<Artist> artists = firstDesigner.FavoriteArtists.Select(x => x.Artist).ToList();
                        firstDesigner.AllocatedArtists = RemoveFashionModels(artists, commonModelsIds);
                    }
                    else if (firstDesignerArtistPosition == secondDesignerArtistPosition)
                    {
                        if (getScore(firstDesigner) < getScore(firstDesigner))
                        {
                            List<Artist> artistsFirstDesigner = new List<Artist>();

                            List<DesignerArtists> allocatedArtists = firstDesigner.AllocatedArtists;
                            foreach (var artist in allocatedArtists)
                            {
                                artistsFirstDesigner.Add(artist.Artist);
                            }

                            firstDesigner.AllocatedArtists =
                                RemoveFashionModels(artistsFirstDesigner, commonModelsIds);
                        }
                        else if (getScore(firstDesigner) > getScore(secondDesigner))
                        {
                            List<Artist> artistsSecondDesigner = new List<Artist>();

                            List<DesignerArtists> allocatedArtists = secondDesigner.AllocatedArtists;
                            foreach (var artist in allocatedArtists)
                            {
                                artistsSecondDesigner.Add(artist.Artist);
                            }

                            secondDesigner.AllocatedArtists =
                                RemoveFashionModels(artistsSecondDesigner, commonModelsIds);
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

                            firstDesigner.AllocatedArtists =
                                RemoveFashionModels(artistsFirstDesigner, commonModelsIds);

                            secondDesigner.AllocatedArtists =
                                RemoveFashionModels(artistsSecondDesigner, commonModelsIds);
                        }
                    }
                }
            }
            return designers;
        }

        public List<Designer> DescendingDemand(List<Designer> designers, List<CommonArtistEntity> commonArtists)
        {

            foreach (var designer in designers)
            {
                designer.AllocatedArtists = designer.FavoriteArtists;
            }

            foreach (var commonArtist in commonArtists)
            {
                var firstDesigner = designers.First(x => x.Id == commonArtist.FirstDesigner);
                var secondDesigner = designers.First(x => x.Id == commonArtist.SecondDesigner);

                if (firstDesigner.DateTimeShow.Date.Equals(secondDesigner.DateTimeShow.Date))
                {
                    while (PartitionTesting(firstDesigner, secondDesigner) != 3)
                    {
                        RemoveArtistsPartition(firstDesigner);
                        RemoveArtistsPartition(secondDesigner);
                    }
                }
            }

            return designers;
        }

        public int PartitionTesting(Designer firstDesigner, Designer secondDesigner)
        {
            int partition = 0;
            bool stop = false;

            foreach (var artistOfFirstDesigner in firstDesigner.AllocatedArtists)
            {
                foreach (var artistOfSecondDesigner in secondDesigner.AllocatedArtists)
                {
                    if (artistOfSecondDesigner.ArtistId == artistOfFirstDesigner.ArtistId)
                    {
                        stop = true;
                        break;
                    }

                    if (stop == false) 
                    {
                        partition++;
                    }

                    if (partition == 3)
                    {
                        break;
                    }
                }

                if (partition == 3)
                {
                    break;
                }
            }

            if (firstDesigner.AllocatedArtists == null || secondDesigner.AllocatedArtists == null)
            {
                partition = 3;
            }

            return partition;
        }

        public Designer RemoveArtistsPartition(Designer designer)
        {
            int partitionForFirtsDesigner = 0;

            Designer designerCopy = new Designer();
            designerCopy.AllocatedArtists = designer.AllocatedArtists;

            foreach (var artistOfFirstDesigner in designerCopy.AllocatedArtists.ToList())
            {
                designer.AllocatedArtists.Remove(artistOfFirstDesigner);

                partitionForFirtsDesigner++;

                if (partitionForFirtsDesigner == 3)
                    break;
            }
            
            return designer;
        }

        public List<Designer> AllocateArtistsAlgorithm(List<Designer> designers, List<Artist> artist)
        {
            List<CommonArtistEntity> commonArtists = new List<CommonArtistEntity>();

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
                        GetCommonModels(firstDesigner, secondDesigner, commonArtists);
                    }
                }
            }

            //return AdjustedWinner(designers, commonArtists);
            return DescendingDemand(designers, commonArtists);
        }
    }
}


