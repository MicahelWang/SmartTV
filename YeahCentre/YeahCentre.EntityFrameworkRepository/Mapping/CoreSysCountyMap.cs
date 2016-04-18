using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahCentre.EntityFrameworkRepository.Mapping
{
    public class CoreSysCountyMap : EntityTypeConfiguration<CoreSysCounty>
    {
        public CoreSysCountyMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(20);

            this.Property(t => t.Code)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("CoreSysCounty", "YeahTV");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
        }
    }
}
