using Net14Web.DbStuff.Models.Movies;

namespace Net14Web.DbStuff.Models.PcShop
{
    public class PCModel : BaseModel
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public string? ModelFromManufacturer { get; set; }

        public List<Pc> PCs { get; set;}
        public virtual List<User> Users { get; set; }
    }

}
