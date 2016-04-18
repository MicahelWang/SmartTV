namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    
    using YeahTVApi.DomainModel.Models;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.ModelConfiguration;

    public class DeviceTraceMap : EntityTypeConfiguration<DeviceTrace>
    {
        public DeviceTraceMap()
        {
            // Primary Key
            this.HasKey(t => new { t.DeviceSeries });
            this.Ignore(t => t.Id);

            // Properties
            this.Property(t => t.DeviceSeries)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.DeviceKey)
                .HasMaxLength(64);

            this.Property(t => t.Ip)
                .HasMaxLength(64);

            this.Property(t => t.Platfrom)
                .HasMaxLength(64);

            this.Property(t => t.Brand)
                .HasMaxLength(64);

            this.Property(t => t.Manufacturer)
                .HasMaxLength(64);

            this.Property(t => t.Model)
                .HasMaxLength(64);

            this.Property(t => t.OsVersion)
                .HasMaxLength(64);

            this.Property(t => t.HotelId)
                .HasMaxLength(50);

            this.Property(t => t.RoomNo)
                .HasMaxLength(20);

            this.Property(t => t.Remark)
                .HasMaxLength(512);

            this.Property(t => t.Token)
                .HasMaxLength(1073741823);

            this.Property(t => t.GuestId)
                .HasMaxLength(1073741823);

            this.Property(t => t.GroupId)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("DeviceTrace", "YeahTV");
            this.Property(t => t.DeviceSeries).HasColumnName("DeviceSeries");
            this.Property(t => t.FirstVisitTime).HasColumnName("FirstVisitTime");
            this.Property(t => t.LastVisitTime).HasColumnName("LastVisitTime");
            this.Property(t => t.DeviceKey).HasColumnName("DeviceKey");
            this.Property(t => t.Ip).HasColumnName("Ip");
            this.Property(t => t.Platfrom).HasColumnName("Platfrom");
            this.Property(t => t.Brand).HasColumnName("Brand");
            this.Property(t => t.Manufacturer).HasColumnName("Manufacturer");
            this.Property(t => t.Model).HasColumnName("Model");
            this.Property(t => t.OsVersion).HasColumnName("OsVersion");
            this.Property(t => t.HotelId).HasColumnName("HotelId");
            this.Property(t => t.RoomNo).HasColumnName("RoomNo");
            this.Property(t => t.ModelId).HasColumnName("ModelId");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.Token).HasColumnName("Token");
            this.Property(t => t.GuestId).HasColumnName("GuestId");
            this.Property(t => t.GroupId).HasColumnName("GroupId");
            this.Property(t => t.Attachments).HasColumnName("Attachments");
            this.Property(t => t.DeviceType).HasColumnName("DeviceType");
        }
    }
}
