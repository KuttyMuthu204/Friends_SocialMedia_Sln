using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Friends_App_Data.Data.Models;
using Microsoft.AspNetCore.Http;

namespace Friends_App_Data.Services
{
    public interface IPostService
    {
        Task<List<Post>> GetAllPostsAsync(int loggedInUserId);
        Task<Post> CreatePostAsync(Post post, IFormFile Image);
        Task<Post> RemovePostAsync(int postId);

        Task ReportPostAsync(int postId, int userId);
        Task AddPostCommentAsync(Comment comment);
        Task RemovePostCommentAsync(int commentId);

        Task TogglePostLikeAsync(int postId, int userId);
        Task TogglePostFavoriteAsync(int postId, int userId);
        Task TogglePostVisibilityAsync(int postId, int userId);
    }
}
 