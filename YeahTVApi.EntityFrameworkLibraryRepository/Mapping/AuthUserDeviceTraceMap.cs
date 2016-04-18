namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    
    using YeahTVApi.DomainModel.Models;
    using System.Data.Entity.ModelConfiguration;

    public class AuthUserDeviceTraceMap : EntityTypeConfiguration<AuthUserDeviceTrace>
    {
        public AuthUserDeviceTraceMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.UserId)
                .HasMaxLength(50);

            this.Property(t => t.DeviceNo)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("AuthUserDeviceTrace", "YeahTV");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.DeviceNo).HasColumnName("DeviceNo");
            this.Property(t => t.BindTime).HasColumnName("BindTime");
            this.Property(t => t.LastVisitTime).HasColumnName("LastVisitTime");
        }
    }
}
