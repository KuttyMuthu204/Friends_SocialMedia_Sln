using Friends_App_Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Friends_SocialMedia_UI.ViewComponent
{
    public class HastagsViewComponent : Microsoft.AspNetCore.Mvc.ViewComponent
    {
        private readonly AppDbContext _context;
        private readonly ILogger<HastagsViewComponent> _logger;

        public HastagsViewComponent(AppDbContext context, ILogger<HastagsViewComponent> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var oneWeekAgoNow = DateTime.UtcNow.AddDays(-7);

            var top3Hasttags = await _context.Hastags
                .Where(h => h.DateCreated >= oneWeekAgoNow)
                .OrderByDescending(n => n.Count)
                .Take(3).ToListAsync();
            return View(top3Hasttags);
        }
    }
}
