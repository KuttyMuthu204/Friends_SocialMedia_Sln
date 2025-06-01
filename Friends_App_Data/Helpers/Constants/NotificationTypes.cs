using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Friends_Data.Helpers.Constants
{
    public class NotificationTypes
    {
        public const string Like = "Like";
        public const string Comment = "Comment";
        public const string Favorite = "Favorite";
        public const string Follow = "Follow";
        public const string Mention = "Mention";
        public const string Share = "Share";
        public const string Report = "Report";
        public const string PostCreated = "PostCreated";
        public const string PostUpdated = "PostUpdated";
        public const string PostDeleted = "PostDeleted";
        public const string FriendRequest = "FriendRequest";
        public const string FriendRequestAccepted = "FriendRequestAccepted";
        public const string FriendRequestRejected = "FriendRequestRejected";
    }
}
