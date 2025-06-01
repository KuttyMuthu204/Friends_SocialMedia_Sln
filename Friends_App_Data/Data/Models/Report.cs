namespace Friends_Data.Data.Models
{
    public class Report
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
