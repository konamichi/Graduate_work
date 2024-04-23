using BenchmarkDotNet.Attributes;
using NewsReader.Services;

namespace BenchmarkWebApp
{
    public class BenchmarkService
    {
        private readonly Service _service;

        public BenchmarkService()
        {
            
        }

        [Benchmark]
        public void RunDownloadArticlesFromApi() => _service.DownloadArticlesFromApi("tesla", "2024-01-01", "technology");
        
        [Benchmark]
        public void RunGetArticles() => _service.GetArticles();
        
        [Benchmark]
        public void RunSearch() => _service.Search("тест");
        
        [Benchmark]
        public void RunPublishArticle() => _service.PublishArticle(1, "тест", "тест", "тест", "тест", "01.01.0001 00:00:00", "тест");

        [Benchmark]
        public void RunDeleteArticle() => _service.DeleteArticle(1);

        [Benchmark]
        public void RunEditArticle() => _service.EditArticle(1, 1, "тест", "тест", "тест", "тест", "01.01.0001 00:00:00", "тест");

        [Benchmark]
        public void RunGetCategoryByName() => _service.GetCategory("business");
        
        [Benchmark]
        public void RunGetCategoryById() => _service.GetCategory(1);
    }
}