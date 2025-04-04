﻿using System.ComponentModel.DataAnnotations;

namespace Friends_App_Data.Data.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string? Content { get; set; }
        public string? ImageUrl { get; set; }
        public int NoOfReports { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        //Foreign key
        public int UserId { get; set; }

        //Navigation properties
        public User User { get; set; }

        public ICollection<Like> Likes { get; set; } = new List<Like>();

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
