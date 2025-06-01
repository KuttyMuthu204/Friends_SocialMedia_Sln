using Friends_Data.Data;
using Friends_Data.Data.Models;
using Friends_Data.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Friends_Data.Services
{
    public class HashtagService : IHashtagService
    {
        private readonly AppDbContext _context;

        public HashtagService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddPostHashTagsAsync(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                //find and store the hashtags
                var postsHashtags = HastagHelper.GetHastags(content);

                foreach (var postHashtag in postsHashtags)
                {
                    var hashtagDb = await _context.Hastags.FirstOrDefaultAsync(n => n.Name == postHashtag);

                    if (hashtagDb != null)
                    {
                        hashtagDb.Count += 1;
                        hashtagDb.DateUpdated = DateTime.UtcNow;

                        _context.Hastags.Update(hashtagDb);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        var newHashtag = new Hastag()
                        {
                            Name = postHashtag,
                            Count = 1,
                            DateCreated = DateTime.UtcNow,
                            DateUpdated = DateTime.UtcNow
                        };

                        _context.Hastags.Add(newHashtag);
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }

        public async Task RemovePostHashTagsAsync(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                ////update hashtags
                var posthashtags = HastagHelper.GetHastags(content);
                foreach (var tag in posthashtags)
                {
                    var hastagdb = await _context.Hastags.FirstOrDefaultAsync(n => n.Name == tag);
                    if (hastagdb != null)
                    {
                        hastagdb.Count -= 1;
                        hastagdb.DateUpdated = DateTime.UtcNow;

                        _context.Hastags.Update(hastagdb);
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
