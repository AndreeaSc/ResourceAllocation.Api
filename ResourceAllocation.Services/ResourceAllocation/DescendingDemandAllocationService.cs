using ResourceAllocation.DataLayer.Artists;
using ResourceAllocation.DataLayer.Designers;
using ResourceAllocation.Domain;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;

namespace ResourceAllocation.Services.ResourceAllocation
{
    public class DescendingDemandAllocationService : BaseAllocationAlgorithm, IDescendingDemandAllocationService
    {
        private readonly IDesignersRepository _designersRepository;
        private readonly IArtistsRepository _artistsRepository;

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

        private List<DesignerArtists> alllocateNoOfNeededArtists(Designer designer)
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

            return designer.AllocatedArtists;
        }

        public List<Designer> DescendingDemand(List<Designer> designers, List<CommonArtistEntity> commonArtists)
        {
            foreach (var commonArtist in commonArtists)
            {
                var firstDesigner = designers.First(x => x.Id == commonArtist.FirstDesigner);
                var secondDesigner = designers.First(x => x.Id == commonArtist.SecondDesigner);

                int smallestPartition = SmallestNoOfArtistsNeeded(firstDesigner, secondDesigner);

                if (firstDesigner.DateTimeShow.Date.Equals(secondDesigner.DateTimeShow.Date))
                {
                    while (PartitionTesting(firstDesigner, secondDesigner) != smallestPartition && firstDesigner.AllocatedArtists.Count !=0 && secondDesigner.AllocatedArtists.Count != 0)
                    {
                        RemoveArtistsPartition(firstDesigner);
                        RemoveArtistsPartition(secondDesigner);
                    }

                    secondDesigner.AllocatedArtists = alllocateNoOfNeededArtists(secondDesigner);
                    firstDesigner.AllocatedArtists = alllocateNoOfNeededArtists(firstDesigner);
                }
                else
                {
                    secondDesigner.AllocatedArtists = alllocateNoOfNeededArtists(secondDesigner);
                    firstDesigner.AllocatedArtists = alllocateNoOfNeededArtists(firstDesigner);
                }
            
            }

            return designers;
        }

        public int SmallestNoOfArtistsNeeded(Designer firstDesigner, Designer secondDesigner)
        {
            int smallestPartition;

            if (firstDesigner.nrOfArtistsNeeded <= secondDesigner.nrOfArtistsNeeded)
            {
                smallestPartition = firstDesigner.nrOfArtistsNeeded;
            }
            else
            {
                smallestPartition = secondDesigner.nrOfArtistsNeeded;
            }

            return smallestPartition;
        }

        public int PartitionTesting(Designer firstDesigner, Designer secondDesigner)
        {
            int partitionCount = 0;
            int smallestPartition = SmallestNoOfArtistsNeeded(firstDesigner, secondDesigner);

            foreach (var artist in firstDesigner.AllocatedArtists)
            {
                foreach (var otherArtist in secondDesigner.AllocatedArtists)
                {
                    if (artist.ArtistId != otherArtist.ArtistId && partitionCount != smallestPartition)
                    {
                        partitionCount++;
                    }
                    else
                    {
                        return partitionCount;
                    }
                }
            }

            return partitionCount;
        }

        public void RemoveArtistsPartition(Designer designer)
        {
            for (int i = 0; i < noArtistsWanted; i++)
            {
                if (designer.AllocatedArtists.Count != 0)
                {
                    designer.AllocatedArtists.RemoveAt(0);
                }
            }

        }
    }
}