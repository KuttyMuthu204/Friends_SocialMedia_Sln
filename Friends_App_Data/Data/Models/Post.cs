using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Friends_App_Data.Data.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }
        public string? Content { get; set; }
        public string? ImageUrl { get; set; }
        public int NoOfReports { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
