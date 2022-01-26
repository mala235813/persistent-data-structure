using System.Collections.Immutable;
using System.Linq;
using BenchmarkDotNet.Attributes;
using Medallion.Collections;
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
            [Params(1000000)] public int Count { get; set; }

            [Benchmark(Baseline = true, Description = "ImmutableList")]
            public ImmutableList<int> List()
            {
                return ImmutableList.CreateRange(Enumerable.Range(0, Count));
            }

            [Benchmark(Description = "ImmutableArray")]
            public ImmutableArray<int> Array()
            {
                return ImmutableArray.CreateRange(Enumerable.Range(0, Count));
            }

            [Benchmark(Description = "ImmutableTreeList")]
            public ImmutableTreeList<int> TreeList()
            {
                return ImmutableTreeList.CreateRange(Enumerable.Range(0, Count));
            }
            
            [Benchmark(Description = "ImmutableLinkedList")]
            public ImmutableLinkedList<int> ImmutableLinkedList()
            {
                return ImmutableLinkedList<int>.Empty.AppendRange(Enumerable.Range(0, Count));
            }

            [Benchmark(Description = "PersistentLinkedList")]
            public IPersistentLinkedList<int> LinkedList()
            {
                return new PersistentLinkedList<int>().AddRange(Enumerable.Range(0, Count));
            }

            [Benchmark(Description = "PersistentList")]
            public IPersistentList<int> PersistentList()
            {
                return new PersistentList<int>().AddRange(Enumerable.Range(0, Count));
            }
        }


        [ShortRunJob]
        [JsonExporterAttribute.Brief]
        [JsonExporterAttribute.Full]
        [JsonExporterAttribute.BriefCompressed]
        [JsonExporterAttribute.FullCompressed]
        [RPlotExporter]
        [JsonExporter("-custom", indentJson: true, excludeMeasurements: true)]
        public class AggregateAdd
        {
            [Params(100000)] public int Count { get; set; }

            [Benchmark(Baseline = true, Description = "ImmutableList")]
            public ImmutableList<int> List()
            {
                return Enumerable.Range(0, Count)
                    .Aggregate(ImmutableList<int>.Empty, (current, item) => current.Add(item));
            }

            [Benchmark(Description = "ImmutableArray")]
            public ImmutableArray<int> Array()
            {
                return Enumerable.Range(0, Count)
                    .Aggregate(ImmutableArray<int>.Empty, (current, item) => current.Add(item));
            }

            [Benchmark(Description = "ImmutableTreeList")]
            public ImmutableTreeList<int> TreeList()
            {
                return Enumerable.Range(0, Count)
                    .Aggregate(ImmutableTreeList<int>.Empty, (current, item) => current.Add(item));
            }
            
            [Benchmark(Description = "ImmutableLinkedList")]
            public ImmutableLinkedList<int> ImmutableLinkedList()
            {
                return Enumerable.Range(0, Count)
                    .Aggregate(ImmutableLinkedList<int>.Empty, (current, item) => current.Append(item));
            }
            
            [Benchmark(Description = "PersistentLinkedList")]
            public IPersistentLinkedList<int> LinkedList()
            {
                return Enumerable.Range(0, Count)
                    .Aggregate(new PersistentLinkedList<int>(), (current, item) => current.PushBack(item));
            }

            [Benchmark(Description = "PersistentList")]
            public IPersistentList<int> PersistentList()
            {
                return Enumerable.Range(0, Count)
                    .Aggregate(new PersistentList<int>(), (current, item) => current.Add(item));
            }
        }
    }
}