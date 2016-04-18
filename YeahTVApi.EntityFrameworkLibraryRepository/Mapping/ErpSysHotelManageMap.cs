using System.Data.Entity.ModelConfiguration;
using YeahTVApi.DomainModel.Models.DomainModels;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Mapping
{
    public class ErpSysHotelManageMap : EntityTypeConfiguration<ErpSysHotelManage>
    {
        public ErpSysHotelManageMap()
        {
            // Primary Key
            this.HasKey(t => new { t.HotelId, t.UserId });

            // Properties
            this.Property(t => t.HotelId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.UserId)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ErpSysHotelManage", "YeahTV");
            this.Property(t => t.HotelId).HasColumnName("HotelId");
            this.Property(t => t.UserId).HasColumnName("UserId");
        }
    }
}
