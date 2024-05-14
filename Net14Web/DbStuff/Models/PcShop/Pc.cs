namespace Net14Web.DbStuff.Models.PcShop
{
    public class Pc: BaseModel
    {
        public PCModel PCModel { get; set; }

        public int CPUId { get; set; }
        public CpuModel? CPU { get; set; }

    }
}
