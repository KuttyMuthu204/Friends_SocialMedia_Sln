﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friends_App_Data.Data.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }

        //Navigatianal properties
        public Post Post { get; set; }
        public User User { get; set; }
    }
}
