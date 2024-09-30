namespace DailyNew.Models
{
    public class Articles
    {
        public int ArticleId { get; set; }
        public int RssFeedId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime PublishedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation property to Source
        public RssFeeds RssFeeds { get; set; }
    }
}
