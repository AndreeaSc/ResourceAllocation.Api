using ResourceAllocation.DataLayer.Artists;
using ResourceAllocation.DataLayer.Designers;
using ResourceAllocation.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ResourceAllocation.Services.ResourceAllocation
{
    public class AdjustedWinnerAllocationService : BaseAllocationAlgorithm, IAdjustedWinnerAllocationService
    {
        private readonly IDesignersRepository _designersRepository;
        private readonly IArtistsRepository _artistsRepository;

        int noArtistsWanted = 4;

        public AdjustedWinnerAllocationService(IDesignersRepository designersRepository, IArtistsRepository artistsRepository)
        {
            _designersRepository = designersRepository;
            _artistsRepository = artistsRepository;
        }

        public AlgorithmResult Execute()
        {
            var designers = _designersRepository.GetAll();
            SetInitialAllocatedArtists(designers);
            var artists = _artistsRepository.GetAll();

            var commonArtists = GetCommonModels(designers);

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = AdjustedWinner(designers, commonArtists);
            stopWatch.Stop();

            return new AlgorithmResult
            {
                Designers = result,
                Score = FinalScore(result),
                TimeExecuted = stopWatch.Elapsed.TotalMilliseconds
            };
        }

        private void alllocateNoOfNeededArtists(Designer designer)
        {
            int counter = 0;
            List<DesignerArtists> artistsWantedTemp = new List<DesignerArtists>();

            foreach (var artist in designer.AllocatedArtists)
            {
                artistsWantedTemp.Add(artist);
                counter++;
                if (counter == designer.nrOfArtistsNeeded)
                {
                    break;
                }
            }

            designer.AllocatedArtists.Clear();

            foreach (var artist in artistsWantedTemp)
            {
                designer.AllocatedArtists.Add(artist);
            }
        }

        private List<Designer> AdjustedWinner(List<Designer> designers, List<CommonArtistEntity> commonArtists)
        {
            foreach (var commonArtist in commonArtists)
            {
                var firstDesigner = designers.First(x => x.Id == commonArtist.FirstDesigner);
                var firstDesignerArtistPosition = GetModelPosition(firstDesigner, commonArtist);

                var secondDesigner = designers.First(x => x.Id == commonArtist.SecondDesigner);
                var secondDesignerArtistPosition = GetModelPosition(secondDesigner, commonArtist);

                if (firstDesigner.DateTimeShow.Date.Equals(secondDesigner.DateTimeShow.Date))
                {
                    if (firstDesignerArtistPosition < secondDesignerArtistPosition)
                    {
                        RemoveCommonArtistFromDesigner(secondDesigner, commonArtist);
                    }
                    else if (firstDesignerArtistPosition > secondDesignerArtistPosition)
                    {
                        RemoveCommonArtistFromDesigner(firstDesigner, commonArtist);
                    }
                    else if (firstDesignerArtistPosition == secondDesignerArtistPosition)
                    {
                        if (GetDesignerScore(firstDesigner) < GetDesignerScore(firstDesigner))
                        {
                            RemoveCommonArtistFromDesigner(firstDesigner, commonArtist);
                        }
                        else if (GetDesignerScore(firstDesigner) > GetDesignerScore(secondDesigner))
                        {
                            RemoveCommonArtistFromDesigner(secondDesigner, commonArtist);
                        }
                        else
                        {
                            RemoveCommonArtistFromDesigner(firstDesigner, commonArtist);
                            RemoveCommonArtistFromDesigner(secondDesigner, commonArtist);
                        }
                    }

                    //    alllocateNoOfNeededArtists(firstDesigner);
                    //    alllocateNoOfNeededArtists(secondDesigner);
                    //}
                    //else
                    //{
                    //    alllocateNoOfNeededArtists(firstDesigner);
                    //    alllocateNoOfNeededArtists(secondDesigner);
                    //}
                }
            }

            foreach (var designer in designers)
            {
                alllocateNoOfNeededArtists(designer);
            }

            return designers;
        }

        private void RemoveCommonArtistFromDesigner(Designer designer, CommonArtistEntity commonArtist)
        {
            designer.AllocatedArtists = designer.AllocatedArtists
                .Where(x => x.ArtistId != commonArtist.ArtistId)
                .ToList();
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

        private int GetDesignerScore(Designer designer)
        {
            return designer.AllocatedArtists.Sum(x => x.Order);
        }
    }
}
