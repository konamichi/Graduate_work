using NewsReader.Data;
using NewsReader.Models;

namespace NewsReader.Repositories
{
    public class NewsRepository
    {
        private readonly DataContext _dataContext;

        public NewsRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public List<Article> GetAllArticles()
        {
            return _dataContext.Articles.ToList();
        }

        public List<Category> GetAllCategories()
        {
            return _dataContext.Categories.ToList();
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