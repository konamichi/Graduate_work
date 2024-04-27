using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NewsReader.ViewModels;
using NewsReader.Services;
using System.Text.RegularExpressions;

namespace NewsReader.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly Service _newsReader;

    public HomeController(ILogger<HomeController> logger, Service newsReader)
    {
        _logger = logger;
        _newsReader = newsReader;
    }

    public IActionResult Index(string searchTerm)
    { 
        var articles = new ArticleCategoryViewModel();  

        if (string.IsNullOrEmpty(searchTerm))
            articles = _newsReader.GetArticlesWithCategories();
        else
            articles = _newsReader.Search(searchTerm);

        return View(articles);
    }

    [HttpPost]
    public IActionResult Download(string q, string category)
    {
        var articles = _newsReader.DownloadArticlesFromApi(q);
        if (articles != null)
            _newsReader.LoadArticles(articles, category);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult CreateArticle(string category, string name, string author, string title, string description, string publishedAt, string content, string? url, string? urlToImage)
    {
        var existCategory = _newsReader.GetCategory(category);

        _newsReader.PublishArticle(existCategory.Id, name, author, title, description, publishedAt, content, url, urlToImage);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult ChangeArticle(int id, int categoryId, string name, string author, string title, string description, string publishedAt, string content, string? url, string? urlToImage)
    {
        _newsReader.EditArticle(id, categoryId, name, author, title, description, publishedAt, content, url, urlToImage);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        _newsReader.DeleteArticle(id);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Search(string searchTerm)
    {
        return RedirectToAction("Index", new { searchTerm });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
