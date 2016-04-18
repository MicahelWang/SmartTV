namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    
    using YeahTVApi.DomainModel.Models;
    using System.Data.Entity.ModelConfiguration;

    public class AppPublishMap : EntityTypeConfiguration<AppPublish>
    {
        public AppPublishMap()
        {
            // Primary Key
            this.HasKey(t => new { t.VersionCode, t.Id, t.HotelId });

            // Properties
            this.Property(t => t.Id) 
                .IsRequired()
                .HasMaxLength(64);

            this.Property(t => t.HotelId)
                .HasMaxLength(50);

            this.Property(t => t.LastUpdater)
                .HasMaxLength(32);

            this.HasRequired(t => t.AppVersion)
                .WithMany(t => t.AppPublishs)
                .HasForeignKey(t => new { t.VersionCode, t.Id });

            // Table & Column Mappings
            this.ToTable("AppPublish");
            this.Property(t => t.Id) .HasColumnName("AppId");
            this.Property(t => t.VersionCode).HasColumnName("VersionCode");
            this.Property(t => t.PublishDate).HasColumnName("PublishDate");
            this.Property(t => t.HotelId).HasColumnName("HotelId");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.LastUpdater).HasColumnName("LastUpdater");
            this.Property(t => t.LastUpdateTime).HasColumnName("LastUpdateTime");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
