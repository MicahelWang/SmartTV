using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    public class BackupDeviceMap : EntityTypeConfiguration<BackupDevice>
    {
        public BackupDeviceMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.DeviceSeries)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.HotelId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.LastUpdatUser)
                .IsRequired()
                .HasMaxLength(45);
            this.Property(t => t.Model).HasMaxLength(45);

            // Table & Column Mappings
            this.ToTable("BackupDevice", "YeahTV");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.DeviceSeries).HasColumnName("DeviceSeries");
            this.Property(t => t.HotelId).HasColumnName("HotelId");
            this.Property(t => t.LastUpdateTime).HasColumnName("LastUpdateTime");
            this.Property(t => t.LastUpdatUser).HasColumnName("LastUpdatUser");
            this.Property(t => t.Model).HasColumnName("Model");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.IsUsed).HasColumnName("IsUsed");
            this.Property(t => t.DeviceType).HasColumnName("DeviceType");
        }
    }
}
