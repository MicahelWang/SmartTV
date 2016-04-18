using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahTVApi.DomainModel.Models
{
    public class LocalizeResourceMap : EntityTypeConfiguration<LocalizeResource>
    {
        public LocalizeResourceMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Id, t.Lang });

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(64);

            this.Property(t => t.Lang)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.Content)
                .HasMaxLength(65535);
            

            // Table & Column Mappings
            this.ToTable("LocalizeResource", "YeahTV");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Lang).HasColumnName("Lang");
            this.Property(t => t.Content).HasColumnName("Content");
        }
    }
}
