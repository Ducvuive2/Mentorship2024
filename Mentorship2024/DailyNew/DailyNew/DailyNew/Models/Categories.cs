namespace DailyNew.Models
{
    public class Categories
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        // Navigation Properties
        public ICollection<RssFeeds> RssFeeds { get; set; }
    }
}
