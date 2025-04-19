using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friends_App_Data.Services
{
    public interface IHashtagService
    {
        Task AddPostHashTagsAsync(string content);
        Task RemovePostHashTagsAsync(string content);
    }
}
