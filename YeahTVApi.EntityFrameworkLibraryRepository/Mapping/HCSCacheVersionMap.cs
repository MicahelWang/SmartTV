using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    public class HCSCacheVersionMap : EntityTypeConfiguration<HCSCacheVersion>
    {
        public HCSCacheVersionMap()
        {
            this.HasKey(t => new { t.Id });


            this.ToTable("HCSCacheVersion", "YeahTV");

            this.Property(t => t.PermitionType).HasMaxLength(45);
            this.Property(t => t.TypeId).HasMaxLength(64);
            this.Property(t => t.LastUpdateUser).HasMaxLength(64);

            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.PermitionType).HasColumnName("PermitionType");
            this.Property(t => t.TypeId).HasColumnName("TypeId");
            this.Property(t => t.LastUpdateUser).HasColumnName("LastUpdateUser");
            this.Property(t => t.LastUpdateTime).HasColumnName("LastUpdateTime");

        }
    }
}
