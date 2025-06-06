using Friends_Data.Data;
using Friends_Data.Data.Models;
using Friends_Data.Dtos;
using Friends_Data.Migrations;
using Friends_Data.Services;
using Microsoft.EntityFrameworkCore;

namespace Friends_Data.Services
{
    public class PostService : IPostService
    {
        private readonly AppDbContext _context;
        private readonly IFilesService _filesService;
        private readonly INotificationsService _notificationsService;

        public PostService(AppDbContext context, IFilesService filesService, INotificationsService notificationsService)
        {
            _context = context;
            _filesService = filesService;
            _notificationsService = notificationsService;
        }

        public async Task<List<Post>> GetAllPostsAsync(int loggedInUserId)
        {
            var posts = await _context.Posts
                              .Where(n => (!n.IsPrivate || n.UserId == loggedInUserId) && (n.Reports.Count < 5 && !n.IsDeleted))
                              .Include(u => u.User)
                              .Include(n => n.Likes)
                              .Include(f => f.Favorites)
                              .Include(n => n.Reports)
                              .Include(n => n.Comments)
                              .ThenInclude(n => n.User)
                              .OrderByDescending(o => o.DateCreated).ToListAsync();

            return posts;
        }

        public async Task<Post> GetPostByIdAsync(int postId)
        {
            var post = await _context.Posts
                              .Include(u => u.User)
                              .Include(n => n.Likes)
                              .Include(f => f.Favorites)
                              .Include(n => n.Comments).ThenInclude(n => n.User)
                              .FirstOrDefaultAsync(n => n.Id == postId);
                
            return post;
        }

        public async Task<List<Post>> GetAllFavoritePostsAsync(int loggedInUserId)
        {
            var allFavoritePosts = await _context.Favorites
                                          .Include(n => n.Post.Reports)
                                          .Include(n => n.Post.User)
                                          .Include(n => n.Post.Comments)
                                              .ThenInclude(n => n.User)
                                          .Include(n => n.Post.Likes)
                                          .Include(n => n.Post.Favorites)
                                          .Where(n => (n.UserId == loggedInUserId
                                                       && n.Post.Reports.Count < 5 &&
                                                       !n.Post.IsDeleted))
                                          .OrderByDescending(n => n.DateCreated)
                                          .Select(n => n.Post)
                                          .ToListAsync();

            return allFavoritePosts;
        }

        public async Task<Post> CreatePostAsync(Post post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task<Post> RemovePostAsync(int postId)
        {
            var postDB = await _context.Posts.FirstOrDefaultAsync(n => n.Id == postId);

            if (postDB != null)
            {
                //_context.Posts.Remove(postDB);
                postDB.IsDeleted = true;
                _context.Posts.Update(postDB);
                await _context.SaveChangesAsync();
            }

            return postDB;
        }

        public async Task RemovePostCommentAsync(int commentId)
        {
            var commentDb = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);

            if (commentDb != null)
            {
                _context.Comments.Remove(commentDb);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ReportPostAsync(int postId, int userId)
        {
            var newReport = new Report()
            {
                PostId = postId,
                UserId = userId,
                DateCreated = DateTime.UtcNow
            };

            await _context.Reports.AddAsync(newReport);
            await _context.SaveChangesAsync();

            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if (post != null)
            {
                post.NoOfReports += 1;
                _context.Posts.Update(post);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<GetNotificationDto> TogglePostFavoriteAsync(int postId, int userId)
        {
            var response = new GetNotificationDto()
            {
                SendNotification = false,
                Success = false
            };

            var favorites = await _context.Favorites.Where(l => l.PostId == postId && l.UserId == userId).FirstOrDefaultAsync();

            if (favorites != null)
            {
                _context.Favorites.Remove(favorites);
                await _context.SaveChangesAsync();
            }
            else
            {
                var newFavorite = new Favorite()
                {
                    PostId = postId,
                    UserId = userId,
                    DateCreated = DateTime.UtcNow
                };

                await _context.Favorites.AddAsync(newFavorite);
                await _context.SaveChangesAsync();

                response.SendNotification = true;
            }

            response.Success = true;
            return response;
        }

        public async Task<GetNotificationDto> TogglePostLikeAsync(int postId, int userId)
        {
            var response = new GetNotificationDto()
            {
                SendNotification = false,
                Success = false
            };

            //check if user has already liked the post
            var likes = await _context.Likes.Where(l => l.PostId == postId && l.UserId == userId).FirstOrDefaultAsync();

            if (likes != null)
            {
                _context.Likes.Remove(likes);
                await _context.SaveChangesAsync();
            }
            else
            {
                var newLikes = new Like()
                {
                    PostId = postId,
                    UserId = userId
                };

                await _context.Likes.AddAsync(newLikes);
                await _context.SaveChangesAsync();

                response.SendNotification = true;
            }

            response.Success = true;
            return response;
        }

        public async Task TogglePostVisibilityAsync(int postId, int userId)
        {
            //Chekc post by id and the loggedin user
            var post = await _context.Posts.Where(l => l.Id == postId && l.UserId == userId).FirstOrDefaultAsync();

            if (post != null)
            {
                post.IsPrivate = !post.IsPrivate;
                _context.Posts.Update(post);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddPostCommentAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task ApproveReportedPost(int postId)
        {
            var post = await GetPostByIdAsync(postId);

            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }

        public async Task CancelReportedPost(int postId)
        {
            var post = await GetPostByIdAsync(postId);

            if (post != null)
            {
                post.NoOfReports = 0;
                _context.Posts.Update(post);
                await _context.SaveChangesAsync();
            }
        }
    }
}
