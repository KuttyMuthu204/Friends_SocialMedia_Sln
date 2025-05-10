using Friends_App_Data.Data.Models;

namespace Friends_UI.ViewModels.User
{
    public class GetUserProfileVM
    {
        public List<Post> Posts { get; set; }
        public Friends_App_Data.Data.Models.User User { get; set; }
    }
}
