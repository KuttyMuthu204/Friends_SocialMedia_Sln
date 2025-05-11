using Friends_App_Data.Data;
using Friends_App_Data.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Friends_App_Data.Services
{
    public class StoriesService : IStoriesService
    {
        private readonly AppDbContext _context;

        public StoriesService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Story>> GetAllStroiesAsync()
        {
            var allStories = await _context.Stories
                       .Where(n => n.DateCreated >= DateTime.UtcNow.AddHours(-24))
                       .Include(s => s.User)
                       .ToListAsync();

            return allStories;
        }

        public async Task<Story> CreateStoryAsync(Story story)
        {
            await _context.Stories.AddAsync(story);
            await _context.SaveChangesAsync();
            return story;
        }
    }
}
