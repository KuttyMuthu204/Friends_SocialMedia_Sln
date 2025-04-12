using Friends_App_Data.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Friends_App_Data.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Story> Stories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Posts)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Stories)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<Like>()
                .HasKey(l => new { l.PostId, l.UserId });

            modelBuilder.Entity<Like>()
                .HasOne(l => l.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Like>()
              .HasOne(l => l.User)
              .WithMany(p => p.Likes)
              .HasForeignKey(l => l.UserId)
              .OnDelete(DeleteBehavior.Restrict);

            //Comments
            modelBuilder.Entity<Comment>()
              .HasOne(l => l.Post)
              .WithMany(p => p.Comments)
              .HasForeignKey(l => l.PostId)
              .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Comment>()
              .HasOne(l => l.User)
              .WithMany(p => p.Comments)
              .HasForeignKey(l => l.UserId)
              .OnDelete(DeleteBehavior.Restrict);

            //Favorites
            modelBuilder.Entity<Favorite>()
              .HasKey(l => new { l.PostId, l.UserId });

            modelBuilder.Entity<Favorite>()
                .HasOne(l => l.Post)
                .WithMany(p => p.Favorites)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Favorite>()
               .HasOne(l => l.User)
               .WithMany(p => p.Favorites)
               .HasForeignKey(l => l.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            //Reports
            modelBuilder.Entity<Report>()
                .HasKey(l => new { l.PostId, l.UserId });

            modelBuilder.Entity<Report>()
                .HasOne(l => l.Post)
                .WithMany(p => p.Reports)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Report>()
               .HasOne(l => l.User)
               .WithMany(p => p.Reports)
               .HasForeignKey(l => l.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
