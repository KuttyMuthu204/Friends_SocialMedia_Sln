using Friends_App_Data.Data.Models;

namespace Friends_Data.Data.Models
{
    public class FriendShip
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public int SenderId { get; set; }
        public virtual User Sender { get; set; }
        public int ReceiverId { get; set; }
        public virtual User Receiver { get; set; }
    }
}
