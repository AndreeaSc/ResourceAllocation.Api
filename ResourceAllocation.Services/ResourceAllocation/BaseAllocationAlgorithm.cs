using ResourceAllocation.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResourceAllocation.Services.ResourceAllocation
{
    public class BaseAllocationAlgorithm
    {
        public static List<Guid> GetCommonModels(Designer designer, Designer otherDesigner, List<CommonArtistEntity> commonArtists)
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

        public int FinalScore(Designer designer)
        {
            int result = designer.FavoriteArtists.Count - designer.AllocatedArtists.Count;

            return result;
        }
    }
}
