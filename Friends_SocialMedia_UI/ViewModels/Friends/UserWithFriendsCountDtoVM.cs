using System.Globalization;

namespace Friends_UI.ViewModels.Friends
{
    public class UserWithFriendsCountDtoVM
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public int FriendsCount { get; set; }
        public string FriendsCountDisplay => 
            FriendsCount == 0 ? "No followers" : FriendsCount > 1 ? "1 follower" : $"{FriendsCount} follower";
    }
}
