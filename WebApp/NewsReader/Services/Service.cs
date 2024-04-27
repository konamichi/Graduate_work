using NewsReader.Data;
using NewsReader.Models;
using NewsReader.ViewModels;
using Newtonsoft.Json;

namespace NewsReader.Services
{
    public class Service
    {
        private readonly IConfiguration _config;
        private readonly DataContext _dataContext;

        public Service(IConfiguration config, DataContext dataContext)
        {
            _config = config;
            _dataContext = dataContext;
        }

        public void LoadArticles(NewsApiModel newsModel, string category)
        {
            foreach (var article in newsModel.Articles)
            {
                var existCategory = GetCategory(category);

                var articleToDb = new Article
                {
                    CategoryId = existCategory.Id,
                    Name = article.Source.Name,
                    Author = article.Author,
                    Title = article.Title,
                    Description = article.Description,
                    PublishedAt = article.PublishedAt,
                    Content = article.Content,
                    Url = article.Url,
                    UrlToImage = article.UrlToImage
                };

                _dataContext.Articles.Add(articleToDb);
                _dataContext.SaveChanges();
            }
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

        public ArticleCategoryViewModel GetArticlesWithCategories() 
        {
            var model = new ArticleCategoryViewModel
            {
                Articles = new List<ArticleViewModel>(),
                Categories = new List<CategoryViewModel>()
            };

            var articlesFromDb = _dataContext.Articles.ToList();
            var categories = _dataContext.Categories.ToList();

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

            var articles = GetArticlesWithCategories();

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

        public void PublishArticle(int categoryId, string name, string author, string title, string description, DateTime publishedAt, string content, string? url, string? urlToImage)
        {
            var articleToDb = new Article
            {
                CategoryId = categoryId,
                Name = name,
                Author = author,
                Title = title,
                Description = description,
                PublishedAt = publishedAt.ToUniversalTime(),
                Content = content,
                Url = url,
                UrlToImage = urlToImage
            };

            _dataContext.Articles.Add(articleToDb);
            _dataContext.SaveChanges();
        }

        public void DeleteArticle(int id)
        {
            var existArticle = _dataContext.Articles.First(a => a.Id == id);

            _dataContext.Articles.Remove(existArticle);
            _dataContext.SaveChanges();      
        }

        public void EditArticle(int id, int categoryId, string name, string author, string title, string description, DateTime publishedAt, string content, string? url, string? urlToImage)
        {
            var existArticle = _dataContext.Articles.First(a => a.Id == id);
            existArticle.CategoryId = categoryId;
            existArticle.Name = name;
            existArticle.Author = author;
            existArticle.Title = title;
            existArticle.Description = description;
            existArticle.PublishedAt = publishedAt.ToUniversalTime();
            existArticle.Content = content;
            existArticle.Url = url;
            existArticle.UrlToImage = urlToImage;

            _dataContext.SaveChanges();
        }

        public Category GetCategory(string name) 
        {
            var existCategory = _dataContext.Categories.First(c => c.CategoryName == name);

            return existCategory;
        }

        public Category GetCategory(int id)
        {
            var existCategory = _dataContext.Categories.First(c => c.Id == id);

            return existCategory;
        }
    }
}