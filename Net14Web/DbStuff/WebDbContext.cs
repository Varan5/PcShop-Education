using Microsoft.EntityFrameworkCore;
using Net14Web.DbStuff.Models;
using Net14Web.DbStuff.Models.Movies;
using Net14Web.DbStuff.Models.PcShop;
using System.Reflection.Metadata;

namespace Net14Web.DbStuff
{
    public class WebDbContext : DbContext
    {
        public DbSet<PCModel> PCModel { get; set; }
        public DbSet<CpuModel> CpuModel { get; set; }
        public DbSet<Pc> PCs { get; set; }
       
       
        public DbSet<User> Users { get; set; }
        
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        
        // LifeScore
       
        public DbSet<Alert> Alerts { get; set; }

        public WebDbContext(DbContextOptions<WebDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .Entity<PCModel>()
                .HasMany(PCModel => PCModel.PCs)
                .WithOne(PC => PC.PCModel)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Entity<CpuModel>()
                .HasOne(CpuModel => CpuModel.Pc)
                .WithOne(PC => PC.CPU)
                .HasForeignKey<Pc>(e => e.CPUId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Entity<User>()
                .HasMany(User => User.PCModels)
                .WithMany(PCModel => PCModel.Users);

            builder.Entity<Role>()
                .HasMany(role => role.Permissions)
                .WithMany(permission => permission.Roles);

           

            builder.Entity<Alert>()
                .HasMany(user => user.NotifiedUsers)
                .WithMany(alert => alert.SeenAlerts);

            builder.Entity<Alert>()
               .HasOne(user => user.Creater)
               .WithMany(alert => alert.CreatedAlerts)
               .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
