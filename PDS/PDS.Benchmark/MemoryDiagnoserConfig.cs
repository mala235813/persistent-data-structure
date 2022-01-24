using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;

namespace PDS.Benchmark
{
    public class MemoryDiagnoserConfig : ManualConfig
    {
        public MemoryDiagnoserConfig()
        {
            Add(MemoryDiagnoser.Default);
        }
    }
}
