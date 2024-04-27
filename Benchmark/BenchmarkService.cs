using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Microsoft.Extensions.Configuration;
using NewsReader.Data;
using NewsReader.Models;
using NewsReader.Services;

namespace BenchmarkWebApp
{
    [SimpleJob(RunStrategy.Monitoring, iterationCount: 1, invocationCount: 1)]
    public class BenchmarkService
    {
        private readonly Service _service;

        public BenchmarkService()
        {
            var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

            var dataContext = new DataContext(config);

            _service = new Service(config, dataContext);
        }

        [Benchmark]
        public void RunLoadArticles()
        {
            var news = new NewsApiModel
            {
                Status = "ok",
                TotalResults = 1000
            };

            for (int i = 0; i < 1000; i++)
            {
                news.Articles.Add(new ArticleModel
                {
                    Source = new SourceModel
                    {
                        Id = null,
                        Name = "some source.com"
                    },
                    Author = "Julia",
                    Title = "Breaking news",
                    Description = "Not breaking news actually",
                    Url = "http://panorama.ru",
                    UrlToImage = null,
                    PublishedAt = DateTime.Now,
                    Content = "Kolobok povesilsya"
                });
            }
            
            _service.LoadArticles(news, "business");
        }
        
        [Benchmark]
        public void RunGetArticles() => _service.GetArticlesWithCategories();
        
        [Benchmark]
        public void RunSearch() => _service.Search("тест");
        
        [Benchmark]
        public void RunPublishArticle() => _service.PublishArticle(1, "тест", "тест", "тест", "тест", "01.01.0001 00:00:00", "тест", "тест", "тест");

        [Benchmark]
        public void RunDeleteArticle() => _service.DeleteArticle(151123);

        [Benchmark]
        public void RunEditArticle() => _service.EditArticle(152124, 1, "тест", "тест", "тест", "тест", "01.01.0001 00:00:00", "тест", "тест", "тест");

        [Benchmark]
        public void RunGetCategoryByName() => _service.GetCategory("business");
        
        [Benchmark]
        public void RunGetCategoryById() => _service.GetCategory(1);
    }
}