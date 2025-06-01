using Friends_Data.Data.Models;

namespace Friends_Data.Services
{
    public interface IStoriesService
    {
        Task<List<Story>> GetAllStroiesAsync();
        Task<Story> CreateStoryAsync(Story story);
    }
}
