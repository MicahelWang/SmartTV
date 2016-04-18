using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahCentre.EntityFrameworkRepository.Mapping
{
    public class CoreSysBrandMap : EntityTypeConfiguration<CoreSysBrand>
    {
        public CoreSysBrandMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.BrandName)
                .HasMaxLength(20);

            this.Property(t => t.BrandCode)
                .HasMaxLength(20);

            this.Property(t => t.GroupId)
                .HasMaxLength(50);

            this.Property(t => t.Logo)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("CoreSysBrand", "YeahTV");
            this.Property(t => t.Id).HasColumnName("BrandId");
            this.Property(t => t.BrandName).HasColumnName("BrandName");
            this.Property(t => t.BrandCode).HasColumnName("BrandCode");
            this.Property(t => t.GroupId).HasColumnName("GroupId");
            this.Property(t => t.TemplateId).HasColumnName("TemplateId");
            this.Property(t => t.Logo).HasColumnName("Logo");
            this.Property(t => t.IsDelete).HasColumnName("IsDelete");

            this.HasRequired(m => m.CoreSysGroup)
                .WithMany(m => m.CoreSysBrands)
                .HasForeignKey(m => m.GroupId);
        }
    }
}
