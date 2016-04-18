using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahCentre.EntityFrameworkRepository.Mapping
{
    public class CoreSysGroupMap : EntityTypeConfiguration<CoreSysGroup>
    {
        public CoreSysGroupMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.GroupCode)
                .HasMaxLength(20);

            this.Property(t => t.GroupName)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("CoreSysGroup", "YeahTV");
            this.Property(t => t.Id).HasColumnName("GroupId");
            this.Property(t => t.GroupCode).HasColumnName("GroupCode");
            this.Property(t => t.GroupName).HasColumnName("GroupName");
            this.Property(t => t.TemplateId).HasColumnName("TemplateId");
            this.Property(t => t.IsDelete).HasColumnName("IsDelete");
            this.Property(t => t.ApiKey).HasColumnName("ApiKey");
        }
    }
}
