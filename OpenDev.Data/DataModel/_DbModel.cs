using Microsoft.EntityFrameworkCore;
namespace OpenDev.Data.DataModel
{
    public class DbModel:DbContext
    {
        public DbModel(DbContextOptions<DbModel> context) : base(context)
        {
        }
        public DbModel()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseNpgsql("Server=server.opndv.com;Port=5433;Database=opendev;User Id=postgres;Password=p4p8P4DEQb;");
                //Server=server.opndv.com;Port=5433;Database=opendev;User Id=Postgres;Password=p4p8P4DEQb;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("core");
            modelBuilder.UseSerialColumns();
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<App> AppList { get; set; }
        public DbSet<V_ColumnInfo> V_ColumnInfoList { get; set; }
        public DbSet<V_ForeignColumnInfo> V_ForeignColumnInfoList { get; set; }
        public DbSet<Form> FormList { get; set; }
        public DbSet<Cloud> CloudList { get; set; }
        public DbSet<Con> ConList { get; set; }
        public DbSet<ConType> ConTypeList { get; set; }
        public DbSet<Access> AccessList { get; set; }
        public DbSet<V_PrimaryColumnInfo> V_PrimaryColumnInfoList { get; set; }
        public DbSet<Setting> SettingList { get; set; }

    }
}
