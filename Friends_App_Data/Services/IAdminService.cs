using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Friends_Data.Data.Models;

namespace Friends_Data.Services
{
    public interface IAdminService
    {
        Task<List<Post>> GetReportedPostsAsync();
    }
}
