using NewsReader.Data;
using NewsReader.Models;
using NewsReader.Repositories;
using NewsReader.ViewModels;
using Newtonsoft.Json;

namespace NewsReader.Services
{
    public class NewsService
    {
        private readonly IConfiguration _config;
        private readonly NewsRepository _newsRepository;

        public NewsService(IConfiguration config, NewsRepository newsRepository)
        {
            _config = config;
            _newsRepository = newsRepository;
        } 

        public void LoadArticles(NewsApiModel newsModel, string category)
        {
            _newsRepository.LoadArticles(newsModel, category);
        }

        public void PublishArticle(int categoryId, string name, string author, string title, string description, DateTime publishedAt, string content, string? url, string? urlToImage)
        {
            _newsRepository.PublishArticle(categoryId, name, author, title, description, publishedAt, content, url, urlToImage);
        }

        public void EditArticle(int id, int categoryId, string name, string author, string title, string description, DateTime publishedAt, string content, string? url, string? urlToImage)
        {
            _newsRepository.EditArticle(id, categoryId, name, author, title, description, publishedAt, content, url, urlToImage);
        }

        public void DeleteArticle(int id)
        {
            _newsRepository.DeleteArticle(id);
        }

        public Category GetCategory(string name)
        {
            return _newsRepository.GetCategory(name);
        }

        public Category GetCategory(int id)
        {
            return _newsRepository.GetCategory(id);
        } 

        public NewsApiModel? DownloadArticlesFromApi(string q)
        {   
            var key = _config["APIKey"];

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("user-agent", "News-API-csharp/0.1");
            httpClient.DefaultRequestHeaders.Add("x-api-key", key);

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"https://newsapi.org/v2/everything?q={q}&apiKey={key}");
            var response = httpClient.SendAsync(httpRequest).Result;

            if (response.IsSuccessStatusCode)
            {
                string responseBody = response.Content.ReadAsStringAsync().Result;
                var deserializedNews = JsonConvert.DeserializeObject<NewsApiModel>(responseBody);
                return deserializedNews;
            }

            return null;
        }

        public ArticleCategoryViewModel GetArticlesWithCategoriesModel() 
        {
            var model = new ArticleCategoryViewModel
            {
                Articles = new List<ArticleViewModel>(),
                Categories = new List<CategoryViewModel>()
            };

            var articlesFromDb = _newsRepository.GetAllArticles();
            var categories = _newsRepository.GetAllCategories();

            foreach (var category in categories)
            {
                model.Categories.Add(new CategoryViewModel 
                {
                    Id = category.Id,
                    CategoryName = category.CategoryName
                });
            }

            if (articlesFromDb.Count > 0)
            {
                foreach (var article in articlesFromDb)
                {
                    model.Articles.Add(new ArticleViewModel
                    {
                        Id = article.Id,
                        CategoryId = article.CategoryId,
                        Name = article.Name,
                        Author = article.Author,
                        Title = article.Title,
                        Description = article.Description,
                        PublishedAt = article.PublishedAt,
                        Content = article.Content,
                        Url = article.Url,
                        UrlToImage = article.UrlToImage
                    });
                }
            }

            return model;
        }

        public ArticleCategoryViewModel Search(string searchTerm)
        {
            var result = new ArticleCategoryViewModel
            {
                Articles = new List<ArticleViewModel>(),
                Categories = new List<CategoryViewModel>()
            };

            var articles = GetArticlesWithCategoriesModel();

            foreach (var category in articles.Categories)
            {
                result.Categories.Add(new CategoryViewModel 
                {
                    Id = category.Id,
                    CategoryName = category.CategoryName
                });
            }
            foreach (var article in articles.Articles)
            {
                if ((article.Title != null && article.Title.Contains(searchTerm)) 
                || (article.Description != null && article.Description.Contains(searchTerm)) 
                || (article.Author != null && article.Author.Contains(searchTerm))
                || (article.Content != null && article.Content.Contains(searchTerm))
                || (article.Name != null && article.Name.Contains(searchTerm)))
                {
                    result.Articles.Add(article);
                }
            }

            return result;
        }
    }
}