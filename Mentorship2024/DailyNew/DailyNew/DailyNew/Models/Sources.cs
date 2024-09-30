namespace DailyNew.Models
{
    public class Sources
    {
        public int SourceId { get; set; }
        public string Name { get; set; }
        public string WebsiteUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<RssFeeds> RssFeeds { get; set; }
    }

}
