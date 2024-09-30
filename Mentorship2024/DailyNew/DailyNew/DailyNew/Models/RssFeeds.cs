namespace DailyNew.Models
{
    public class RssFeeds
    {
        public int RssFeedId { get; set; }
        public int SourceId { get; set; }
        public int CategoryId { get; set; }
        public string RssFeedUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation property to Source
        public ICollection<Articles> Articles { get; set; }
        public Categories Category { get; set; }
        public Sources Sources { get; set; }
    }
}
