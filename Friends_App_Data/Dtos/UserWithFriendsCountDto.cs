using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Friends_App_Data.Data.Models;

namespace Friends_Data.Dtos
{
    public class UserWithFriendsCountDto
    {
        public User User { get; set; }
        public int FriendsCount { get; set; }
    }
}
