namespace YeahTVApi.EntityFrameworkRepository.Mapping
{
    
    using YeahTVApi.DomainModel.Models;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    public class AppConfigMap : EntityTypeConfiguration<AppConfig>
    {
        public AppConfigMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.AppId)
                .HasMaxLength(64);

            this.Property(t => t.VersionId)
                .HasMaxLength(64);

            this.Property(t => t.ConfigCode)
                .IsRequired()
                .HasMaxLength(32);

            this.Property(t => t.ConfigName)
                .IsRequired()
                .HasMaxLength(32);

            this.Property(t => t.ConfigValue)
                .IsRequired()
                .HasMaxLength(1280);

            this.Property(t => t.LastUpdater)
                .HasMaxLength(32);

            // Table & Column Mappings
            this.ToTable("AppConfig");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.AppId).HasColumnName("AppId");
            this.Property(t => t.VersionId).HasColumnName("VersionId");
            this.Property(t => t.ConfigCode).HasColumnName("ConfigCode");
            this.Property(t => t.ConfigName).HasColumnName("ConfigName");
            this.Property(t => t.ConfigValue).HasColumnName("ConfigValue");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.LastUpdater).HasColumnName("LastUpdater");
            this.Property(t => t.LastUpdateTime).HasColumnName("LastUpdateTime");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
