using ResourceAllocation.DataLayer.Artists;
using ResourceAllocation.DataLayer.Designers;
using ResourceAllocation.Domain;
using System.Collections.Generic;
using System.Diagnostics;
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

        public List<Designer> DescendingDemand(List<Designer> designers, List<CommonArtistEntity> commonArtists)
        {
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

        public void RemoveArtistsPartition(Designer designer)
        {
            try
            {
                designer.AllocatedArtists.RemoveAt(0);
                designer.AllocatedArtists.RemoveAt(0);
                designer.AllocatedArtists.RemoveAt(0);
            }
            catch (System.Exception)
            {
                
            }
        }
    }
}
