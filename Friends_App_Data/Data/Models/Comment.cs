namespace Friends_App_Data.Data.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        //Foriegn Keys
        public int PostId { get; set; }
        public int UserId { get; set; }

        //Navigatianal properties
        public Post Post { get; set; }
        public User User { get; set; }
    }
}
