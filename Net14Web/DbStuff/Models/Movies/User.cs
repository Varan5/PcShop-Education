using Net14Web.DbStuff.Models.PcShop;

namespace Net14Web.DbStuff.Models.Movies
{
    public class User : BaseModel
    {
        public string? Login { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Password { get; set; }
        public string? AvatarUrl { get; set; }
        public string? PreferLocale { get; set; }
        public virtual List<Role> Roles { get; set; }
        public virtual List<Alert> SeenAlerts { get; set; }
        public virtual List<Alert> CreatedAlerts { get; set; }
        public virtual List<PCModel> PCModels { get; set; }
    }
}
