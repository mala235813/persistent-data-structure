using System.Threading;
using BenchmarkDotNet.Attributes;

namespace PDS.Benchmark
{
    [DryJob]
    [JsonExporterAttribute.Brief]
    [JsonExporterAttribute.Full]
    [JsonExporterAttribute.BriefCompressed]
    [JsonExporterAttribute.FullCompressed]
    [JsonExporter("-custom", indentJson: true, excludeMeasurements: true)]
    public class TestBench
    {
        [Params(10)]
        public int A { get; set; }

        [Params(10)]
        public int B { get; set; }

        [Benchmark]
        public void Benchmark()
        {
            Thread.Sleep(A + B + 5);
        }
    }
}