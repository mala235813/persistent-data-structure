using System.Collections.Immutable;
using System.Linq;
using BenchmarkDotNet.Attributes;
using PDS.Collections;
using PDS.Implementation.Collections;
using TunnelVisionLabs.Collections.Trees.Immutable;

namespace PDS.Benchmark
{
    public class ImmutableListBenchmark
    {
        [ShortRunJob]
        [JsonExporterAttribute.Brief]
        [JsonExporterAttribute.Full]
        [JsonExporterAttribute.BriefCompressed]
        [JsonExporterAttribute.FullCompressed]
        [RPlotExporter]
        [JsonExporter("-custom", indentJson: true, excludeMeasurements: true)]
        public class RangeToList
        {
            [Params(10, 1000 )]
            public int Count { get; set; }

            [Benchmark(Baseline = true, Description = "ImmutableList<T>")]
            public ImmutableList<int> List()
            {
                return ImmutableList.CreateRange(Enumerable.Range(0, Count));
            }

            [Benchmark(Description = "ImmutableArray<T>")]
            public ImmutableArray<int> Array()
            {
                return ImmutableArray.CreateRange(Enumerable.Range(0, Count));
            }

            [Benchmark(Description = "ImmutableTreeList<T>")]
            public ImmutableTreeList<int> TreeList()
            {
                return ImmutableTreeList.CreateRange(Enumerable.Range(0, Count));
            }
            
            [Benchmark(Description = "IPersistentLinkedList<T>")]
            public IPersistentLinkedList<int> LinkedList()
            {
                return new PersistentLinkedList<int>().AddRange(Enumerable.Range(0, Count));
            }
            
            //TODO: Add PersistentListRealisation
        }
    }
}
