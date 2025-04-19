using Friends_App_Data.Data;
using Friends_SocialMedia_UI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Friends_SocialMedia_UI.ViewComponent
{
    public class StoriesViewComponent : Microsoft.AspNetCore.Mvc.ViewComponent
    {
        private readonly AppDbContext _context;
        private readonly ILogger<StoriesController> _logger;

        public StoriesViewComponent(AppDbContext context, ILogger<StoriesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var allStories = await _context.Stories
                                   .Where(n => n.DateCreated >= DateTime.UtcNow.AddHours(-24))
                                   .Include(s => s.User)
                                   .ToListAsync();
            return View(allStories);
        }
    }
}
