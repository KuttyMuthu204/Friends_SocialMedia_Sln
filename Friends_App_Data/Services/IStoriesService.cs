using Friends_App_Data.Data.Models;

namespace Friends_App_Data.Services
{
    public interface IStoriesService
    {
        Task<List<Story>> GetAllStroiesAsync();
        Task<Story> CreateStoryAsync(Story story);
    }
}
