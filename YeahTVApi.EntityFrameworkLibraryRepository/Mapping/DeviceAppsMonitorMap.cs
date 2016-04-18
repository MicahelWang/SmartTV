using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    public class DeviceAppsMonitorMap : EntityTypeConfiguration<DeviceAppsMonitor>
    {
        public DeviceAppsMonitorMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
               .IsRequired()
               .HasMaxLength(64);

            this.Property(t => t.DeviceSeries)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.Action)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.PackageName)
                .HasMaxLength(45);

            this.Property(t => t.VersionName)
               .IsRequired()
               .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("DeviceAppsMonitor", "YeahTV");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.DeviceSeries).HasColumnName("DeviceSeries");
            this.Property(t => t.Action).HasColumnName("Action");
            this.Property(t => t.PackageName).HasColumnName("PackageName");
            this.Property(t => t.VersionCode).HasColumnName("VersionCode");
            this.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.VersionName).HasColumnName("VersionName");
        }
    }
}
