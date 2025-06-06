using Friends_Data.Data.Models;
using Friends_Data.Dtos;

namespace Friends_Data.Services
{
    public interface IPostService
    {
        Task<List<Post>> GetAllPostsAsync(int loggedInUserId);
        Task<List<Post>> GetAllFavoritePostsAsync(int loggedInUserId);
        Task<Post> CreatePostAsync(Post post);
        Task<Post> RemovePostAsync(int postId);

        Task ReportPostAsync(int postId, int userId);
        Task AddPostCommentAsync(Comment comment);
        Task RemovePostCommentAsync(int commentId);

        Task<GetNotificationDto> TogglePostLikeAsync(int postId, int userId);
        Task<GetNotificationDto> TogglePostFavoriteAsync(int postId, int userId);
        Task TogglePostVisibilityAsync(int postId, int userId);
        Task<Post> GetPostByIdAsync(int postId);
        Task ApproveReportedPost(int postId);
        Task CancelReportedPost(int postId);
    }
}
