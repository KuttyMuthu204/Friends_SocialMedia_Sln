using Friends_App_Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace Friends_SocialMedia_UI.ViewComponent
{
    public class StoriesViewComponent : Microsoft.AspNetCore.Mvc.ViewComponent
    {
        private readonly IStoriesService _storiesService;

        public StoriesViewComponent(IStoriesService storiesService)
        {
            _storiesService = storiesService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var allStories =  await _storiesService.GetAllStroiesAsync();
            return View(allStories);
        }
    }
}
