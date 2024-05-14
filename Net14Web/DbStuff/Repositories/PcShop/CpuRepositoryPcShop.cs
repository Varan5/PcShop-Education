using Microsoft.EntityFrameworkCore;
using Net14Web.DbStuff.Models.PcShop;

namespace Net14Web.DbStuff.Repositories.PcShop
{
    public class CpuRepositoryPcShop : BaseRepository<CpuModel>
    {
        public CpuRepositoryPcShop(WebDbContext context) : base(context) { }
        public IEnumerable<CpuModel> GetCpu(int maxCount = 10)
        {
            return _context.CpuModel.Take(maxCount).ToList();
        }
    }
}