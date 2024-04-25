namespace NewsReader.ViewModels 
{
    public class ArticleViewModel
    {
        public int? Id { get; set; }
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Author { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? PublishedAt { get; set; }
        public string? Content { get; set; }
        public string? Url { get; set; }
        public string? UrlToImage { get; set; }
    }
}