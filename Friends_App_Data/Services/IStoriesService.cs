using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Friends_App_Data.Data.Models;
using Microsoft.AspNetCore.Http;

namespace Friends_App_Data.Services
{
    public interface IStoriesService
    {
        Task<List<Story>> GetAllStroiesAsync();
        Task<Story> CreateStoryAsync(Story story);
    }
}
