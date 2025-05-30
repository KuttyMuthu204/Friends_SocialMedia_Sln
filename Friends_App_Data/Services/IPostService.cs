﻿using Friends_App_Data.Data.Models;

namespace Friends_App_Data.Services
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

        Task TogglePostLikeAsync(int postId, int userId);
        Task TogglePostFavoriteAsync(int postId, int userId);
        Task TogglePostVisibilityAsync(int postId, int userId);
        Task<Post> GetPostByIdAsync(int postId); 
    }
}
