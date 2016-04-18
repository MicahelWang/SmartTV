namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    using YeahTVApi.DomainModel.Models;
    using System.Data.Entity.ModelConfiguration;

    public class SystemConfigMap : EntityTypeConfiguration<SystemConfig>
    {
        public SystemConfigMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties

            this.Property(t => t.ConfigType)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.ConfigName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ConfigValue)
                .HasMaxLength(4000);

            this.Property(t => t.AppId)
              .HasMaxLength(64);

            // Table & Column Mappings
            this.ToTable("SystemConfig");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ConfigType).HasColumnName("ConfigType");
            this.Property(t => t.ConfigName).HasColumnName("ConfigName");
            this.Property(t => t.ConfigValue).HasColumnName("ConfigValue");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.LastUpdateTime).HasColumnName("LastUpdateTime");
            this.Property(t => t.AppId).HasColumnName("AppId");
        }
    }
}
