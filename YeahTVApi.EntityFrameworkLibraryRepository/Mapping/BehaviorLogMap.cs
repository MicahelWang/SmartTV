using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    public class BehaviorLogMap : EntityTypeConfiguration<BehaviorLog>
    {
        public BehaviorLogMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.BehaviorInfo)
                .IsRequired()
                .HasMaxLength(65535);

            this.Property(t => t.DeviceSerise)
                .HasMaxLength(50);

            this.Property(t => t.BehaviorType)
                .HasMaxLength(50);

            this.Property(t => t.HotelId)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("BehaviorLog", "YeahTV");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.BehaviorInfo).HasColumnName("BehaviorInfo");
            this.Property(t => t.HotelId).HasColumnName("HotelId");
            this.Property(t => t.BehaviorType).HasColumnName("BehaviorType");
            this.Property(t => t.DeviceSerise).HasColumnName("DeviceSerise");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
