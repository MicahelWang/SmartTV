namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    using YeahTVApi.DomainModel.Models;
    using System.Data.Entity.ModelConfiguration;

    public class AppVersionMap : EntityTypeConfiguration<AppVersion>
    {
        public AppVersionMap()
        {
            this.HasKey(t => new { t.VersionCode ,t.Id});

            // Properties
            this.Property(t => t.Id)
                .HasMaxLength(128);

            this.Property(t => t.LastUpdater)
                .HasMaxLength(32);

            this.Property(t => t.AppUrl)
                .HasMaxLength(128);

            this.Property(t => t.Description)
                .HasMaxLength(128);

            this.Property(t => t.VersionName)
             .HasMaxLength(100);

            this.HasRequired(t => t.App)
                .WithMany(t => t.AppVresions)
                .HasForeignKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("AppVersion");
            this.Property(t => t.Id).HasColumnName("AppId");
            this.Property(t => t.VersionCode).HasColumnName("VersionCode");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.LastUpdater).HasColumnName("LastUpdater");
            this.Property(t => t.LastUpdateTime).HasColumnName("LastUpdateTime");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.AppUrl).HasColumnName("AppUrl");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.FileName).HasColumnName("FileName");
            this.Property(t => t.VersionName).HasColumnName("VersionName");
        }
    }
}
