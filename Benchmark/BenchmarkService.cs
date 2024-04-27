using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Microsoft.Extensions.Configuration;
using NewsReader.Data;
using NewsReader.Models;
using NewsReader.Repositories;
using NewsReader.Services;

namespace BenchmarkWebApp
{
    [SimpleJob(RunStrategy.Monitoring, iterationCount: 1, invocationCount: 1)]
    public class BenchmarkService
    {
        private readonly NewsService _service;

        private readonly NewsRepository _repository;

        public BenchmarkService()
        {
            var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

            var dataContext = new DataContext(config);

            _repository = new NewsRepository(dataContext);
            _service = new NewsService(config, _repository);
        }

        [Benchmark]
        public void RunLoadArticles()
        {
            var news = new NewsApiModel
            {
                Status = "ok",
                TotalResults = 1000,
                Articles = new List<ArticleModel>()
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
                    PublishedAt = DateTime.Now.ToUniversalTime(),
                    Content = "Kolobok povesilsya"
                });
            }
            
            _service.LoadArticles(news, "business");
        }

        [Benchmark]
        public void RunGetAllArticles() => _repository.GetAllArticles();
        
        [Benchmark]
        public void RunGetAllCategories() => _repository.GetAllCategories();
        
        [Benchmark]
        public void RunGetArticlesWithCategoriesModel() => _service.GetArticlesWithCategoriesModel();
        
        [Benchmark]
        public void RunSearch() => _service.Search("тест");
        
        [Benchmark]
        public void RunPublishArticle() => _service.PublishArticle(1, "тест", "тест", "тест", "тест", DateTime.Now, "тест", "тест", "тест");

        [Benchmark]
        public void RunDeleteArticle() => _service.DeleteArticle(158134);

        [Benchmark]
        public void RunEditArticle() => _service.EditArticle(157133, 1, "тест", "тест", "тест", "тест", DateTime.Now, "тест", "тест", "тест");

        [Benchmark]
        public void RunGetCategoryByName() => _service.GetCategory("business");
        
        [Benchmark]
        public void RunGetCategoryById() => _service.GetCategory(1);
    }
}