using ResourceAllocation.DataLayer.Artists;
using ResourceAllocation.DataLayer.Designers;
using ResourceAllocation.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResourceAllocation.Services.ResourceAllocation
{
    public class AdjustedWinnerAllocationService : BaseAllocationAlgorithm, IAdjustedWinnerAllocationService
    {
        private readonly IDesignersRepository _designersRepository;
        private readonly IArtistsRepository _artistsRepository;

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

            List<CommonArtistEntity> commonArtists = new List<CommonArtistEntity>();

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

            List<Designer> AWFinalScoreDesigners = new List<Designer>();

            var watch = System.Diagnostics.Stopwatch.StartNew();
            AWFinalScoreDesigners = AdjustedWinner(designers, commonArtists);
            watch.Stop();

            int AdjustedWinnerScore = 0;

            foreach (var designer in AWFinalScoreDesigners)
            {
                AdjustedWinnerScore += FinalScore(designer);
            }

            return new AlgorithmResult
            {
                Designers = AWFinalScoreDesigners,
                Score = AdjustedWinnerScore,
                TimeExecuted = watch.ElapsedMilliseconds
        };

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
                        foreach (var artist in secondDesigner.AllocatedArtists)
                        {
                            if (artist.ArtistId == commonArtist.ArtistId)
                            {
                                secondDesigner.AllocatedArtists.Remove(artist);
                            }
                        }
                    }
                    else if (firstDesignerArtistPosition > secondDesignerArtistPosition)
                    {
                        foreach (var artist in firstDesigner.AllocatedArtists)
                        {
                            if (artist.ArtistId == commonArtist.ArtistId)
                            {
                                firstDesigner.AllocatedArtists.Remove(artist);
                            }
                        }
                    }
                    else if (firstDesignerArtistPosition == secondDesignerArtistPosition)
                    {
                        if (getScore(firstDesigner) < getScore(firstDesigner))
                        {
                            List<Artist> artistsFirstDesigner = new List<Artist>();

                            foreach (var artist in firstDesigner.AllocatedArtists)
                            {
                                if (artist.ArtistId == commonArtist.ArtistId)
                                {
                                    firstDesigner.AllocatedArtists.Remove(artist);
                                }
                            }
                        }
                        else if (getScore(firstDesigner) > getScore(secondDesigner))
                        {
                            List<Artist> artistsSecondDesigner = new List<Artist>();

                            List<DesignerArtists> allocatedArtists = secondDesigner.AllocatedArtists;
                            foreach (var artist in allocatedArtists)
                            {
                                artistsSecondDesigner.Add(artist.Artist);
                            }

                            foreach (var artist in secondDesigner.AllocatedArtists)
                            {
                                if (artist.ArtistId == commonArtist.ArtistId)
                                {
                                    secondDesigner.AllocatedArtists.Remove(artist);
                                }
                            }
                        }
                        else
                        {
                            foreach (var artist in firstDesigner.AllocatedArtists.ToList())
                            {
                                if (artist.ArtistId == commonArtist.ArtistId)
                                {
                                    firstDesigner.AllocatedArtists.Remove(artist);
                                }
                            }

                            foreach (var artist in secondDesigner.AllocatedArtists.ToList())
                            {
                                if (artist.ArtistId == commonArtist.ArtistId)
                                {
                                    secondDesigner.AllocatedArtists.Remove(artist);
                                }
                            }
                        }
                    }
                }
            }
            return designers;
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
    }
}
