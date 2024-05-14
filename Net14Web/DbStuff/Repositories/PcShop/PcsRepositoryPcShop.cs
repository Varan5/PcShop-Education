using Microsoft.EntityFrameworkCore;
using Net14Web.DbStuff.Models.PcShop;

namespace Net14Web.DbStuff.Repositories.PcShop
{
    public class PcsRepositoryPcShop : BaseRepository<Pc>
    {
        public PcsRepositoryPcShop(WebDbContext context) : base(context) { }
        public IEnumerable<Pc> GetPCs(int maxCount = 10)
        {
            return _context.PCs
                .Include(x => x.CPU)
                .Include(x => x.PCModel)
                .Take(maxCount)
                .ToList();
        }
    }
}