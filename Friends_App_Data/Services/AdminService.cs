using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Friends_Data.Data;
using Friends_Data.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Friends_Data.Services
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _context;

        public AdminService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Post>> GetReportedPostsAsync()
        {
            return await _context.Posts
                .Include(p => p.User)
                .Where(p => p.NoOfReports > 5 && !p.IsDeleted).ToListAsync();
        }
    }
}
