using Friends_Data.Data.Models;

namespace Friends_UI.ViewModels.Friends
{
    public class FriendShipVM
    {
        public List<FriendRequest> FriendRequestsSent  = new List<FriendRequest>();
        public List<FriendRequest> FriendRequestsReceived = new List<FriendRequest>();
    }
}
