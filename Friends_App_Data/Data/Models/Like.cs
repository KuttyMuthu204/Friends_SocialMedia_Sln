namespace Friends_App_Data.Data.Models
{
    public class Like
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }

        //Navigatianal properties
        public Post Post { get; set; }
        public User User { get; set; }
    }
}
