using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Friends_App_Data.Data;
using Friends_App_Data.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Friends_App_Data.Services
{
    public class UsersService : IUsersService
    {
        private readonly AppDbContext _context;

        public UsersService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUser(int loggedInUserId)
        {
            return await _context.Users
                .Where(u => u.Id == loggedInUserId)
                .FirstOrDefaultAsync() ?? new User();
        }
    }
}
