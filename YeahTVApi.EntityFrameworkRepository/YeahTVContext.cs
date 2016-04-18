namespace YeahTVApi.EntityFrameworkRepository.Models
{
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.EntityFrameworkRepository.Mapping;
    using YeahTVApiLibrary.EntityFrameworkRepository.Models;
    using System.Data.Entity;

    public sealed partial class YeahTVContext : YeahTVLibraryContext
    {
        public YeahTVContext()
            : base("Name=YeahTVContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public YeahTVContext(string connectionStrings)
            : base(connectionStrings)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<AppConfig> AppConfigs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new AppConfigMap());
        }
    }
}
