using BenchmarkDotNet.Running;
using BenchmarkWebApp;

namespace Benchmark;

class Program
{
    static void Main(string[] args)
    {  
        var summary = BenchmarkRunner.Run<BenchmarkService>();

        //var service = new BenchmarkService();
        //service.RunEditArticle();
        //service.RunDeleteArticle();
    }
}

