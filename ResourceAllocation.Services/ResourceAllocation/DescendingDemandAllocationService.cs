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

        private int noArtistsWanted = 3;

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
            List<DesignerArtists> artistsWantedTemp = new List<DesignerArtists>();

            foreach (var artist in designer.AllocatedArtists)
            {
                artistsWantedTemp.Add(artist);
                counter++;
                if (counter == noArtistsWanted)
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

        public List<Designer> DescendingDemand(List<Designer> designers, List<CommonArtistEntity> commonArtists)
        {
            foreach (var commonArtist in commonArtists)
            {
                var firstDesigner = designers.First(x => x.Id == commonArtist.FirstDesigner);
                var secondDesigner = designers.First(x => x.Id == commonArtist.SecondDesigner);

                if (firstDesigner.DateTimeShow.Date.Equals(secondDesigner.DateTimeShow.Date))
                {
                    while (PartitionTesting(firstDesigner, secondDesigner) != noArtistsWanted)
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

                    if (partition == noArtistsWanted)
                    {
                        break;
                    }
                }

                if (partition == noArtistsWanted)
                {
                    break;
                }
            }

            if (firstDesigner.AllocatedArtists == null || secondDesigner.AllocatedArtists == null)
            {
                partition = noArtistsWanted;
            }

            return partition;
        }

        public void RemoveArtistsPartition(Designer designer)
        {
            try
            {
                for (int i = 0; i < noArtistsWanted; i++)
                {
                    designer.AllocatedArtists.RemoveAt(0);
                }
            }
            catch (System.Exception)
            {

            }
        }
    }
}
