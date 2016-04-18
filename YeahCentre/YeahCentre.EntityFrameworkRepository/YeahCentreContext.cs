using System.Data.Entity;
using YeahCentre.EntityFrameworkRepository.Mapping;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.DomainModels;
using YeahTVApiLibrary.EntityFrameworkRepository.Models;

namespace YeahCentre.EntityFrameworkRepository
{
    public partial class YeahCentreContext : YeahTVLibraryContext
    {
        static YeahCentreContext()
        {
            Database.SetInitializer<YeahCentreContext>(null);
        }

        public YeahCentreContext()
            : base("Name=YeahApi")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        public YeahCentreContext(string connectionStrings)
            : base(connectionStrings)
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }
       
        public DbSet<CoreSysBrand> CoreSysBrands { get; set; }
        public DbSet<CoreSysGroup> CoreSysGroups { get; set; }
        public DbSet<CoreSysHotel> CoreSysHotels { get; set; }
        public DbSet<CoreSysHotelSencond> CoreSysHotelSenconds { get; set; }
        
        
       
        public DbSet<TvTemplate> TvTemplates { get; set; }
        public DbSet<TvTemplateType> TvTemplateTypes { get; set; }
        public DbSet<TvTemplateAttribute> TvTemplateAttributes { get; set; }
        public DbSet<TvTemplateElement> TvTemplateElements { get; set; }

        public DbSet<TvDocumentElement> TvDocumentElements { get; set; }
        public DbSet<TvDocumentAttribute> TvDocumentAttributes { get; set; }

        public DbSet<CoreSysProvince> CoreSysProvinces { get; set; }
        public DbSet<CoreSysCity> CoreSysCitys { get; set; }
        public DbSet<CoreSysCounty> CoreSysCountys { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new CoreSysBrandMap());
            modelBuilder.Configurations.Add(new CoreSysGroupMap());
            
            modelBuilder.Configurations.Add(new CoreSysHotelMap());
            modelBuilder.Configurations.Add(new CoreSysHotelSencondMap());
           
           
            modelBuilder.Configurations.Add(new TvTemplateMap());
            modelBuilder.Configurations.Add(new TvTemplateTypeMap());
            modelBuilder.Configurations.Add(new TvTemplateElementMap());
            modelBuilder.Configurations.Add(new TvTemplateAttributeMap());
            modelBuilder.Configurations.Add(new TvDocumentElementMap());
            modelBuilder.Configurations.Add(new TvDocumentAttributeMap());
            modelBuilder.Configurations.Add(new CoreSysProvinceMap());
            modelBuilder.Configurations.Add(new CoreSysCityMap());
            modelBuilder.Configurations.Add(new CoreSysCountyMap());
        }
    }
}

