using ResourceAllocation.DataLayer.Artists;
using ResourceAllocation.DataLayer.Designers;
using ResourceAllocation.Domain;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace ResourceAllocation.Services.ResourceAllocation
{
    public class DescendingDemandAllocationService : BaseAllocationAlgorithm, IDescendingDemandAllocationService
    {
        private readonly IDesignersRepository _designersRepository;
        private readonly IArtistsRepository _artistsRepository;

        // private int noArtistsWanted = 4;

        public DescendingDemandAllocationService(IDesignersRepository designersRepository, IArtistsRepository artistsRepository)
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
            var result = DescendingDemand(designers, commonArtists);
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
            List<DesignerArtists> artistsNeededTemp = new List<DesignerArtists>();

            foreach (var artist in designer.AllocatedArtists)
            {
                artistsNeededTemp.Add(artist);
                counter++;
                if (counter == designer.nrOfArtistsNeeded)
                {
                    break;
                }
            }

            designer.AllocatedArtists.Clear();

            foreach (var artist in artistsNeededTemp)
            {
                designer.AllocatedArtists.Add(artist);
            }
        }

        public List<Designer> DescendingDemand(List<Designer> designers, List<CommonArtistEntity> commonArtists)
        {
            foreach (var commonArtist in commonArtists)
            {
                var firstDesigner = designers.First(x => x.Id == commonArtist.FirstDesigner);
                var secondDesigner = designers.First(x => x.Id == commonArtist.SecondDesigner);

                if (firstDesigner.DateTimeShow.Date.Equals(secondDesigner.DateTimeShow.Date))
                {
                    while (PartitionTesting(firstDesigner, secondDesigner) != noArtstsWantedOfDesigners(firstDesigner, secondDesigner))
                    {
                        RemoveArtistsPartition(firstDesigner);
                        RemoveArtistsPartition(secondDesigner);
                    }

                    alllocateNoOfNeededArtists(firstDesigner);
                }
                else
                {
                    alllocateNoOfNeededArtists(firstDesigner);
                }
            }

            return designers;
        }

        public int noArtstsWantedOfDesigners(Designer firstDesigner, Designer secondDesigner)
        {
            int noArtistsWantedMin;

            if (firstDesigner.nrOfArtistsNeeded <= secondDesigner.nrOfArtistsNeeded)
            {
                noArtistsWantedMin = firstDesigner.nrOfArtistsNeeded;
            }
            else
            {
                noArtistsWantedMin = secondDesigner.nrOfArtistsNeeded;
            }

            return noArtistsWantedMin;
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

                    if (partition == noArtstsWantedOfDesigners(firstDesigner, secondDesigner))
                    {
                        break;
                    }
                }

                if (partition == noArtstsWantedOfDesigners(firstDesigner, secondDesigner))
                {
                    break;
                }
            }

            if (firstDesigner.AllocatedArtists == null || secondDesigner.AllocatedArtists == null)
            {
                partition = noArtstsWantedOfDesigners(firstDesigner, secondDesigner);
            }

            return partition;
        }

        public void RemoveArtistsPartition(Designer designer)
        {
            for (int i = 0; i < designer.nrOfArtistsNeeded; i++)
            {
                if (designer.AllocatedArtists.Count != 0)
                {
                    designer.AllocatedArtists.RemoveAt(0);
                }
            }

        }
    }
}
