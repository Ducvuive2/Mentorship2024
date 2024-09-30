using DailyNew.Models;
using Microsoft.EntityFrameworkCore;

namespace DailyNew.Data
{
    public class DailyNewContext : DbContext
    {
        public DailyNewContext(DbContextOptions<DailyNewContext> options) : base(options)
        {
        }

        public DbSet<Sources> Sources { get; set; }
        public DbSet<Articles> Articles { get; set; }
        public DbSet<RssFeeds> RssFeeds { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define primary keys
            modelBuilder.Entity<Sources>().HasKey(s => s.SourceId);
            modelBuilder.Entity<Articles>().HasKey(a => a.ArticleId);
            modelBuilder.Entity<RssFeeds>().HasKey(a => a.RssFeedId);
            modelBuilder.Entity<Categories>().HasKey(a => a.CategoryId);

            modelBuilder.Entity<RssFeeds>()
            .HasOne(r => r.Sources)
            .WithMany(s => s.RssFeeds)
            .HasForeignKey(r => r.SourceId);

            modelBuilder.Entity<RssFeeds>()
                .HasOne(r => r.Category)
                .WithMany(c => c.RssFeeds)
                .HasForeignKey(r => r.CategoryId);

            modelBuilder.Entity<Articles>()
                .HasOne(a => a.RssFeeds)
                .WithMany(r => r.Articles)
                .HasForeignKey(a => a.RssFeedId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
