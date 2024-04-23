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

        public void DownloadArticlesFromApi(string q, string from, string category)
        {   
            var key = _config["APIKey"];

            using var httpClient = new HttpClient();
            var response = httpClient.GetAsync($"https://newsapi.org/v2/everything?q={q}&from={from}&sortBy=publishedAt&apiKey={key}").Result;

            if (response.IsSuccessStatusCode)
            {
                string responseBody = response.Content.ReadAsStringAsync().Result;
                var deserializedArticles = JsonConvert.DeserializeObject<List<ApiArticleModel>>(responseBody);
                foreach (var deserializedArticle in deserializedArticles)
                {
                    var existCategory = GetCategory(category);

                    var articleToDb = new Article
                    {
                        CategoryId = existCategory.Id,
                        Name = deserializedArticle.Source.Name,
                        Author = deserializedArticle.Author,
                        Title = deserializedArticle.Title,
                        Description = deserializedArticle.Description,
                        PublishedAt = deserializedArticle.PublishedAt.ToString(),
                        Content = deserializedArticle.Content
                    };

                    _dataContext.Articles.Add(articleToDb);
                    _dataContext.SaveChanges();
                }
            }
        }

        public ArticleCategoryViewModel GetArticles() 
        {
            var model = new ArticleCategoryViewModel();
            model.Articles = new List<ArticleViewModel>();
            model.Categories = new List<CategoryViewModel>();

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
                        Content = article.Content
                    });
                }
            }

            return model;
        }

        public ArticleCategoryViewModel Search(string searchTerm)
        {
            var result = new ArticleCategoryViewModel();
            result.Articles = new List<ArticleViewModel>();
            result.Categories = new List<CategoryViewModel>();

            var articles = GetArticles();

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
                if (article.Title.Contains(searchTerm) 
                || article.Description.Contains(searchTerm) 
                || article.Author.Contains(searchTerm)
                || article.Content.Contains(searchTerm))
                {
                    result.Articles.Add(article);
                }
            }

            return result;
        }

        public void PublishArticle(int categoryId, string name, string author, string title, string description, string publishedAt, string content)
        {
            var articleToDb = new Article
            {
                CategoryId = categoryId,
                Name = name,
                Author = author,
                Title = title,
                Description = description,
                PublishedAt = publishedAt,
                Content = content
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

        public void EditArticle(int id, int categoryId, string name, string author, string title, string description, string publishedAt, string content)
        {
            var existArticle = _dataContext.Articles.First(a => a.Id == id);
            existArticle.CategoryId = categoryId;
            existArticle.Name = name;
            existArticle.Author = author;
            existArticle.Title = title;
            existArticle.Description = description;
            existArticle.PublishedAt = publishedAt;
            existArticle.Content = content;

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