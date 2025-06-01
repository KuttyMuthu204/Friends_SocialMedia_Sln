namespace Friends_Data.Services
{
    public interface IHashtagService
    {
        Task AddPostHashTagsAsync(string content);
        Task RemovePostHashTagsAsync(string content);
    }
}
