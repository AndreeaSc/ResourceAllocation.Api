using ResourceAllocation.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResourceAllocation.Services.ResourceAllocation
{
    public class BaseAllocationAlgorithm
    {
        public static List<CommonArtistEntity> GetCommonModels(List<Designer> designers)
        {
            var commonArtists = new List<CommonArtistEntity>();
            foreach (var firstDesigner in designers)
            {
                foreach (var secondDesigner in designers)
                {
                    if (firstDesigner.Id != secondDesigner.Id)
                    {
                        var commonModelsIds = firstDesigner.FavoriteArtists
                            .Where(x => secondDesigner.FavoriteArtists.Any(y => y.ArtistId == x.ArtistId))
                            .Select(x => x.ArtistId)
                            .ToList();

                        foreach (var commonModelsId in commonModelsIds)
                        {
                            commonArtists.Add(new CommonArtistEntity()
                            {
                                FirstDesigner = firstDesigner.Id,
                                SecondDesigner = secondDesigner.Id,
                                ArtistId = commonModelsId
                            });
                        }
                    }
                }
            }

            return commonArtists;
        }

        public void SetInitialAllocatedArtists(List<Designer> designers)
        {
            foreach (var designer in designers)
            {
                designer.AllocatedArtists = new List<DesignerArtists>();
                foreach (var designerFavoriteArtist in designer.FavoriteArtists)
                {
                    designer.AllocatedArtists.Add(designerFavoriteArtist);
                }
            }
        }

        public int FinalScore(List<Designer> designers)
        {
            int result = 0;

            foreach (var designer in designers)
            {
                result += designer.AllocatedArtists.Sum(x => x.Order);
            }

            return result;
        }
    }
}
